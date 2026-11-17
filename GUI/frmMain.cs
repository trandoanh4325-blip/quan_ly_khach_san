using System;
using System.Drawing;
using System.Windows.Forms;
using QLKS.BUS;

namespace QLKS.GUI
{
    public partial class frmMain : Form
    {
        private Form formCon;
        private Button nutDangChon;

        public frmMain()
        {
            InitializeComponent();
            Load += frmMain_Load;
            btnNhanVien.Click += (s, e) => MoForm(new frmNhanVien(), btnNhanVien);
            btnKhachHang.Click += (s, e) => MoForm(new frmKhachHang(), btnKhachHang);
            btnThamSo.Click += (s, e) => MoForm(new frmThamSo(), btnThamSo);

            // ---- TV2 (bỏ comment khi đã vẽ nút và có form) ----
            // btnLoaiPhong.Click += (s, e) => MoForm(new frmLoaiPhong(), btnLoaiPhong);
            // btnPhong.Click += (s, e) => MoForm(new frmPhong(), btnPhong);
            // btnPhieuDatPhong.Click += (s, e) => MoForm(new frmPhieuDatPhong(), btnPhieuDatPhong);

            // ---- TV3 ----
            // btnDichVu.Click += (s, e) => MoForm(new frmDichVu(), btnDichVu);
            // btnHoaDon.Click += (s, e) => MoForm(new frmHoaDon(), btnHoaDon);

            // ---- TV4 ----
            btnSanPham.Click += (s, e) => MoForm(new frmSanPham(), btnSanPham);
            btnThongKe.Click += (s, e) => MoForm(new frmThongKe(), btnThongKe);
            // Các module TV4 bổ sung bằng code, không sửa Designer của TV1.
            pnlMenu.AutoScroll = true;
            foreach (Control c in pnlMenu.Controls)
                if (c is Button b) b.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            ThemNutTV4("Loại sản phẩm", 475, () => new frmLoaiSanPham());
            ThemNutTV4("Nhà cung cấp", 519, () => new frmNhaCungCap());
            ThemNutTV4("Phiếu nhập hàng", 563, () => new frmPhieuNhap());
            ThemNutTV4("Excel - Dữ liệu", 607, () => new frmExcelTV4());
        }

        private void ThemNutTV4(string nhan, int y, Func<Form> taoForm)
        {
            var b = new Button
            {
                Text = nhan,
                Left = 12,
                Top = y,
                Width = 210,
                Height = 38,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.White,
                Anchor = AnchorStyles.Top | AnchorStyles.Left
            };
            b.Click += (s, e) => MoForm(taoForm(), b);
            pnlMenu.Controls.Add(b);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lblTieuDe.Text = ThamSoBUS.Doc("ten_khach_san", "QUẢN LÝ KHÁCH SẠN").ToUpper();
            
        }
        private void MoForm(Form f, Button nut)
        {
            if (nutDangChon != null) nutDangChon.BackColor = SystemColors.Control;
            nut.BackColor = Color.FromArgb(217, 226, 243);
            nutDangChon = nut;

            if (formCon != null)
            {
                pnlNoiDung.Controls.Remove(formCon);
                formCon.Dispose();
            }
            

            formCon = f;
            f.TopLevel = false;
            f.FormBorderStyle = FormBorderStyle.None;
            f.Dock = DockStyle.Fill;
            pnlNoiDung.Controls.Add(f);
            f.BringToFront();
            f.Show();
        }
    }
}
