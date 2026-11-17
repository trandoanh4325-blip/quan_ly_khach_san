namespace QLKS.GUI
{
    partial class frmLoaiPhong
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
            lvLoaiPhong = new System.Windows.Forms.ListView();
            lblMaLoai = new System.Windows.Forms.Label();
            txtMaLoai = new System.Windows.Forms.TextBox();
            lblTenLoai = new System.Windows.Forms.Label();
            txtTenLoai = new System.Windows.Forms.TextBox();
            lblDonGiaNgay = new System.Windows.Forms.Label();
            txtDonGiaNgay = new System.Windows.Forms.TextBox();
            lblDonGiaGio = new System.Windows.Forms.Label();
            txtDonGiaGio = new System.Windows.Forms.TextBox();
            lblSoLuongDangCo = new System.Windows.Forms.Label();
            txtSoLuongDangCo = new System.Windows.Forms.TextBox();
            lblTongSoPhong = new System.Windows.Forms.Label();
            txtTongSoPhong = new System.Windows.Forms.TextBox();
            btnThem = new System.Windows.Forms.Button();
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
            lbltieude.Size = new System.Drawing.Size(210, 31);
            lbltieude.TabIndex = 30;
            lbltieude.Text = "Quản Lý Loại Phòng";
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
            // lvLoaiPhong
            //
            lvLoaiPhong.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lvLoaiPhong.FullRowSelect = true;
            lvLoaiPhong.GridLines = true;
            lvLoaiPhong.HideSelection = false;
            lvLoaiPhong.Location = new System.Drawing.Point(12, 55);
            lvLoaiPhong.MultiSelect = false;
            lvLoaiPhong.Name = "lvLoaiPhong";
            lvLoaiPhong.Size = new System.Drawing.Size(1113, 319);
            lvLoaiPhong.TabIndex = 2;
            lvLoaiPhong.UseCompatibleStateImageBehavior = false;
            lvLoaiPhong.View = System.Windows.Forms.View.Details;
            //
            // lblMaLoai
            //
            lblMaLoai.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblMaLoai.AutoSize = true;
            lblMaLoai.Location = new System.Drawing.Point(12, 398);
            lblMaLoai.Name = "lblMaLoai";
            lblMaLoai.Size = new System.Drawing.Size(105, 20);
            lblMaLoai.TabIndex = 40;
            lblMaLoai.Text = "Mã Loại Phòng";
            //
            // txtMaLoai
            //
            txtMaLoai.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtMaLoai.Location = new System.Drawing.Point(150, 395);
            txtMaLoai.Name = "txtMaLoai";
            txtMaLoai.ReadOnly = true;
            txtMaLoai.Size = new System.Drawing.Size(170, 27);
            txtMaLoai.TabIndex = 41;
            //
            // lblTenLoai
            //
            lblTenLoai.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblTenLoai.AutoSize = true;
            lblTenLoai.Location = new System.Drawing.Point(360, 398);
            lblTenLoai.Name = "lblTenLoai";
            lblTenLoai.Size = new System.Drawing.Size(108, 20);
            lblTenLoai.TabIndex = 42;
            lblTenLoai.Text = "Tên Loại Phòng";
            //
            // txtTenLoai
            //
            txtTenLoai.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtTenLoai.Location = new System.Drawing.Point(480, 395);
            txtTenLoai.MaxLength = 50;
            txtTenLoai.Name = "txtTenLoai";
            txtTenLoai.Size = new System.Drawing.Size(250, 27);
            txtTenLoai.TabIndex = 3;
            //
            // lblDonGiaNgay
            //
            lblDonGiaNgay.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblDonGiaNgay.AutoSize = true;
            lblDonGiaNgay.Location = new System.Drawing.Point(12, 438);
            lblDonGiaNgay.Name = "lblDonGiaNgay";
            lblDonGiaNgay.Size = new System.Drawing.Size(97, 20);
            lblDonGiaNgay.TabIndex = 43;
            lblDonGiaNgay.Text = "Đơn Giá Ngày";
            //
            // txtDonGiaNgay
            //
            txtDonGiaNgay.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtDonGiaNgay.Location = new System.Drawing.Point(150, 435);
            txtDonGiaNgay.MaxLength = 15;
            txtDonGiaNgay.Name = "txtDonGiaNgay";
            txtDonGiaNgay.Size = new System.Drawing.Size(170, 27);
            txtDonGiaNgay.TabIndex = 4;
            //
            // lblDonGiaGio
            //
            lblDonGiaGio.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblDonGiaGio.AutoSize = true;
            lblDonGiaGio.Location = new System.Drawing.Point(360, 438);
            lblDonGiaGio.Name = "lblDonGiaGio";
            lblDonGiaGio.Size = new System.Drawing.Size(86, 20);
            lblDonGiaGio.TabIndex = 44;
            lblDonGiaGio.Text = "Đơn Giá Giờ";
            //
            // txtDonGiaGio
            //
            txtDonGiaGio.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtDonGiaGio.Location = new System.Drawing.Point(480, 435);
            txtDonGiaGio.MaxLength = 15;
            txtDonGiaGio.Name = "txtDonGiaGio";
            txtDonGiaGio.Size = new System.Drawing.Size(170, 27);
            txtDonGiaGio.TabIndex = 5;
            //
            // lblSoLuongDangCo
            //
            lblSoLuongDangCo.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblSoLuongDangCo.AutoSize = true;
            lblSoLuongDangCo.Location = new System.Drawing.Point(12, 478);
            lblSoLuongDangCo.Name = "lblSoLuongDangCo";
            lblSoLuongDangCo.Size = new System.Drawing.Size(122, 20);
            lblSoLuongDangCo.TabIndex = 45;
            lblSoLuongDangCo.Text = "Số Lượng Đang Có";
            //
            // txtSoLuongDangCo
            //
            txtSoLuongDangCo.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtSoLuongDangCo.Location = new System.Drawing.Point(150, 475);
            txtSoLuongDangCo.Name = "txtSoLuongDangCo";
            txtSoLuongDangCo.ReadOnly = true;
            txtSoLuongDangCo.Size = new System.Drawing.Size(170, 27);
            txtSoLuongDangCo.TabIndex = 46;
            //
            // lblTongSoPhong
            //
            lblTongSoPhong.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblTongSoPhong.AutoSize = true;
            lblTongSoPhong.Location = new System.Drawing.Point(360, 478);
            lblTongSoPhong.Name = "lblTongSoPhong";
            lblTongSoPhong.Size = new System.Drawing.Size(105, 20);
            lblTongSoPhong.TabIndex = 47;
            lblTongSoPhong.Text = "Tổng Số Phòng";
            //
            // txtTongSoPhong
            //
            txtTongSoPhong.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtTongSoPhong.Location = new System.Drawing.Point(480, 475);
            txtTongSoPhong.Name = "txtTongSoPhong";
            txtTongSoPhong.ReadOnly = true;
            txtTongSoPhong.Size = new System.Drawing.Size(170, 27);
            txtTongSoPhong.TabIndex = 48;
            //
            // btnThem
            //
            btnThem.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnThem.Location = new System.Drawing.Point(12, 524);
            btnThem.Name = "btnThem";
            btnThem.Size = new System.Drawing.Size(94, 29);
            btnThem.TabIndex = 6;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            //
            // btnSua
            //
            btnSua.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnSua.Location = new System.Drawing.Point(125, 524);
            btnSua.Name = "btnSua";
            btnSua.Size = new System.Drawing.Size(94, 29);
            btnSua.TabIndex = 7;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            //
            // btnXoa
            //
            btnXoa.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnXoa.Location = new System.Drawing.Point(238, 524);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new System.Drawing.Size(94, 29);
            btnXoa.TabIndex = 8;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            //
            // btnLuu
            //
            btnLuu.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnLuu.Location = new System.Drawing.Point(351, 524);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new System.Drawing.Size(94, 29);
            btnLuu.TabIndex = 9;
            btnLuu.Text = "Lưu";
            btnLuu.UseVisualStyleBackColor = true;
            //
            // btnHuy
            //
            btnHuy.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnHuy.Location = new System.Drawing.Point(464, 524);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new System.Drawing.Size(94, 29);
            btnHuy.TabIndex = 10;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = true;
            //
            // frmLoaiPhong
            //
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1137, 568);
            Controls.Add(btnHuy);
            Controls.Add(btnLuu);
            Controls.Add(btnXoa);
            Controls.Add(btnSua);
            Controls.Add(btnThem);
            Controls.Add(txtTongSoPhong);
            Controls.Add(lblTongSoPhong);
            Controls.Add(txtSoLuongDangCo);
            Controls.Add(lblSoLuongDangCo);
            Controls.Add(txtDonGiaGio);
            Controls.Add(lblDonGiaGio);
            Controls.Add(txtDonGiaNgay);
            Controls.Add(lblDonGiaNgay);
            Controls.Add(txtTenLoai);
            Controls.Add(lblTenLoai);
            Controls.Add(txtMaLoai);
            Controls.Add(lblMaLoai);
            Controls.Add(lvLoaiPhong);
            Controls.Add(btnTimNangCao);
            Controls.Add(txtTimNhanh);
            Controls.Add(lblTimNhanh);
            Controls.Add(lbltieude);
            Name = "frmLoaiPhong";
            Text = "frmLoaiPhong";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lbltieude;
        private System.Windows.Forms.Label lblTimNhanh;
        private System.Windows.Forms.TextBox txtTimNhanh;
        private System.Windows.Forms.Button btnTimNangCao;
        private System.Windows.Forms.ListView lvLoaiPhong;
        private System.Windows.Forms.Label lblMaLoai;
        private System.Windows.Forms.TextBox txtMaLoai;
        private System.Windows.Forms.Label lblTenLoai;
        private System.Windows.Forms.TextBox txtTenLoai;
        private System.Windows.Forms.Label lblDonGiaNgay;
        private System.Windows.Forms.TextBox txtDonGiaNgay;
        private System.Windows.Forms.Label lblDonGiaGio;
        private System.Windows.Forms.TextBox txtDonGiaGio;
        private System.Windows.Forms.Label lblSoLuongDangCo;
        private System.Windows.Forms.TextBox txtSoLuongDangCo;
        private System.Windows.Forms.Label lblTongSoPhong;
        private System.Windows.Forms.TextBox txtTongSoPhong;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
    }
}
