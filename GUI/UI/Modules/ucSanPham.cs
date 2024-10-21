﻿using DevExpress.Utils;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.UI.Modules
{
    public partial class ucSanPham : ucBase
    {
        public ucSanPham()
        {
            InitializeComponent(); 
        }

        protected override void Load_Data()
        {
            if (strFunctionCode != "")
                lblTitle.Text = strFunctionCode.ToUpper().Trim();
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {


        }
        /// <summary>
        /// Nút thêm hình nếu người dùng thích bấm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Hiển thị hình ảnh trên PictureEdit
                pictureBox.Image = Image.FromFile(openFileDialog.FileName);

                // Lấy đường dẫn đầy đủ của file đã chọn
                string selectedImagePath = openFileDialog.FileName;

                // Hiển thị đường dẫn hình ảnh trong TextBox hoặc Label
                txtUrlHinhAnh.Text = selectedImagePath;
            }
        }
        /// <summary>
        /// Nhấp vào hình hiển thị dialog box chọn ảnh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Hiển thị hình ảnh trên PictureEdit
                pictureBox.Image = Image.FromFile(openFileDialog.FileName);

                // Lấy đường dẫn đầy đủ của file đã chọn
                string selectedImagePath = openFileDialog.FileName;

                // Hiển thị đường dẫn hình ảnh trong TextBox hoặc Label
                txtUrlHinhAnh.Text = selectedImagePath;
            }
        }
    }
}
