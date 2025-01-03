﻿SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sok Kim Thanh>
-- Create date: <22/10/2024>
-- Description:	<Tồn kho và chi phí>
-- =============================================
go
drop proc if exists sp_TonKhoChiTiet
drop proc if exists sp_TonKhoTongQuan
go
CREATE PROCEDURE sp_TonKhoChiTiet
    @StartDate DATETIME,                     -- Ngày bắt đầu
    @EndDate DATETIME,                       -- Ngày kết thúc
    @SalesPerformanceThreshold INT = 50,     -- Ngưỡng hiệu suất bán hàng (mặc định 50%)
    @MinStockThreshold INT = 50,             -- Lượng tồn kho dưới ngưỡng tối thiểu (mặc định 50%)
    @DesiredProfitMargin FLOAT = 0.2,        -- Lợi nhuận mong muốn (20%)
    @InventoryStatus INT = NULL              -- Trạng thái kho để lọc (0: Cần nhập hàng, 1: Đủ hàng)
AS
BEGIN
    SET NOCOUNT ON;

    -- CTE cho số liệu nhập hàng
    WITH Received AS (
        SELECT 
            EX.EX_EXTYPE_AutoID AS ProductID,
            SUM(EX.EX_QUANTITY) AS TotalReceived, -- Tổng số lượng nhập
            SUM(EX.EX_PRICE) AS TotalImportCost,  -- Tổng chi phí nhập
            CASE 
                WHEN SUM(EX.EX_QUANTITY) > 0 THEN SUM(EX.EX_PRICE) / SUM(EX.EX_QUANTITY) 
                ELSE 0 
            END AS UnitCost -- Giá gốc đơn vị
        FROM tbl_SYS_Expense EX
        WHERE EX.CREATED BETWEEN @StartDate AND @EndDate
        GROUP BY EX.EX_EXTYPE_AutoID
    ),
    -- CTE cho số liệu bán vé (Nhóm theo phim)
    TicketRevenue AS (
        SELECT 
            MV.MV_AutoID AS MovieID,              -- Mã phim
            MV.MV_NAME AS MovieName,              -- Tên phim
            MV.MV_PRICE AS TicketPrice,           -- Giá vé
            COUNT(TK.TK_AutoID) AS TotalTickets,  -- Tổng số lượng vé
            COUNT(TK.TK_AutoID) * MV.MV_PRICE AS TotalTicketRevenue -- Tổng doanh thu vé
        FROM tbl_DM_Ticket TK
        INNER JOIN tbl_DM_Movie MV ON TK.TK_MOVIESCHEDULE_AutoID = MV.MV_AutoID
        WHERE TK.CREATED BETWEEN @StartDate AND @EndDate
        GROUP BY MV.MV_AutoID, MV.MV_NAME, MV.MV_PRICE
    ),
    -- CTE cho số liệu bán hàng (sản phẩm)
    Sold AS (
        SELECT 
            BD.BD_PRODUCT_AutoID AS ProductID,
            SUM(BD.BD_QUANTITY) AS TotalSold,                       -- Tổng số lượng bán sản phẩm
            SUM(BD.BD_QUANTITY * P.PD_PRICE) AS ProductRevenue,     -- Doanh thu từ sản phẩm
            P.PD_PRICE AS UnitPrice                                -- Giá bán đơn vị sản phẩm
        FROM tbl_DM_BillDetail BD
        INNER JOIN tbl_DM_Bill B ON BD.BD_BILL_AutoID = B.BL_AutoID
        INNER JOIN tbl_DM_Product P ON BD.BD_PRODUCT_AutoID = P.PD_AutoID
        WHERE B.CREATED BETWEEN @StartDate AND @EndDate
        GROUP BY BD.BD_PRODUCT_AutoID, P.PD_PRICE
    )
    -- Truy vấn chính
    SELECT 
        PD.PD_AutoID AS ProductID,
        PD.PD_NAME AS ProductName,
        ISNULL(R.TotalReceived, 0) AS ReceivedQuantity,             -- Số lượng nhập
        ISNULL(R.TotalImportCost, 0) AS TotalImportCost,            -- Tổng chi phí nhập
        ISNULL(R.UnitCost, 0) AS UnitCost,                          -- Giá gốc đơn vị
        ISNULL(S.TotalSold, 0) AS SoldQuantity,                     -- Số lượng bán sản phẩm
        ISNULL(S.UnitPrice, 0) AS UnitPrice,                        -- Giá bán đơn vị sản phẩm
        ISNULL(S.ProductRevenue, 0) + ISNULL(TR.TotalTicketRevenue, 0) AS TotalRevenue, -- Tổng doanh thu
        
         CASE 
            WHEN (ISNULL(R.TotalReceived, 0) - ISNULL(S.TotalSold, 0)) < @MinStockThreshold THEN N'Cần nhập hàng'
            ELSE N'Đủ hàng'
        END AS RestockStatus -- Cảnh báo thiếu hàng
         
    FROM tbl_DM_Product PD
    LEFT JOIN Received R ON PD.PD_AutoID = R.ProductID
    LEFT JOIN Sold S ON PD.PD_AutoID = S.ProductID
    LEFT JOIN TicketRevenue TR ON TR.MovieID = PD.PD_AutoID
    WHERE PD.DELETED = 0
        AND (@InventoryStatus IS NULL OR
             (CASE 
                 WHEN (ISNULL(R.TotalReceived, 0) - ISNULL(S.TotalSold, 0)) < @MinStockThreshold THEN 0
                 ELSE 1
             END = @InventoryStatus))
    ORDER BY PD.PD_NAME ASC;
END;





go
CREATE PROCEDURE sp_TonKhoTongQuan
    @StartDate DATETIME,                  -- Ngày bắt đầu
    @EndDate DATETIME,                    -- Ngày kết thúc
    @SalesPerformanceThreshold INT = 50,  -- Ngưỡng phần trăm hiệu suất bán hàng (mặc định 50%)
    @MinStockThreshold INT = 50           -- Lượng tồn kho tối thiểu (mặc định 50 sản phẩm)
AS
BEGIN
    SET NOCOUNT ON;

    -- Truy vấn chính
    SELECT 
        PD.PD_AutoID AS ProductID,                                 -- Mã sản phẩm
        PD.PD_NAME AS ProductName,                                 -- Tên sản phẩm

        -- Tổng số lượng nhập
        ISNULL(SUM(EX.EX_QUANTITY), 0) AS TotalReceived,

        -- Tổng số lượng bán
        ISNULL(SUM(BD.BD_QUANTITY), 0) AS TotalSold,

        -- Số lượng còn tồn kho
        ISNULL(SUM(EX.EX_QUANTITY), 0) - ISNULL(SUM(BD.BD_QUANTITY), 0) AS RemainingStock,

        -- Tính hiệu suất bán hàng (%)
        CASE 
            WHEN ISNULL(SUM(EX.EX_QUANTITY), 0) = 0 THEN 0  -- Tránh chia cho 0
            ELSE (ISNULL(SUM(BD.BD_QUANTITY), 0) * 100.0 / ISNULL(SUM(EX.EX_QUANTITY), 0))
        END AS SalesPerformancePercentage,

        -- Phân loại hiệu suất bán hàng
        CASE
            WHEN ISNULL(SUM(EX.EX_QUANTITY), 0) = 0 THEN N'Cần nhập hàng'
            WHEN (ISNULL(SUM(BD.BD_QUANTITY), 0) * 100.0 / ISNULL(SUM(EX.EX_QUANTITY), 0)) < @SalesPerformanceThreshold THEN N'Bán chậm'
            WHEN (ISNULL(SUM(BD.BD_QUANTITY), 0) * 100.0 / ISNULL(SUM(EX.EX_QUANTITY), 0)) BETWEEN @SalesPerformanceThreshold AND (@SalesPerformanceThreshold + 20) THEN N'Hiệu suất ổn định'
            ELSE N'Mặt hàng hot'
        END AS SalesPerformanceCategory,

        -- Trạng thái tồn kho
        CASE
            WHEN (ISNULL(SUM(EX.EX_QUANTITY), 0) - ISNULL(SUM(BD.BD_QUANTITY), 0)) < @MinStockThreshold THEN N'Cần nhập hàng'
            ELSE N'Tồn kho đủ'
        END AS StockStatus,

        -- Tổng chi phí nhập hàng
        ISNULL(SUM(EX.EX_PRICE), 0) AS TotalImportCost,

        -- Tổng doanh thu từ sản phẩm
        ISNULL(SUM(BD.BD_QUANTITY * P.PD_PRICE), 0) AS TotalRevenue,

        -- Lợi nhuận từ sản phẩm
        CASE 
            WHEN ISNULL(SUM(EX.EX_PRICE), 0) > 0 THEN 
                ISNULL(SUM(BD.BD_QUANTITY * P.PD_PRICE), 0) - ISNULL(SUM(EX.EX_PRICE), 0)
            ELSE ISNULL(SUM(BD.BD_QUANTITY * P.PD_PRICE), 0)
        END AS Profit

    FROM tbl_DM_Product PD
    LEFT JOIN tbl_SYS_Expense EX ON EX.EX_EXTYPE_AutoID = PD.PD_AutoID AND EX.CREATED BETWEEN @StartDate AND @EndDate
    LEFT JOIN tbl_DM_BillDetail BD ON BD.BD_PRODUCT_AutoID = PD.PD_AutoID AND BD.CREATED BETWEEN @StartDate AND @EndDate
    LEFT JOIN tbl_DM_Product P ON BD.BD_PRODUCT_AutoID = P.PD_AutoID
    WHERE PD.DELETED = 0
    GROUP BY PD.PD_AutoID, PD.PD_NAME
    ORDER BY PD.PD_NAME ASC;
END;
GO

