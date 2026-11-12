namespace QLKS.GUI
{
    partial class frmPhong
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lbltieude = new System.Windows.Forms.Label();
            lblTimNhanh = new System.Windows.Forms.Label();
            txtTimNhanh = new System.Windows.Forms.TextBox();
            btnTimNangCao = new System.Windows.Forms.Button();
            lvPhong = new System.Windows.Forms.ListView();
            lblMaPhong = new System.Windows.Forms.Label();
            txtMaPhong = new System.Windows.Forms.TextBox();
            lblTenPhong = new System.Windows.Forms.Label();
            txtTenPhong = new System.Windows.Forms.TextBox();
            lblLoaiPhong = new System.Windows.Forms.Label();
            txtLoaiPhong = new System.Windows.Forms.TextBox();
            btnChonLoai = new System.Windows.Forms.Button();
            lblSoNguoi = new System.Windows.Forms.Label();
            cboSoNguoi = new System.Windows.Forms.ComboBox();
            lblTrangThai = new System.Windows.Forms.Label();
            txtTrangThai = new System.Windows.Forms.TextBox();
            btnSua = new System.Windows.Forms.Button();
            btnXoa = new System.Windows.Forms.Button();
            btnLuu = new System.Windows.Forms.Button();
            btnHuy = new System.Windows.Forms.Button();
            SuspendLayout();
            //
            // lbltieude
            //
            lbltieude.AutoSize = true;
            lbltieude.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lbltieude.Location = new System.Drawing.Point(12, 12);
            lbltieude.Name = "lbltieude";
            lbltieude.Size = new System.Drawing.Size(172, 31);
            lbltieude.TabIndex = 30;
            lbltieude.Text = "Quản Lý Phòng";
            //
            // lblTimNhanh
            //
            lblTimNhanh.AutoSize = true;
            lblTimNhanh.Location = new System.Drawing.Point(520, 18);
            lblTimNhanh.Name = "lblTimNhanh";
            lblTimNhanh.Size = new System.Drawing.Size(72, 20);
            lblTimNhanh.TabIndex = 37;
            lblTimNhanh.Text = "Tìm Kiếm";
            //
            // txtTimNhanh
            //
            txtTimNhanh.Location = new System.Drawing.Point(600, 14);
            txtTimNhanh.Name = "txtTimNhanh";
            txtTimNhanh.Size = new System.Drawing.Size(362, 27);
            txtTimNhanh.TabIndex = 0;
            //
            // btnTimNangCao
            //
            btnTimNangCao.Location = new System.Drawing.Point(968, 12);
            btnTimNangCao.Name = "btnTimNangCao";
            btnTimNangCao.Size = new System.Drawing.Size(157, 29);
            btnTimNangCao.TabIndex = 1;
            btnTimNangCao.Text = "Tìm kiếm Nâng Cao";
            btnTimNangCao.UseVisualStyleBackColor = true;
            //
            // lvPhong
            //
            lvPhong.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lvPhong.FullRowSelect = true;
            lvPhong.GridLines = true;
            lvPhong.HideSelection = false;
            lvPhong.Location = new System.Drawing.Point(12, 55);
            lvPhong.MultiSelect = false;
            lvPhong.Name = "lvPhong";
            lvPhong.Size = new System.Drawing.Size(1113, 319);
            lvPhong.TabIndex = 2;
            lvPhong.UseCompatibleStateImageBehavior = false;
            lvPhong.View = System.Windows.Forms.View.Details;
            //
            // lblMaPhong
            //
            lblMaPhong.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblMaPhong.AutoSize = true;
            lblMaPhong.Location = new System.Drawing.Point(12, 398);
            lblMaPhong.Name = "lblMaPhong";
            lblMaPhong.Size = new System.Drawing.Size(77, 20);
            lblMaPhong.TabIndex = 40;
            lblMaPhong.Text = "Mã Phòng";
            //
            // txtMaPhong
            //
            txtMaPhong.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtMaPhong.Location = new System.Drawing.Point(150, 395);
            txtMaPhong.MaxLength = 10;
            txtMaPhong.ReadOnly = true;
            txtMaPhong.Name = "txtMaPhong";
            txtMaPhong.Size = new System.Drawing.Size(250, 27);
            txtMaPhong.TabIndex = 3;
            //
            // lblTenPhong
            //
            lblTenPhong.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblTenPhong.AutoSize = true;
            lblTenPhong.Location = new System.Drawing.Point(480, 398);
            lblTenPhong.Name = "lblTenPhong";
            lblTenPhong.Size = new System.Drawing.Size(80, 20);
            lblTenPhong.TabIndex = 41;
            lblTenPhong.Text = "Tên Phòng";
            //
            // txtTenPhong
            //
            txtTenPhong.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtTenPhong.Location = new System.Drawing.Point(600, 395);
            txtTenPhong.MaxLength = 50;
            txtTenPhong.Name = "txtTenPhong";
            txtTenPhong.Size = new System.Drawing.Size(250, 27);
            txtTenPhong.TabIndex = 4;
            //
            // lblLoaiPhong
            //
            lblLoaiPhong.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblLoaiPhong.AutoSize = true;
            lblLoaiPhong.Location = new System.Drawing.Point(12, 438);
            lblLoaiPhong.Name = "lblLoaiPhong";
            lblLoaiPhong.Size = new System.Drawing.Size(81, 20);
            lblLoaiPhong.TabIndex = 42;
            lblLoaiPhong.Text = "Loại Phòng";
            //
            // txtLoaiPhong
            //
            txtLoaiPhong.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtLoaiPhong.Location = new System.Drawing.Point(150, 435);
            txtLoaiPhong.Name = "txtLoaiPhong";
            txtLoaiPhong.ReadOnly = true;
            txtLoaiPhong.Size = new System.Drawing.Size(250, 27);
            txtLoaiPhong.TabIndex = 43;
            //
            // btnChonLoai
            //
            btnChonLoai.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnChonLoai.Location = new System.Drawing.Point(406, 434);
            btnChonLoai.Name = "btnChonLoai";
            btnChonLoai.Size = new System.Drawing.Size(40, 29);
            btnChonLoai.TabIndex = 5;
            btnChonLoai.Text = "...";
            btnChonLoai.UseVisualStyleBackColor = true;
            //
            // lblSoNguoi
            //
            lblSoNguoi.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblSoNguoi.AutoSize = true;
            lblSoNguoi.Location = new System.Drawing.Point(480, 438);
            lblSoNguoi.Name = "lblSoNguoi";
            lblSoNguoi.Size = new System.Drawing.Size(113, 20);
            lblSoNguoi.TabIndex = 44;
            lblSoNguoi.Text = "Số Người Tối Đa";
            //
            // cboSoNguoi
            //
            cboSoNguoi.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            cboSoNguoi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cboSoNguoi.FormattingEnabled = true;
            cboSoNguoi.Location = new System.Drawing.Point(600, 435);
            cboSoNguoi.Name = "cboSoNguoi";
            cboSoNguoi.Size = new System.Drawing.Size(100, 28);
            cboSoNguoi.TabIndex = 6;
            //
            // lblTrangThai
            //
            lblTrangThai.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblTrangThai.AutoSize = true;
            lblTrangThai.Location = new System.Drawing.Point(12, 478);
            lblTrangThai.Name = "lblTrangThai";
            lblTrangThai.Size = new System.Drawing.Size(76, 20);
            lblTrangThai.TabIndex = 45;
            lblTrangThai.Text = "Trạng Thái";
            //
            // txtTrangThai
            //
            txtTrangThai.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtTrangThai.Location = new System.Drawing.Point(150, 475);
            txtTrangThai.Name = "txtTrangThai";
            txtTrangThai.ReadOnly = true;
            txtTrangThai.Size = new System.Drawing.Size(250, 27);
            txtTrangThai.TabIndex = 46;
            //
            // btnSua
            //
            btnSua.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnSua.Location = new System.Drawing.Point(12, 524);
            btnSua.Name = "btnSua";
            btnSua.Size = new System.Drawing.Size(94, 29);
            btnSua.TabIndex = 8;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            //
            // btnXoa
            //
            btnXoa.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnXoa.Location = new System.Drawing.Point(125, 524);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new System.Drawing.Size(94, 29);
            btnXoa.TabIndex = 9;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            //
            // btnLuu
            //
            btnLuu.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnLuu.Location = new System.Drawing.Point(238, 524);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new System.Drawing.Size(94, 29);
            btnLuu.TabIndex = 10;
            btnLuu.Text = "Lưu";
            btnLuu.UseVisualStyleBackColor = true;
            //
            // btnHuy
            //
            btnHuy.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnHuy.Location = new System.Drawing.Point(351, 524);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new System.Drawing.Size(94, 29);
            btnHuy.TabIndex = 11;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = true;
            //
            // frmPhong
            //
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1137, 568);
            Controls.Add(btnHuy);
            Controls.Add(btnLuu);
            Controls.Add(btnXoa);
            Controls.Add(btnSua);
            Controls.Add(txtTrangThai);
            Controls.Add(lblTrangThai);
            Controls.Add(cboSoNguoi);
            Controls.Add(lblSoNguoi);
            Controls.Add(btnChonLoai);
            Controls.Add(txtLoaiPhong);
            Controls.Add(lblLoaiPhong);
            Controls.Add(txtTenPhong);
            Controls.Add(lblTenPhong);
            Controls.Add(txtMaPhong);
            Controls.Add(lblMaPhong);
            Controls.Add(lvPhong);
            Controls.Add(btnTimNangCao);
            Controls.Add(txtTimNhanh);
            Controls.Add(lblTimNhanh);
            Controls.Add(lbltieude);
            Name = "frmPhong";
            Text = "frmPhong";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lbltieude;
        private System.Windows.Forms.Label lblTimNhanh;
        private System.Windows.Forms.TextBox txtTimNhanh;
        private System.Windows.Forms.Button btnTimNangCao;
        private System.Windows.Forms.ListView lvPhong;
        private System.Windows.Forms.Label lblMaPhong;
        private System.Windows.Forms.TextBox txtMaPhong;
        private System.Windows.Forms.Label lblTenPhong;
        private System.Windows.Forms.TextBox txtTenPhong;
        private System.Windows.Forms.Label lblLoaiPhong;
        private System.Windows.Forms.TextBox txtLoaiPhong;
        private System.Windows.Forms.Button btnChonLoai;
        private System.Windows.Forms.Label lblSoNguoi;
        private System.Windows.Forms.ComboBox cboSoNguoi;
        private System.Windows.Forms.Label lblTrangThai;
        private System.Windows.Forms.TextBox txtTrangThai;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
    }
}
