﻿using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL
{
    public class tbl_DM_Theater_DAL : BasicMethods<tbl_DM_Theater_DTO>
    {
        private static string person = "Admin";
        /// <summary>
        /// Lấy danh sách các phòng chiếu
        /// </summary>
        /// <returns></returns>
        public override List<tbl_DM_Theater_DTO> GetList()
        {
            try
            {
                using(CM_Cinema_DBDataContext db = new CM_Cinema_DBDataContext())
                {
                    // Khởi tạo danh sách các phòng chiếu
                    List<tbl_DM_Theater_DTO> list = new List<tbl_DM_Theater_DTO>();
                    // Chuyển kiểu dữ liệu từ context sang DTO
                    foreach (tbl_DM_Theater item in db.tbl_DM_Theaters.ToList())
                    {
                        // Nếu không bị xóa khỏi context view thì thêm vào danh sách DTO
                        if (item.DELETED != 1)
                        {
                            // Thêm context vào danh sách DTO
                            int delete = 0;
                            if (item.DELETED != null)
                                delete = (int)item.DELETED;
                            list.Add(new tbl_DM_Theater_DTO(item.TT_AutoID, item.TT_NAME, item.TT_STATUS, delete));
                        }
                    }
                    return list;
                }
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public override void AddData(tbl_DM_Theater_DTO obj)
        {
            try
            {
                using (CM_Cinema_DBDataContext db = new CM_Cinema_DBDataContext())
                {
                    // Chuyển kiểu dữ liệu DTO sang context để thêm mới vào danh sách
                    tbl_DM_Theater theater = new tbl_DM_Theater()
                    {
                        TT_NAME = obj.Name,
                        TT_STATUS = obj.Status,

                    };
                    theater.DELETED = 0;
                    theater.CREATED = DateTime.Now;
                    theater.CREATED_BY = person;
                    theater.CREATED_BY_FUNCTION = "Add full";
                    theater.UPDATED = DateTime.Now;
                    theater.UPDATED_BY = person;
                    theater.UPDATED_BY_FUNCTION = "Add full";

                    db.tbl_DM_Theaters.InsertOnSubmit(theater);
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Ẩn phòng chiếu khỏi view
        /// </summary>
        /// <param name="id"></param>
        public override void RemoveData(int id)
        {
            try
            {
                using (CM_Cinema_DBDataContext db = new CM_Cinema_DBDataContext())
                {
                    // Chuyển kiểu dữ liệu DTO sang context để thêm mới vào danh sách
                    tbl_DM_Theater theater = db.tbl_DM_Theaters.SingleOrDefault(item => item.TT_AutoID == id);
                    if (theater != null)
                    {
                        // Sửa thuộc tính Xóa
                        theater.DELETED = 1;
                        theater.UPDATED = DateTime.Now;
                        theater.UPDATED_BY = person;
                        theater.UPDATED_BY_FUNCTION = "Remove";

                        // Cập nhật thông tin mới lên danh sách
                        db.SubmitChanges();
                    }
                    else
                    {
                        throw new Exception("Không tìm thấy phòng chiếu");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sửa thông tin phòng chiếu
        /// </summary>
        /// <param name="obj"></param>
        public override void UpdateData(tbl_DM_Theater_DTO obj)
        {
            try
            {
                using (CM_Cinema_DBDataContext db = new CM_Cinema_DBDataContext())
                {
                    // Chuyển kiểu dữ liệu DTO sang context để thêm mới vào danh sách
                    tbl_DM_Theater theater = db.tbl_DM_Theaters.SingleOrDefault(item => item.TT_AutoID == obj.AutoID);
                    if(theater != null)
                    {
                        // Sửa thông tin phòng chiếu
                        theater.TT_NAME = obj.Name;
                        theater.TT_STATUS = obj.Status;
                        theater.DELETED = obj.Deleted;
                        theater.UPDATED = DateTime.Now;
                        theater.UPDATED_BY = person;
                        theater.UPDATED_BY_FUNCTION = "Update";

                        // Cập nhật thông tin mới lên danh sách
                        db.SubmitChanges();
                    }
                    else
                    {
                        throw new Exception("Không tìm thấy phòng chiếu");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
