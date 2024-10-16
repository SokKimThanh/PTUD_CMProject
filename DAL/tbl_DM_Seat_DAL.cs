﻿using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class tbl_DM_Seat_DAL : BasicMethods<tbl_DM_Seat_DTO>
    {

        /// <summary>
        /// Lấy danh sách số ghế của các rạp phim
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override List<tbl_DM_Seat_DTO> GetList()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Thêm 1 ghế vào danh sách ghế
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool AddData(tbl_DM_Seat_DTO obj)
        {
            try
            {
                bool flg = false;

                tbl_DM_Seat seat = new tbl_DM_Seat()
                {
                    SE_FILE = obj.File,
                    SE_RANK = obj.Rank,
                    SE_THEATER_AutoID = obj.Theater_AutoID,
                };
                DBDataContext.tbl_DM_Seats.InsertOnSubmit(seat);
                DBDataContext.SubmitChanges();

                flg = true;

                return flg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Sửa thông tin ghế
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool UpdateData(tbl_DM_Seat_DTO obj)
        {
            try
            {
                bool flg = false;

                // Tìm ghế có mã ghế như trên trong danh sách
                tbl_DM_Seat seat = DBDataContext.tbl_DM_Seats.SingleOrDefault(item => item.SE_AutoID == obj.AutoID);

                //Nếu tìm thấy ghế
                if (seat != null)
                {
                    seat.DELETED = obj.Deleted;
                    DBDataContext.SubmitChanges();
                    flg = true;
                }

                return flg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Xóa 1 ghế khỏi view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool RemoveData(int id)
        {
            try
            {
                bool flg = false;

                // Tìm ghế có mã ghế như trên trong danh sách
                tbl_DM_Seat seat = DBDataContext.tbl_DM_Seats.SingleOrDefault(item => item.SE_AutoID == id);
                //Nếu tìm thấy ghế
                if (seat != null)
                {
                    seat.DELETED = 1;
                    DBDataContext.tbl_DM_Seats.InsertOnSubmit(seat);
                    DBDataContext.SubmitChanges();
                    flg = true;
                }

                return flg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Tìm ghế
        /// </summary>
        /// <param name="file"></param>
        /// <param name="rank"></param>
        /// <param name="theater_AutoID"></param>
        /// <returns></returns>
        public tbl_DM_Seat_DTO FindSeat(string file, int rank, int theater_AutoID)
        {
            try
            {

                tbl_DM_Seat seat_Found = DBDataContext.tbl_DM_Seats.SingleOrDefault(item => item.SE_FILE == file && item.SE_RANK == rank && item.SE_THEATER_AutoID == theater_AutoID);
                tbl_DM_Seat_DTO result = null;
                if (seat_Found != null)
                {
                    result = new tbl_DM_Seat_DTO(seat_Found.SE_AutoID, seat_Found.SE_FILE, seat_Found.SE_RANK, seat_Found.SE_THEATER_AutoID, (int)seat_Found.DELETED);
                }
                return result;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
