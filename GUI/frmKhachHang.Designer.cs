namespace QLKS.GUI
{
    partial class frmKhachHang
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnThem = new System.Windows.Forms.Button();
            btnSua = new System.Windows.Forms.Button();
            btnXoa = new System.Windows.Forms.Button();
            btnLuu = new System.Windows.Forms.Button();
            btnHuy = new System.Windows.Forms.Button();
            txtMaKH = new System.Windows.Forms.TextBox();
            txtHoTen = new System.Windows.Forms.TextBox();
            txtCCCD = new System.Windows.Forms.TextBox();
            txtTongThu = new System.Windows.Forms.TextBox();
            txtSoLuotToi = new System.Windows.Forms.TextBox();
            cboGioiTinh = new System.Windows.Forms.ComboBox();
            lblMaKH = new System.Windows.Forms.Label();
            lblHoTen = new System.Windows.Forms.Label();
            lblGioiTinh = new System.Windows.Forms.Label();
            lblCCCD = new System.Windows.Forms.Label();
            lblTongThu = new System.Windows.Forms.Label();
            lblSoLuotToi = new System.Windows.Forms.Label();
            lblTimNhanh = new System.Windows.Forms.Label();
            btnTimNangCao = new System.Windows.Forms.Button();
            txtTimNhanh = new System.Windows.Forms.TextBox();
            lbltieude = new System.Windows.Forms.Label();
            lvKhachHang = new System.Windows.Forms.ListView();
            SuspendLayout();
            // 
            // btnThem
            // 
            btnThem.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnThem.Location = new System.Drawing.Point(12, 595);
            btnThem.Name = "btnThem";
            btnThem.Size = new System.Drawing.Size(94, 29);
            btnThem.TabIndex = 2;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            // 
            // btnSua
            // 
            btnSua.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnSua.Location = new System.Drawing.Point(125, 595);
            btnSua.Name = "btnSua";
            btnSua.Size = new System.Drawing.Size(94, 29);
            btnSua.TabIndex = 3;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            // 
            // btnXoa
            // 
            btnXoa.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnXoa.Location = new System.Drawing.Point(238, 595);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new System.Drawing.Size(94, 29);
            btnXoa.TabIndex = 4;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            // 
            // btnLuu
            // 
            btnLuu.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnLuu.Location = new System.Drawing.Point(351, 595);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new System.Drawing.Size(94, 29);
            btnLuu.TabIndex = 5;
            btnLuu.Text = "Lưu";
            btnLuu.UseVisualStyleBackColor = true;
            // 
            // btnHuy
            // 
            btnHuy.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnHuy.Location = new System.Drawing.Point(464, 595);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new System.Drawing.Size(94, 29);
            btnHuy.TabIndex = 6;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = true;
            // 
            // txtMaKH
            // 
            txtMaKH.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtMaKH.Location = new System.Drawing.Point(130, 452);
            txtMaKH.MaxLength = 12;
            txtMaKH.Name = "txtMaKH";
            txtMaKH.Size = new System.Drawing.Size(151, 27);
            txtMaKH.TabIndex = 8;
            // 
            // txtHoTen
            // 
            txtHoTen.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtHoTen.Location = new System.Drawing.Point(450, 452);
            txtHoTen.MaxLength = 200;
            txtHoTen.Name = "txtHoTen";
            txtHoTen.Size = new System.Drawing.Size(300, 27);
            txtHoTen.TabIndex = 9;
            // 
            // txtCCCD
            // 
            txtCCCD.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtCCCD.Location = new System.Drawing.Point(450, 497);
            txtCCCD.MaxLength = 12;
            txtCCCD.Name = "txtCCCD";
            txtCCCD.Size = new System.Drawing.Size(153, 27);
            txtCCCD.TabIndex = 10;
            // 
            // txtTongThu
            // 
            txtTongThu.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtTongThu.Location = new System.Drawing.Point(450, 542);
            txtTongThu.MaxLength = 1000;
            txtTongThu.Name = "txtTongThu";
            txtTongThu.ReadOnly = true;
            txtTongThu.Size = new System.Drawing.Size(151, 27);
            txtTongThu.TabIndex = 11;
            // 
            // txtSoLuotToi
            // 
            txtSoLuotToi.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtSoLuotToi.Location = new System.Drawing.Point(130, 545);
            txtSoLuotToi.MaxLength = 999999999;
            txtSoLuotToi.Name = "txtSoLuotToi";
            txtSoLuotToi.ReadOnly = true;
            txtSoLuotToi.Size = new System.Drawing.Size(151, 27);
            txtSoLuotToi.TabIndex = 12;
            // 
            // cboGioiTinh
            // 
            cboGioiTinh.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            cboGioiTinh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboGioiTinh.FormattingEnabled = true;
            cboGioiTinh.Location = new System.Drawing.Point(130, 497);
            cboGioiTinh.Name = "cboGioiTinh";
            cboGioiTinh.Size = new System.Drawing.Size(151, 28);
            cboGioiTinh.TabIndex = 13;
            // 
            // lblMaKH
            // 
            lblMaKH.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblMaKH.AutoSize = true;
            lblMaKH.Location = new System.Drawing.Point(12, 451);
            lblMaKH.Name = "lblMaKH";
            lblMaKH.Size = new System.Drawing.Size(114, 20);
            lblMaKH.TabIndex = 17;
            lblMaKH.Text = "Mã Khách Hàng";
            // 
            // lblHoTen
            // 
            lblHoTen.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblHoTen.AutoSize = true;
            lblHoTen.Location = new System.Drawing.Point(372, 455);
            lblHoTen.Name = "lblHoTen";
            lblHoTen.Size = new System.Drawing.Size(60, 20);
            lblHoTen.TabIndex = 19;
            lblHoTen.Text = "Họ Tên ";
            // 
            // lblGioiTinh
            // 
            lblGioiTinh.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblGioiTinh.AutoSize = true;
            lblGioiTinh.Location = new System.Drawing.Point(34, 496);
            lblGioiTinh.Name = "lblGioiTinh";
            lblGioiTinh.Size = new System.Drawing.Size(68, 20);
            lblGioiTinh.TabIndex = 21;
            lblGioiTinh.Text = "Giới Tính";
            // 
            // lblCCCD
            // 
            lblCCCD.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblCCCD.AutoSize = true;
            lblCCCD.Location = new System.Drawing.Point(306, 500);
            lblCCCD.Name = "lblCCCD";
            lblCCCD.Size = new System.Drawing.Size(136, 20);
            lblCCCD.TabIndex = 22;
            lblCCCD.Text = "Căn cước công dân";
            // 
            // lblTongThu
            // 
            lblTongThu.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblTongThu.AutoSize = true;
            lblTongThu.Location = new System.Drawing.Point(31, 545);
            lblTongThu.Name = "lblTongThu";
            lblTongThu.Size = new System.Drawing.Size(71, 20);
            lblTongThu.TabIndex = 23;
            lblTongThu.Text = "Tổng Thu";
            // 
            // lblSoLuotToi
            // 
            lblSoLuotToi.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblSoLuotToi.AutoSize = true;
            lblSoLuotToi.Location = new System.Drawing.Point(360, 545);
            lblSoLuotToi.Name = "lblSoLuotToi";
            lblSoLuotToi.Size = new System.Drawing.Size(82, 20);
            lblSoLuotToi.TabIndex = 24;
            lblSoLuotToi.Text = "Số lượt Tới";
            // 
            // lblTimNhanh
            // 
            lblTimNhanh.AutoSize = true;
            lblTimNhanh.Location = new System.Drawing.Point(520, 18);
            lblTimNhanh.Name = "lblTimNhanh";
            lblTimNhanh.Size = new System.Drawing.Size(72, 20);
            lblTimNhanh.TabIndex = 27;
            lblTimNhanh.Text = "Tìm Kiếm";
            // 
            // btnTimNangCao
            // 
            btnTimNangCao.Location = new System.Drawing.Point(968, 14);
            btnTimNangCao.Name = "btnTimNangCao";
            btnTimNangCao.Size = new System.Drawing.Size(157, 29);
            btnTimNangCao.TabIndex = 26;
            btnTimNangCao.Text = "Tìm kiếm Nâng Cao";
            btnTimNangCao.UseVisualStyleBackColor = true;
            // 
            // txtTimNhanh
            // 
            txtTimNhanh.Location = new System.Drawing.Point(600, 14);
            txtTimNhanh.Name = "txtTimNhanh";
            txtTimNhanh.Size = new System.Drawing.Size(362, 27);
            txtTimNhanh.TabIndex = 25;
            // 
            // lbltieude
            // 
            lbltieude.AutoSize = true;
            lbltieude.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lbltieude.Location = new System.Drawing.Point(12, 12);
            lbltieude.Name = "lbltieude";
            lbltieude.Size = new System.Drawing.Size(242, 31);
            lbltieude.TabIndex = 29;
            lbltieude.Text = "Quản Lý Khách Hàng ";
            // 
            // lvKhachHang
            // 
            lvKhachHang.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lvKhachHang.FullRowSelect = true;
            lvKhachHang.GridLines = true;
            lvKhachHang.Location = new System.Drawing.Point(12, 55);
            lvKhachHang.MultiSelect = false;
            lvKhachHang.Name = "lvKhachHang";
            lvKhachHang.Size = new System.Drawing.Size(1288, 379);
            lvKhachHang.TabIndex = 30;
            lvKhachHang.UseCompatibleStateImageBehavior = false;
            lvKhachHang.View = System.Windows.Forms.View.Details;
            // 
            // frmKhachHang
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1317, 662);
            Controls.Add(lvKhachHang);
            Controls.Add(lbltieude);
            Controls.Add(lblTimNhanh);
            Controls.Add(btnTimNangCao);
            Controls.Add(txtTimNhanh);
            Controls.Add(lblSoLuotToi);
            Controls.Add(lblTongThu);
            Controls.Add(lblCCCD);
            Controls.Add(lblGioiTinh);
            Controls.Add(lblHoTen);
            Controls.Add(lblMaKH);
            Controls.Add(cboGioiTinh);
            Controls.Add(txtSoLuotToi);
            Controls.Add(txtTongThu);
            Controls.Add(txtCCCD);
            Controls.Add(txtHoTen);
            Controls.Add(txtMaKH);
            Controls.Add(btnHuy);
            Controls.Add(btnLuu);
            Controls.Add(btnXoa);
            Controls.Add(btnSua);
            Controls.Add(btnThem);
            Name = "frmKhachHang";
            Text = "frmKhachHang";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.TextBox txtMaKH;
        private System.Windows.Forms.TextBox txtHoTen;
        private System.Windows.Forms.TextBox txtCCCD;
        private System.Windows.Forms.TextBox txtTongThu;
        private System.Windows.Forms.TextBox txtSoLuotToi;
        private System.Windows.Forms.ComboBox cboGioiTinh;
        private System.Windows.Forms.Label lblMaKH;
        private System.Windows.Forms.Label lblHoTen;
        private System.Windows.Forms.Label lblGioiTinh;
        private System.Windows.Forms.Label lblCCCD;
        private System.Windows.Forms.Label lblTongThu;
        private System.Windows.Forms.Label lblSoLuotToi;
        private System.Windows.Forms.Label lblTimNhanh;
        private System.Windows.Forms.Button btnTimNangCao;
        private System.Windows.Forms.TextBox txtTimNhanh;
        private System.Windows.Forms.Label lbltieude;
        private System.Windows.Forms.ListView lvKhachHang;
    }
}