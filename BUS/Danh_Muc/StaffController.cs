﻿using DAL;
using DTO.Common;
using DTO.Custom;
using DTO.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Danh_Muc
{
    public class StaffController
    {
        public void LoginProcess(string strMaDangNhap, string strMatKhau)
        {
            using (CM_Cinema_DBDataContext objDB = new CM_Cinema_DBDataContext(CConfig.CM_Cinema_DB_ConnectionString))
            {
                string strError = "";
                string strStep = "1";
                try
                {
                    strMaDangNhap = strMaDangNhap.Trim();
                    strMatKhau = strMatKhau.Trim();

                    //Kiểm tra các trường thông tin nhập vào
                    if (strMaDangNhap == "")
                        strError += "Mã đăng nhập không được để trống.\n";

                    if (strMatKhau == "")
                        strError += "Mật khẩu không được để trống.\n";

                    if (strError != "")
                        throw new Exception(strError);

                    strStep = "2";
                    //Kiểm tra xem có user nào tồn tại với mã đăng nhập
                    tbl_DM_Staff objUser = objDB.tbl_DM_Staffs.FirstOrDefault(it => it.ST_USERNAME.Trim() == strMaDangNhap && it.DELETED == 0);
                    if (objUser == null)
                        throw new Exception("Mã đăng nhập không tồn tại.");

                    strStep = "3";
                    //Kiểm tra mật khẩu
                    if (objUser.ST_PASSWORD.Trim() != CUtility.MD5_Encrypt(strMatKhau))
                        throw new Exception("Mật khẩu không chính xác.");

                    strStep = "4";
                    //Kiểm tra xem nó đã được phân quyền chưa
                    if (objUser.ST_LEVEL == (int)ELevel.None)
                        throw new Exception("Tài khoản này chưa được phân quyền.");

                    //Nếu đã pass hết các trường hợp trên thì gán biến common
                    CCommon.MaDangNhap = objUser.ST_USERNAME;
                }
                catch (Exception ex)
                {
                    throw new Exception("Step: " + strStep + " , message: " + ex.Message);
                }
            }
        }

        public int LoadLevelByMaDangNhap(string strMaDangNhap)
        {
            int iRes = (int)ELevel.None;
            using (CM_Cinema_DBDataContext objDB = new CM_Cinema_DBDataContext(CConfig.CM_Cinema_DB_ConnectionString))
            {
                //Kiểm tra xem có user nào tồn tại với mã đăng nhập
                tbl_DM_Staff objUser = objDB.tbl_DM_Staffs.FirstOrDefault(it => it.ST_USERNAME == strMaDangNhap && it.DELETED == 0);
                if (objUser == null)
                    throw new Exception("Mã đăng nhập không tồn tại.");

                iRes = objUser.ST_LEVEL;
            }
            return iRes;
        }

        public string CheckLoginFileProcess(string strMaDangNhap, string strMatKhau)
        {
            using (CM_Cinema_DBDataContext objDB = new CM_Cinema_DBDataContext(CConfig.CM_Cinema_DB_ConnectionString))
            {
                string strError = "";
                string strStep = "1";
                try
                {
                    strMaDangNhap = strMaDangNhap.Trim();
                    strMatKhau = strMatKhau.Trim();

                    //Kiểm tra các trường thông tin nhập vào
                    if (strMaDangNhap == "")
                        strError += "Mã đăng nhập không được để trống.\n";

                    if (strMatKhau == "")
                        strError += "Mật khẩu không được để trống.\n";

                    if (strError != "")
                        throw new Exception(strError);

                    strStep = "2";
                    //Kiểm tra xem có user nào tồn tại với mã đăng nhập
                    tbl_DM_Staff objUser = null;
                    foreach (tbl_DM_Staff objTemp in objDB.tbl_DM_Staffs)
                    {
                        if (CUtility.MD5_Encrypt(objTemp.ST_USERNAME.Trim()) == strMaDangNhap && objTemp.DELETED == 0)
                        {
                            objUser = objTemp;
                            break;
                        }
                    }

                    if (objUser == null)
                        throw new Exception("Mã đăng nhập không tồn tại.");

                    strStep = "3";
                    //Kiểm tra mật khẩu
                    if (objUser.ST_PASSWORD.Trim() != strMatKhau)
                        throw new Exception("Mật khẩu không chính xác.");

                    strStep = "4";
                    //Kiểm tra xem nó đã được phân quyền chưa
                    if (objUser.ST_LEVEL == (int)ELevel.None)
                        throw new Exception("Tài khoản này chưa được phân quyền.");

                    return objUser.ST_USERNAME.Trim();
                }
                catch (Exception ex)
                {
                    throw new Exception("Step: " + strStep + " , message: " + ex.Message);
                }
            }
        }
    }
}
