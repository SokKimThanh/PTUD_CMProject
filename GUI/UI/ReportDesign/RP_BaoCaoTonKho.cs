﻿using BUS.Danh_Muc;
using DevExpress.XtraRichEdit.Import.Html;
using DTO.Common;
using System;
using System.Runtime.InteropServices.ComTypes;

namespace GUI.UI.ReportDesign
{
    public partial class RP_BaoCaoTonKho : DevExpress.XtraReports.UI.XtraReport
    {
        private tbl_DM_Staff_BUS tbl_DM_Staff_BUS = new tbl_DM_Staff_BUS();

        public RP_BaoCaoTonKho()
        {
            InitializeComponent();
        }
        /// <summary> 
        /// @StartDate DATETIME,                     -- Ngày bắt đầu
        /// @EndDate DATETIME,                       -- Ngày kết thúc
        /// @SalesPerformanceThreshold INT = 50,     -- Ngưỡng hiệu suất bán hàng(mặc định 50%)
        /// @MinStockThreshold INT = 50,             -- Lượng tồn kho dưới ngưỡng tối thiểu(mặc định 50%)
        /// @DesiredProfitMargin FLOAT = 0.2,        -- Lợi nhuận mong muốn(20%)
        /// @InventoryStatus INT = NULL-- Trạng thái kho để lọc(0: Cần nhập hàng, 1: Đủ hàng) 
        /// </summary>
        /// <param name="_startDate"></param>
        /// <param name="_endDate"></param>
        public void Add(DateTime _startDate, DateTime _endDate, int inventoryStatus)
        {
            // Truyền tham số vào báo cáo
            this.Parameters["StartDate"].Value = _startDate;
            this.Parameters["EndDate"].Value = _endDate;
            this.Parameters["Performance"].Value = 50;
            this.Parameters["MinStock"].Value = 50;
            this.Parameters["Profit"].Value = 0.2;
            this.Parameters["NguoiLapBaoCao"].Value = tbl_DM_Staff_BUS.GetStaff_ByUserName(CCommon.MaDangNhap).ST_NAME;
            this.Parameters["InventoryStatus"].Value = inventoryStatus;

            // Tắt field nhập parameter khi preview
            this.Parameters["StartDate"].Visible = false;
            this.Parameters["EndDate"].Visible = false;
            this.Parameters["Performance"].Visible = false;
            this.Parameters["MinStock"].Visible = false;
            this.Parameters["Profit"].Visible = false;
            this.Parameters["NguoiLapBaoCao"].Visible = false;
            this.Parameters["InventoryStatus"].Visible = false;

        }
    }
}
