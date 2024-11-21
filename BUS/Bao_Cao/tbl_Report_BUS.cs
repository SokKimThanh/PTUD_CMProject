﻿using DAL;
using DTO.tbl_DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Bao_Cao
{
    public class tbl_Report_BUS
    {
        private tbl_Report_Sales_DAL sales_DAL = new tbl_Report_Sales_DAL();
        private tbl_Report_Expense_DAL expense_DAL = new tbl_Report_Expense_DAL();
        private tbl_Report_Inventory_DAL inventory_DAL = new tbl_Report_Inventory_DAL();

        // Hàm báo cáo doanh thu
        public List<tbl_Report_Sales_DTO> GetAllSalesReport(DateTime ngayBatDau, DateTime ngayKetThuc)
        {
            return sales_DAL.GetSalesReport(ngayBatDau, ngayKetThuc);
        }

        /// <summary>
        /// Hàm tạo báo cáo doanh thu chi tiết
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public object GetDetailSaleReport(DateTime startDate, DateTime endDate)
        {
            return sales_DAL.GetDetailSaleReport(startDate, endDate);
        }

        /// <summary>
        /// Hàm báo cáo thu chi
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<tbl_Report_Expense_DTO> GetExpenseReport(DateTime startDate, DateTime endDate)
        {
            return expense_DAL.GetExpenseReport(startDate, endDate);
        }
        /// <summary>
        /// hàm báo cáo tồn kho
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public List<tbl_Report_Inventory_DTO> GetInventoryReport(DateTime startDate, DateTime endDate)
        {
            return inventory_DAL.GetInventoryReport(startDate, endDate);
        }
        /// <summary>
        /// Báo cáo tồn kho chi tiết
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="salesPerformanceThreshold"></param>
        /// <param name="minStockThreshold"></param>
        /// <param name="desiredProfitMargin"></param>
        /// <returns></returns>
        public List<tbl_Report_Inventory_DTO> GetInventoryReportByStatusAndDate(
              DateTime startDate,
              DateTime endDate, int salesPerformanceThreshold = 50,         // hiệu suất bán hàng
              int minStockThreshold = 50,    // ngưỡng tồn kho
              double desiredProfitMargin = 0.2 // lợi nhuận mong muốn
            )
        {
            return inventory_DAL.GetInventoryReportByStatusAndDate(startDate, endDate, salesPerformanceThreshold, minStockThreshold, desiredProfitMargin);
        }
    }
}
