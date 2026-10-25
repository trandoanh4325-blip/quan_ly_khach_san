using System.Windows.Forms;

namespace QLKS.GUI
{
    partial class frmMain
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
            lblTieuDe = new Label();
            btnKhachHang = new Button();
            btnNhanVien = new Button();
            btnThamSo = new Button();
            pnlMenu = new Panel();
            btnThongKe = new Button();
            btnSanPham = new Button();
            btnHoaDon = new Button();
            btnDichVu = new Button();
            btnPhieuDatPhong = new Button();
            btnPhong = new Button();
            btnLoaiPhong = new Button();
            pnlNoiDung = new Panel();
            pnlMenu.SuspendLayout();
            SuspendLayout();
            // 
            // lblTieuDe
            // 
            lblTieuDe.BackColor = System.Drawing.Color.Cyan;
            lblTieuDe.Dock = DockStyle.Top;
            lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lblTieuDe.ForeColor = System.Drawing.Color.White;
            lblTieuDe.Location = new System.Drawing.Point(0, 0);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new System.Drawing.Size(1298, 66);
            lblTieuDe.TabIndex = 32;
            lblTieuDe.Text = "Khách Sạn";
            lblTieuDe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnKhachHang
            // 
            btnKhachHang.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnKhachHang.BackColor = System.Drawing.Color.White;
            btnKhachHang.Location = new System.Drawing.Point(12, 73);
            btnKhachHang.Name = "btnKhachHang";
            btnKhachHang.Size = new System.Drawing.Size(210, 38);
            btnKhachHang.TabIndex = 29;
            btnKhachHang.Text = "Khách Hàng";
            btnKhachHang.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnKhachHang.UseVisualStyleBackColor = false;
            // 
            // btnNhanVien
            // 
            btnNhanVien.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnNhanVien.BackColor = System.Drawing.Color.White;
            btnNhanVien.Location = new System.Drawing.Point(12, 27);
            btnNhanVien.Name = "btnNhanVien";
            btnNhanVien.Size = new System.Drawing.Size(210, 38);
            btnNhanVien.TabIndex = 28;
            btnNhanVien.Text = "Nhân Viên";
            btnNhanVien.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnNhanVien.UseVisualStyleBackColor = false;
            // 
            // btnThamSo
            // 
            btnThamSo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnThamSo.BackColor = System.Drawing.Color.White;
            btnThamSo.Location = new System.Drawing.Point(12, 119);
            btnThamSo.Name = "btnThamSo";
            btnThamSo.Size = new System.Drawing.Size(210, 38);
            btnThamSo.TabIndex = 27;
            btnThamSo.Text = "Tham số";
            btnThamSo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnThamSo.UseVisualStyleBackColor = false;
            // 
            // pnlMenu
            // 
            pnlMenu.Controls.Add(btnThongKe);
            pnlMenu.Controls.Add(btnSanPham);
            pnlMenu.Controls.Add(btnHoaDon);
            pnlMenu.Controls.Add(btnDichVu);
            pnlMenu.Controls.Add(btnPhieuDatPhong);
            pnlMenu.Controls.Add(btnPhong);
            pnlMenu.Controls.Add(btnLoaiPhong);
            pnlMenu.Controls.Add(btnNhanVien);
            pnlMenu.Controls.Add(btnKhachHang);
            pnlMenu.Controls.Add(btnThamSo);
            pnlMenu.Dock = DockStyle.Left;
            pnlMenu.Location = new System.Drawing.Point(0, 66);
            pnlMenu.Name = "pnlMenu";
            pnlMenu.Size = new System.Drawing.Size(240, 514);
            pnlMenu.TabIndex = 40;
            // 
            // btnThongKe
            // 
            btnThongKe.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnThongKe.BackColor = System.Drawing.Color.White;
            btnThongKe.Location = new System.Drawing.Point(12, 427);
            btnThongKe.Name = "btnThongKe";
            btnThongKe.Size = new System.Drawing.Size(210, 38);
            btnThongKe.TabIndex = 36;
            btnThongKe.Text = "Thống Kê";
            btnThongKe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnThongKe.UseVisualStyleBackColor = false;
            // 
            // btnSanPham
            // 
            btnSanPham.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnSanPham.BackColor = System.Drawing.Color.White;
            btnSanPham.Location = new System.Drawing.Point(12, 383);
            btnSanPham.Name = "btnSanPham";
            btnSanPham.Size = new System.Drawing.Size(210, 38);
            btnSanPham.TabIndex = 35;
            btnSanPham.Text = "Sản Phẩm";
            btnSanPham.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnSanPham.UseVisualStyleBackColor = false;
            // 
            // btnHoaDon
            // 
            btnHoaDon.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnHoaDon.BackColor = System.Drawing.Color.White;
            btnHoaDon.Location = new System.Drawing.Point(12, 339);
            btnHoaDon.Name = "btnHoaDon";
            btnHoaDon.Size = new System.Drawing.Size(210, 38);
            btnHoaDon.TabIndex = 34;
            btnHoaDon.Text = "Hóa Đơn";
            btnHoaDon.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnHoaDon.UseVisualStyleBackColor = false;
            // 
            // btnDichVu
            // 
            btnDichVu.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnDichVu.BackColor = System.Drawing.Color.White;
            btnDichVu.Location = new System.Drawing.Point(12, 295);
            btnDichVu.Name = "btnDichVu";
            btnDichVu.Size = new System.Drawing.Size(210, 38);
            btnDichVu.TabIndex = 33;
            btnDichVu.Text = "Dịch Vụ";
            btnDichVu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnDichVu.UseVisualStyleBackColor = false;
            // 
            // btnPhieuDatPhong
            // 
            btnPhieuDatPhong.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnPhieuDatPhong.BackColor = System.Drawing.Color.White;
            btnPhieuDatPhong.Location = new System.Drawing.Point(12, 251);
            btnPhieuDatPhong.Name = "btnPhieuDatPhong";
            btnPhieuDatPhong.Size = new System.Drawing.Size(210, 38);
            btnPhieuDatPhong.TabIndex = 32;
            btnPhieuDatPhong.Text = "Phiếu Đặt Phòng";
            btnPhieuDatPhong.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnPhieuDatPhong.UseVisualStyleBackColor = false;
            // 
            // btnPhong
            // 
            btnPhong.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnPhong.BackColor = System.Drawing.Color.White;
            btnPhong.Location = new System.Drawing.Point(12, 207);
            btnPhong.Name = "btnPhong";
            btnPhong.Size = new System.Drawing.Size(210, 38);
            btnPhong.TabIndex = 31;
            btnPhong.Text = "Phòng";
            btnPhong.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnPhong.UseVisualStyleBackColor = false;
            // 
            // btnLoaiPhong
            // 
            btnLoaiPhong.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnLoaiPhong.BackColor = System.Drawing.Color.White;
            btnLoaiPhong.Location = new System.Drawing.Point(12, 163);
            btnLoaiPhong.Name = "btnLoaiPhong";
            btnLoaiPhong.Size = new System.Drawing.Size(210, 38);
            btnLoaiPhong.TabIndex = 30;
            btnLoaiPhong.Text = "Loại Phòng";
            btnLoaiPhong.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btnLoaiPhong.UseVisualStyleBackColor = false;
            // 
            // pnlNoiDung
            // 
            pnlNoiDung.BackColor = System.Drawing.Color.White;
            pnlNoiDung.Dock = DockStyle.Fill;
            pnlNoiDung.Location = new System.Drawing.Point(240, 66);
            pnlNoiDung.Name = "pnlNoiDung";
            pnlNoiDung.Size = new System.Drawing.Size(1058, 514);
            pnlNoiDung.TabIndex = 41;
            // 
            // frmMain
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1298, 580);
            Controls.Add(pnlNoiDung);
            Controls.Add(pnlMenu);
            Controls.Add(lblTieuDe);
            Name = "frmMain";
            Text = "Quản Lý Khách Sạn";
            WindowState = FormWindowState.Maximized;
            pnlMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label lblChucVu;
        private ucTimKiemNangCao ucTimKiemNangCao1;
        private System.Windows.Forms.Label lblHoTen;
        private System.Windows.Forms.Label lblSDT;
        private System.Windows.Forms.Label lblMaNV;
        private System.Windows.Forms.Label lblTimKiem;
        private System.Windows.Forms.Label lblTieuDe;
        private Button btnKhachHang;
        private Button btnNhanVien;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnThamSo;
        private System.Windows.Forms.TextBox txtSDT;
        private System.Windows.Forms.Button btnTimKiemNangCao;
        private System.Windows.Forms.ComboBox cboChucVu;
        private System.Windows.Forms.TextBox txtMaNV;
        private System.Windows.Forms.TextBox txtHoTen;
        private System.Windows.Forms.TextBox txtTimKiem;
        private System.Windows.Forms.Panel pnlMenu;
        private Panel pnlNoiDung;
        private Button btnPhieuDatPhong;
        private Button btnPhong;
        private Button btnLoaiPhong;
        private Button btnThongKe;
        private Button btnSanPham;
        private Button btnHoaDon;
        private Button btnDichVu;
    }
}