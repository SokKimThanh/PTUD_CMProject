﻿using BUS.Danh_Muc;
using BUS.Sys;
using DevExpress.XtraEditors;
using DevExpress.XtraExport.Helpers;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DTO.Custom;
using DTO.tbl_DTO;
using DTO.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.UI.Modules
{
    public partial class ucNhanVien : ucBase
    {
        private List<tbl_DM_Staff_DTO> arrData = new List<tbl_DM_Staff_DTO>();
        private tbl_DM_Staff_DTO objEdit = null;

        private List<string> arrLevel = new List<string>();

        public ucNhanVien()
        {
            InitializeComponent();
        }

        protected override void Load_FirstBase()
        {
            if (strFunctionCode != "")
                lblTitle.Text = strFunctionCode.ToUpper().Trim();

            btnThem.ItemClick += Add_DataBase;
            btnXoa.ItemClick += Remove_DataBase;
            btnCapNhat.ItemClick += Update_DataBase;
            btnLamMoi.ItemClick += Refresh_Load_DataBase;

            this.layoutTitle.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
            this.layoutTitle.MaxSize = new System.Drawing.Size(0, 42);
            this.layoutTitle.MinSize = new System.Drawing.Size(36, 36);
            // Ngăn không cho phép sửa dữ liệu trực tiếp trên GridView
            grdData.OptionsBehavior.Editable = false;
            
            grdData.RowClick += RowClick_Grid;

            //cbbLevel.Properties.Items.Add(LanguageController.GetLanguageDataLabel("Admin"));
            cbbLevel.Properties.Items.Add(LanguageController.GetLanguageDataLabel("None"));
            cbbLevel.Properties.Items.Add(LanguageController.GetLanguageDataLabel("Manager"));
            cbbLevel.Properties.Items.Add(LanguageController.GetLanguageDataLabel("Staff"));

            cbbLevel.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
        }

        protected override void Load_Data()
        {
            tbl_DM_Staff_BUS objBUS = new tbl_DM_Staff_BUS();
            arrData = objBUS.ListData();

            dgv.DataSource = arrData;

            objEdit = null;

            txtNameStaff.Text = "";
            txtUserName.Text = "";
            txtPassword.Text = "";

            txtPhone.Text = "";
            txtCIC.Text = "";
            txtNOTE.Text = "";

            // Gán giá trị và focus vào mục đã chọn
            cbbLevel.SelectedItem = LanguageController.GetLanguageDataLabel("None");

            // Focus vào ComboBoxEdit
            cbbLevel.Focus();

            grdData.Columns["ST_AutoID"].Visible = false;
            grdData.Columns["ST_PASSWORD"].Visible = false;
            grdData.Columns["ST_LEVEL"].Visible = false;

            grdData.Columns["ST_USERNAME"].Caption = LanguageController.GetLanguageDataLabel("Mã đăng nhập");
            grdData.Columns["ST_PASSWORD"].Caption = LanguageController.GetLanguageDataLabel("Mật khẩu");

            grdData.Columns["ST_NAME"].Caption = LanguageController.GetLanguageDataLabel("Tên");
            grdData.Columns["ST_PHONE"].Caption = LanguageController.GetLanguageDataLabel("SĐT");
            grdData.Columns["ST_CIC"].Caption = LanguageController.GetLanguageDataLabel("CCCD");
            grdData.Columns["ST_NOTE"].Caption = LanguageController.GetLanguageDataLabel("Ghi chú");
            grdData.Columns["ST_LEVELText"].Caption = LanguageController.GetLanguageDataLabel("Level");

            FormatGridView(grdData);
            
        }

        protected override void Add_Data()
        {
            tbl_DM_Staff_BUS objBUS = new tbl_DM_Staff_BUS();
            tbl_DM_Staff_DTO objNew = new tbl_DM_Staff_DTO();

            objNew.ST_USERNAME = txtUserName.Text.Trim();
            objNew.ST_PASSWORD = CUtility.MD5_Encrypt(txtPassword.Text.Trim());
            objNew.ST_NAME = txtNameStaff.Text.Trim();
            objNew.ST_PHONE = txtPhone.Text.Trim();
            objNew.ST_CIC = txtCIC.Text.Trim();
            objNew.ST_NOTE = txtNOTE.Text.Trim();

            if (cbbLevel.SelectedItem != null)
            {
                string strCBBValue = cbbLevel.SelectedItem.ToString();

                if (strCBBValue == LanguageController.GetLanguageDataLabel("None"))
                    objNew.ST_LEVEL = (int)ELevel.None;
                else if (strCBBValue == LanguageController.GetLanguageDataLabel("Admin"))
                    objNew.ST_LEVEL = (int)ELevel.Admin;
                else if (strCBBValue == LanguageController.GetLanguageDataLabel("Manager"))
                    objNew.ST_LEVEL = (int)ELevel.Manager;
                else if (strCBBValue == LanguageController.GetLanguageDataLabel("Staff"))
                    objNew.ST_LEVEL = (int)ELevel.Staff;
            }

            objNew.DELETED = 0;
            objNew.CREATED = DateTime.Now;
            objNew.CREATED_BY = strActive_User_Name;
            objNew.CREATED_BY_FUNCTION = strFunctionCode;
            objNew.UPDATED = DateTime.Now;
            objNew.UPDATED_BY = strActive_User_Name;
            objNew.UPDATED_BY_FUNCTION = strFunctionCode;
            objBUS.AddData(objNew);
        }

        protected override void Update_Data()
        {
            tbl_DM_Staff_BUS objBUS = new tbl_DM_Staff_BUS();
            if (objEdit != null)
            {
                objEdit.ST_AutoID = iAuto_ID;
                objEdit.ST_USERNAME = txtUserName.Text.Trim();
                objEdit.ST_PASSWORD = CUtility.MD5_Encrypt(txtPassword.Text.Trim());
                objEdit.ST_NAME = txtNameStaff.Text.Trim();
                objEdit.ST_PHONE = txtPhone.Text.Trim();
                objEdit.ST_CIC = txtCIC.Text.Trim();
                objEdit.ST_NOTE = txtNOTE.Text.Trim();

                if (cbbLevel.SelectedItem != null)
                {
                    string strCBBValue = cbbLevel.SelectedItem.ToString();

                    if (strCBBValue == LanguageController.GetLanguageDataLabel("None"))
                        objEdit.ST_LEVEL = (int)ELevel.None;
                    else if (strCBBValue == LanguageController.GetLanguageDataLabel("Admin"))
                        objEdit.ST_LEVEL = (int)ELevel.Admin;
                    else if (strCBBValue == LanguageController.GetLanguageDataLabel("Manager"))
                        objEdit.ST_LEVEL = (int)ELevel.Manager;
                    else if (strCBBValue == LanguageController.GetLanguageDataLabel("Staff"))
                        objEdit.ST_LEVEL = (int)ELevel.Staff;
                }
                objEdit.UPDATED = DateTime.Now;
                objEdit.UPDATED_BY = strActive_User_Name;
                objEdit.UPDATED_BY_FUNCTION = strFunctionCode;

                objBUS.UpdateData(objEdit);
            }
        }

        protected override void RemoveData(long iAuto_ID)
        {
            tbl_DM_Staff_BUS objBUS = new tbl_DM_Staff_BUS();
            objBUS.RemoveData(iAuto_ID, strActive_User_Name, strFunctionCode);
        }

        protected override void ObjectProcessing(object obj)
        {
            objEdit = obj as tbl_DM_Staff_DTO;
            iAuto_ID = objEdit.ST_AutoID;
            txtNameStaff.Text = objEdit.ST_NAME.Trim();
            txtUserName.Text = objEdit.ST_USERNAME.Trim();
            txtPhone.Text = objEdit.ST_PHONE.Trim();
            txtCIC.Text = objEdit.ST_CIC.Trim();
            txtNOTE.Text = objEdit.ST_NOTE.Trim();

            // Gán giá trị và focus vào mục đã chọn
            cbbLevel.SelectedItem = LanguageController.GetLanguageDataLabel(objEdit.ST_LEVELText);

            // Focus vào ComboBoxEdit
            cbbLevel.Focus();

        }
    }
}
