namespace QLKS.GUI
{
    partial class frmNhanVien
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
            btnHuy = new System.Windows.Forms.Button();
            btnLuu = new System.Windows.Forms.Button();
            btnXoa = new System.Windows.Forms.Button();
            btnSua = new System.Windows.Forms.Button();
            btnThem = new System.Windows.Forms.Button();
            txtHoTen = new System.Windows.Forms.TextBox();
            cboChucVu = new System.Windows.Forms.ComboBox();
            txtSDT = new System.Windows.Forms.TextBox();
            txtMaNV = new System.Windows.Forms.TextBox();
            txtTimNhanh = new System.Windows.Forms.TextBox();
            btnTimNangCao = new System.Windows.Forms.Button();
            lbltieude = new System.Windows.Forms.Label();
            lblTimNhanh = new System.Windows.Forms.Label();
            lvNhanVien = new System.Windows.Forms.ListView();
            lblMaNV = new System.Windows.Forms.Label();
            lblSDT = new System.Windows.Forms.Label();
            lblHoTen = new System.Windows.Forms.Label();
            ucTimKiemNangCao1 = new ucTimKiemNangCao();
            lblChucVu = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // btnHuy
            // 
            btnHuy.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnHuy.Location = new System.Drawing.Point(464, 694);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new System.Drawing.Size(94, 29);
            btnHuy.TabIndex = 12;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = true;
            // 
            // btnLuu
            // 
            btnLuu.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnLuu.Location = new System.Drawing.Point(351, 694);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new System.Drawing.Size(94, 29);
            btnLuu.TabIndex = 11;
            btnLuu.Text = "Lưu";
            btnLuu.UseVisualStyleBackColor = true;
            // 
            // btnXoa
            // 
            btnXoa.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnXoa.Location = new System.Drawing.Point(238, 694);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new System.Drawing.Size(94, 29);
            btnXoa.TabIndex = 10;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            // 
            // btnSua
            // 
            btnSua.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnSua.Location = new System.Drawing.Point(125, 694);
            btnSua.Name = "btnSua";
            btnSua.Size = new System.Drawing.Size(94, 29);
            btnSua.TabIndex = 9;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            // 
            // btnThem
            // 
            btnThem.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnThem.Location = new System.Drawing.Point(12, 694);
            btnThem.Name = "btnThem";
            btnThem.Size = new System.Drawing.Size(94, 29);
            btnThem.TabIndex = 8;
            btnThem.Text = "Thêm";
            btnThem.UseVisualStyleBackColor = true;
            // 
            // txtHoTen
            // 
            txtHoTen.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtHoTen.Location = new System.Drawing.Point(450, 596);
            txtHoTen.MaxLength = 100;
            txtHoTen.Name = "txtHoTen";
            txtHoTen.Size = new System.Drawing.Size(236, 27);
            txtHoTen.TabIndex = 3;
            // 
            // cboChucVu
            // 
            cboChucVu.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            cboChucVu.FormattingEnabled = true;
            cboChucVu.Location = new System.Drawing.Point(130, 641);
            cboChucVu.Name = "cboChucVu";
            cboChucVu.Size = new System.Drawing.Size(151, 28);
            cboChucVu.TabIndex = 5;
            cboChucVu.Text = "Chức vụ";
            // 
            // txtSDT
            // 
            txtSDT.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtSDT.Location = new System.Drawing.Point(450, 641);
            txtSDT.MaxLength = 11;
            txtSDT.Name = "txtSDT";
            txtSDT.Size = new System.Drawing.Size(158, 27);
            txtSDT.TabIndex = 7;
            // 
            // txtMaNV
            // 
            txtMaNV.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtMaNV.Location = new System.Drawing.Point(130, 596);
            txtMaNV.MaxLength = 12;
            txtMaNV.Name = "txtMaNV";
            txtMaNV.Size = new System.Drawing.Size(144, 27);
            txtMaNV.TabIndex = 4;
            // 
            // txtTimNhanh
            // 
            txtTimNhanh.Location = new System.Drawing.Point(598, 12);
            txtTimNhanh.Name = "txtTimNhanh";
            txtTimNhanh.Size = new System.Drawing.Size(362, 27);
            txtTimNhanh.TabIndex = 2;
            // 
            // btnTimNangCao
            // 
            btnTimNangCao.Location = new System.Drawing.Point(966, 11);
            btnTimNangCao.Name = "btnTimNangCao";
            btnTimNangCao.Size = new System.Drawing.Size(157, 29);
            btnTimNangCao.TabIndex = 6;
            btnTimNangCao.Text = "Tìm kiếm Nâng Cao";
            btnTimNangCao.UseVisualStyleBackColor = true;
            // 
            // lbltieude
            // 
            lbltieude.AutoSize = true;
            lbltieude.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lbltieude.Location = new System.Drawing.Point(12, 12);
            lbltieude.Name = "lbltieude";
            lbltieude.Size = new System.Drawing.Size(224, 31);
            lbltieude.TabIndex = 13;
            lbltieude.Text = "Quản Lý Nhân Viên ";
            // 
            // lblTimNhanh
            // 
            lblTimNhanh.AutoSize = true;
            lblTimNhanh.Location = new System.Drawing.Point(520, 18);
            lblTimNhanh.Name = "lblTimNhanh";
            lblTimNhanh.Size = new System.Drawing.Size(72, 20);
            lblTimNhanh.TabIndex = 14;
            lblTimNhanh.Text = "Tìm Kiếm";
            // 
            // lvNhanVien
            // 
            lvNhanVien.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lvNhanVien.FullRowSelect = true;
            lvNhanVien.GridLines = true;
            lvNhanVien.Location = new System.Drawing.Point(12, 55);
            lvNhanVien.MultiSelect = false;
            lvNhanVien.Name = "lvNhanVien";
            lvNhanVien.Size = new System.Drawing.Size(1317, 524);
            lvNhanVien.TabIndex = 15;
            lvNhanVien.UseCompatibleStateImageBehavior = false;
            lvNhanVien.View = System.Windows.Forms.View.Details;
            // 
            // lblMaNV
            // 
            lblMaNV.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblMaNV.AutoSize = true;
            lblMaNV.Location = new System.Drawing.Point(12, 599);
            lblMaNV.Name = "lblMaNV";
            lblMaNV.Size = new System.Drawing.Size(102, 20);
            lblMaNV.TabIndex = 16;
            lblMaNV.Text = "Mã Nhân Viên";
            // 
            // lblSDT
            // 
            lblSDT.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblSDT.AutoSize = true;
            lblSDT.Location = new System.Drawing.Point(318, 644);
            lblSDT.Name = "lblSDT";
            lblSDT.Size = new System.Drawing.Size(102, 20);
            lblSDT.TabIndex = 17;
            lblSDT.Text = "Số Điện Thoại";
            // 
            // lblHoTen
            // 
            lblHoTen.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblHoTen.AutoSize = true;
            lblHoTen.Location = new System.Drawing.Point(360, 599);
            lblHoTen.Name = "lblHoTen";
            lblHoTen.Size = new System.Drawing.Size(60, 20);
            lblHoTen.TabIndex = 18;
            lblHoTen.Text = "Họ Tên ";
            // 
            // ucTimKiemNangCao1
            // 
            ucTimKiemNangCao1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            ucTimKiemNangCao1.BackColor = System.Drawing.Color.WhiteSmoke;
            ucTimKiemNangCao1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            ucTimKiemNangCao1.Location = new System.Drawing.Point(639, 641);
            ucTimKiemNangCao1.Name = "ucTimKiemNangCao1";
            ucTimKiemNangCao1.Size = new System.Drawing.Size(736, 44);
            ucTimKiemNangCao1.TabIndex = 19;
            // 
            // lblChucVu
            // 
            lblChucVu.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblChucVu.AutoSize = true;
            lblChucVu.Location = new System.Drawing.Point(12, 644);
            lblChucVu.Name = "lblChucVu";
            lblChucVu.Size = new System.Drawing.Size(63, 20);
            lblChucVu.TabIndex = 20;
            lblChucVu.Text = "Chức Vụ";
            // 
            // frmNhanVien
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1341, 751);
            Controls.Add(lblChucVu);
            Controls.Add(ucTimKiemNangCao1);
            Controls.Add(lblHoTen);
            Controls.Add(lblSDT);
            Controls.Add(lblMaNV);
            Controls.Add(lvNhanVien);
            Controls.Add(lblTimNhanh);
            Controls.Add(lbltieude);
            Controls.Add(btnHuy);
            Controls.Add(btnLuu);
            Controls.Add(btnXoa);
            Controls.Add(btnSua);
            Controls.Add(btnThem);
            Controls.Add(txtSDT);
            Controls.Add(btnTimNangCao);
            Controls.Add(cboChucVu);
            Controls.Add(txtMaNV);
            Controls.Add(txtHoTen);
            Controls.Add(txtTimNhanh);
            Name = "frmNhanVien";
            Text = "frmNhanVien";
            Load += frmNhanVien_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.TextBox txtHoTen;
        private System.Windows.Forms.ComboBox cboChucVu;
        private System.Windows.Forms.TextBox txtSDT;
        private System.Windows.Forms.TextBox txtMaNV;
        private System.Windows.Forms.TextBox txtTimNhanh;
        private System.Windows.Forms.Button btnTimNangCao;
        private System.Windows.Forms.Label lbltieude;
        private System.Windows.Forms.Label lblTimNhanh;
        private System.Windows.Forms.ListView lvNhanVien;
        private System.Windows.Forms.Label lblMaNV;
        private System.Windows.Forms.Label lblSDT;
        private System.Windows.Forms.Label lblHoTen;
        private ucTimKiemNangCao ucTimKiemNangCao1;
        private System.Windows.Forms.Label lblChucVu;
    }
}