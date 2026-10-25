namespace QLKS.GUI
{
    partial class frmThamSo
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
            txtTenThamSo = new System.Windows.Forms.TextBox();
            txtGiaTri = new System.Windows.Forms.TextBox();
            txtMoTa = new System.Windows.Forms.TextBox();
            btnThem = new System.Windows.Forms.Button();
            btnSua = new System.Windows.Forms.Button();
            btnXoa = new System.Windows.Forms.Button();
            btnLuu = new System.Windows.Forms.Button();
            btnHuy = new System.Windows.Forms.Button();
            lbltieude = new System.Windows.Forms.Label();
            lvThamSo = new System.Windows.Forms.ListView();
            lblTenThamSo = new System.Windows.Forms.Label();
            lblMoTa = new System.Windows.Forms.Label();
            lblTimNhanh = new System.Windows.Forms.Label();
            btnTimNangCao = new System.Windows.Forms.Button();
            txtTimNhanh = new System.Windows.Forms.TextBox();
            lblGiaTri = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // txtTenThamSo
            // 
            txtTenThamSo.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtTenThamSo.Location = new System.Drawing.Point(132, 406);
            txtTenThamSo.Name = "txtTenThamSo";
            txtTenThamSo.Size = new System.Drawing.Size(283, 27);
            txtTenThamSo.TabIndex = 2;
            // 
            // txtGiaTri
            // 
            txtGiaTri.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtGiaTri.Location = new System.Drawing.Point(530, 402);
            txtGiaTri.Name = "txtGiaTri";
            txtGiaTri.Size = new System.Drawing.Size(186, 27);
            txtGiaTri.TabIndex = 3;
            // 
            // txtMoTa
            // 
            txtMoTa.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            txtMoTa.Location = new System.Drawing.Point(130, 447);
            txtMoTa.Name = "txtMoTa";
            txtMoTa.Size = new System.Drawing.Size(394, 27);
            txtMoTa.TabIndex = 4;
            // 
            // btnThem
            // 
            btnThem.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnThem.Location = new System.Drawing.Point(12, 500);
            btnThem.Name = "btnThem";
            btnThem.Size = new System.Drawing.Size(94, 29);
            btnThem.TabIndex = 6;
            btnThem.Text = "Thêm ";
            btnThem.UseVisualStyleBackColor = true;
            // 
            // btnSua
            // 
            btnSua.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnSua.Location = new System.Drawing.Point(125, 500);
            btnSua.Name = "btnSua";
            btnSua.Size = new System.Drawing.Size(94, 29);
            btnSua.TabIndex = 7;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = true;
            // 
            // btnXoa
            // 
            btnXoa.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnXoa.Location = new System.Drawing.Point(238, 500);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new System.Drawing.Size(94, 29);
            btnXoa.TabIndex = 8;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = true;
            // 
            // btnLuu
            // 
            btnLuu.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnLuu.Location = new System.Drawing.Point(351, 500);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new System.Drawing.Size(94, 29);
            btnLuu.TabIndex = 9;
            btnLuu.Text = "Lưu";
            btnLuu.UseVisualStyleBackColor = true;
            // 
            // btnHuy
            // 
            btnHuy.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            btnHuy.Location = new System.Drawing.Point(464, 500);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new System.Drawing.Size(94, 29);
            btnHuy.TabIndex = 10;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = true;
            // 
            // lbltieude
            // 
            lbltieude.AutoSize = true;
            lbltieude.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lbltieude.Location = new System.Drawing.Point(12, 12);
            lbltieude.Name = "lbltieude";
            lbltieude.Size = new System.Drawing.Size(168, 31);
            lbltieude.TabIndex = 30;
            lbltieude.Text = "Bảng Tham Số";
            // 
            // lvThamSo
            // 
            lvThamSo.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            lvThamSo.FullRowSelect = true;
            lvThamSo.GridLines = true;
            lvThamSo.Location = new System.Drawing.Point(12, 55);
            lvThamSo.MultiSelect = false;
            lvThamSo.Name = "lvThamSo";
            lvThamSo.Size = new System.Drawing.Size(1113, 319);
            lvThamSo.TabIndex = 31;
            lvThamSo.UseCompatibleStateImageBehavior = false;
            lvThamSo.View = System.Windows.Forms.View.Details;
            // 
            // lblTenThamSo
            // 
            lblTenThamSo.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblTenThamSo.AutoSize = true;
            lblTenThamSo.Location = new System.Drawing.Point(32, 407);
            lblTenThamSo.Name = "lblTenThamSo";
            lblTenThamSo.Size = new System.Drawing.Size(94, 20);
            lblTenThamSo.TabIndex = 33;
            lblTenThamSo.Text = "Tên Tham Số";
            // 
            // lblMoTa
            // 
            lblMoTa.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblMoTa.AutoSize = true;
            lblMoTa.Location = new System.Drawing.Point(32, 445);
            lblMoTa.Name = "lblMoTa";
            lblMoTa.Size = new System.Drawing.Size(51, 20);
            lblMoTa.TabIndex = 34;
            lblMoTa.Text = "Mô Tả";
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
            // btnTimNangCao
            // 
            btnTimNangCao.Location = new System.Drawing.Point(968, 12);
            btnTimNangCao.Name = "btnTimNangCao";
            btnTimNangCao.Size = new System.Drawing.Size(157, 29);
            btnTimNangCao.TabIndex = 36;
            btnTimNangCao.Text = "Tìm kiếm Nâng Cao";
            btnTimNangCao.UseVisualStyleBackColor = true;
            // 
            // txtTimNhanh
            // 
            txtTimNhanh.Location = new System.Drawing.Point(600, 14);
            txtTimNhanh.Name = "txtTimNhanh";
            txtTimNhanh.Size = new System.Drawing.Size(362, 27);
            txtTimNhanh.TabIndex = 35;
            // 
            // lblGiaTri
            // 
            lblGiaTri.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            lblGiaTri.AutoSize = true;
            lblGiaTri.Location = new System.Drawing.Point(460, 403);
            lblGiaTri.Name = "lblGiaTri";
            lblGiaTri.Size = new System.Drawing.Size(51, 20);
            lblGiaTri.TabIndex = 38;
            lblGiaTri.Text = "Giá Trị";
            // 
            // frmThamSo
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1137, 568);
            Controls.Add(lblGiaTri);
            Controls.Add(lblTimNhanh);
            Controls.Add(btnTimNangCao);
            Controls.Add(txtTimNhanh);
            Controls.Add(lblMoTa);
            Controls.Add(lblTenThamSo);
            Controls.Add(lvThamSo);
            Controls.Add(lbltieude);
            Controls.Add(btnHuy);
            Controls.Add(btnLuu);
            Controls.Add(btnXoa);
            Controls.Add(btnSua);
            Controls.Add(btnThem);
            Controls.Add(txtMoTa);
            Controls.Add(txtGiaTri);
            Controls.Add(txtTenThamSo);
            Name = "frmThamSo";
            Text = "frmThamSo";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.TextBox txtTenThamSo;
        private System.Windows.Forms.TextBox txtGiaTri;
        private System.Windows.Forms.TextBox txtMoTa;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Label lbltieude;
        private System.Windows.Forms.ListView lvThamSo;
        private System.Windows.Forms.Label lblTenThamSo;
        private System.Windows.Forms.Label lblMoTa;
        private System.Windows.Forms.Label lblTimNhanh;
        private System.Windows.Forms.Button btnTimNangCao;
        private System.Windows.Forms.TextBox txtTimNhanh;
        private System.Windows.Forms.Label lblGiaTri;
    }
}