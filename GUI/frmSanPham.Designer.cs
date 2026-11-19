using System.Drawing;
using System.Windows.Forms;

namespace QLKS.GUI
{
    partial class frmSanPham
    {
        private System.ComponentModel.IContainer components = null;
        private TableLayoutPanel bangChinh;
        private Label lblTieuDe;
        private TableLayoutPanel bangNhap;
        private TextBox txtMaSP;
        private TextBox txtTenSP;
        private ComboBox cboLoaiSP;
        private TextBox txtDonViTinh;
        private NumericUpDown nudGiaBan;
        private TextBox txtSoLuongTon;
        private FlowLayoutPanel pnlNut;
        private Button btnThem, btnSua, btnXoa, btnExcel, btnLuu, btnHuy, btnTaiLai;
        private FlowLayoutPanel pnlTimNhanh;
        private TextBox txtTimNhanh;
        private Button btnTimNangCao;
        private Panel pnlTimNangCao;
        private ListView lvSanPham;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null) components.Dispose();
            base.Dispose(disposing);
        }

        private Label Nhan(string noiDung) => new Label
        {
            Text = noiDung, Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft,
            AutoSize = false, Margin = new Padding(6, 4, 4, 4)
        };

        private Button Nut(string noiDung) => new Button
        {
            Text = noiDung, Width = 104, Height = 34, Margin = new Padding(5, 3, 5, 3),
            FlatStyle = FlatStyle.System
        };

        private void InitializeComponent()
        {
            SuspendLayout();
            bangChinh = new TableLayoutPanel {
                Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 6,
                Padding = new Padding(12), BackColor = Color.White
            };
            bangChinh.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            bangChinh.RowStyles.Add(new RowStyle(SizeType.Absolute, 48));
            bangChinh.RowStyles.Add(new RowStyle(SizeType.Absolute, 150));
            bangChinh.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            bangChinh.RowStyles.Add(new RowStyle(SizeType.Absolute, 44));
            bangChinh.RowStyles.Add(new RowStyle(SizeType.Absolute, 0));
            bangChinh.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            lblTieuDe = new Label {
                Name = "lblTieuDe", Text = "QUẢN LÝ SẢN PHẨM", Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 17F, FontStyle.Bold), ForeColor = Color.Navy,
                TextAlign = ContentAlignment.MiddleCenter
            };
            bangChinh.Controls.Add(lblTieuDe, 0, 0);

            bangNhap = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 4, RowCount = 3, Padding = new Padding(0, 4, 0, 4) };
            bangNhap.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            bangNhap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            bangNhap.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120));
            bangNhap.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            for (int i = 0; i < 3; i++) bangNhap.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33F));

            txtMaSP = new TextBox { Name = "txtMaSP", Dock = DockStyle.Fill, MaxLength = 10, ReadOnly = true };
            txtTenSP = new TextBox { Name = "txtTenSP", Dock = DockStyle.Fill, MaxLength = 100 };
            cboLoaiSP = new ComboBox { Name = "cboLoaiSP", Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
            txtDonViTinh = new TextBox { Name = "txtDonViTinh", Dock = DockStyle.Fill, MaxLength = 20 };
            nudGiaBan = new NumericUpDown { Name = "nudGiaBan", Dock = DockStyle.Fill, DecimalPlaces = 0, ThousandsSeparator = true, Maximum = 999999999999999M };
            txtSoLuongTon = new TextBox { Name = "txtSoLuongTon", Dock = DockStyle.Fill, ReadOnly = true };
            bangNhap.Controls.Add(Nhan("Mã sản phẩm"), 0, 0);
            bangNhap.Controls.Add(txtMaSP, 1, 0);
            bangNhap.Controls.Add(Nhan("Tên sản phẩm"), 2, 0);
            bangNhap.Controls.Add(txtTenSP, 3, 0);
            bangNhap.Controls.Add(Nhan("Loại sản phẩm"), 0, 1);
            bangNhap.Controls.Add(cboLoaiSP, 1, 1);
            bangNhap.Controls.Add(Nhan("Đơn vị tính"), 2, 1);
            bangNhap.Controls.Add(txtDonViTinh, 3, 1);
            bangNhap.Controls.Add(Nhan("Giá bán / đền bù"), 0, 2);
            bangNhap.Controls.Add(nudGiaBan, 1, 2);
            bangNhap.Controls.Add(Nhan("Số lượng tồn"), 2, 2);
            bangNhap.Controls.Add(txtSoLuongTon, 3, 2);
            bangChinh.Controls.Add(bangNhap, 0, 1);

            pnlNut = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, AutoScroll = true };
            btnThem = Nut("Thêm"); btnSua = Nut("Sửa"); btnXoa = Nut("Xóa");
            btnExcel = Nut("Excel"); btnLuu = Nut("Lưu"); btnHuy = Nut("Hủy"); btnTaiLai = Nut("Tải lại");
            pnlNut.Controls.AddRange(new Control[] { btnThem, btnSua, btnXoa, btnExcel, btnLuu, btnHuy, btnTaiLai });
            bangChinh.Controls.Add(pnlNut, 0, 2);

            pnlTimNhanh = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, AutoScroll = true };
            pnlTimNhanh.Controls.Add(new Label { Text = "Tìm kiếm:", Width = 85, Height = 30, TextAlign = ContentAlignment.MiddleLeft });
            txtTimNhanh = new TextBox { Name = "txtTimNhanh", Width = 320, PlaceholderText = "Mã, tên hoặc loại sản phẩm..." };
            btnTimNangCao = Nut("Tìm nâng cao"); btnTimNangCao.Width = 130;
            pnlTimNhanh.Controls.Add(txtTimNhanh);
            pnlTimNhanh.Controls.Add(btnTimNangCao);
            bangChinh.Controls.Add(pnlTimNhanh, 0, 3);

            pnlTimNangCao = new Panel { Name = "pnlTimNangCao", Dock = DockStyle.Fill };
            bangChinh.Controls.Add(pnlTimNangCao, 0, 4);

            lvSanPham = new ListView {
                Name = "lvSanPham", Dock = DockStyle.Fill, View = View.Details,
                FullRowSelect = true, GridLines = true, MultiSelect = false,
                HideSelection = false, UseCompatibleStateImageBehavior = false
            };
            bangChinh.Controls.Add(lvSanPham, 0, 5);

            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1080, 680);
            MinimumSize = new Size(780, 440);
            Font = new Font("Segoe UI", 10F);
            BackColor = Color.White;
            Controls.Add(bangChinh);
            Name = "frmSanPham";
            Text = "QUẢN LÝ SẢN PHẨM";
            StartPosition = FormStartPosition.CenterScreen;
            ResumeLayout(false);
        }
    }
}
