﻿using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.UI.Modules
{
    public partial class ucPhongChieu : DevExpress.XtraEditors.XtraUserControl
    {
        public ucPhongChieu()
        {
            InitializeComponent();
            lblTitle.Text = "Quản lý phòng chiếu".ToUpper();
        }
    }
}