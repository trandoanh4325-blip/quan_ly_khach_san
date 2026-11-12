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
        private readonly Button btnPhuThu = new Button { Name = "btnPhuThu", Text = "Phụ Thu" };
        private readonly Button btnKhuyenMai = new Button { Name = "btnKhuyenMai", Text = "Khuyến Mãi" };

        public frmMain()
        {
            InitializeComponent();
            pnlMenu.AutoScroll = true;
            pnlMenu.Controls.Add(btnPhuThu);
            pnlMenu.Controls.Add(btnKhuyenMai);
            var menu = new[] { btnNhanVien, btnKhachHang, btnThamSo, btnLoaiPhong, btnPhong, btnPhieuDatPhong,
                btnDichVu, btnPhuThu, btnKhuyenMai, btnHoaDon, btnSanPham, btnThongKe };
            for (int i = 0; i < menu.Length; i++)
            {
                menu[i].Anchor = AnchorStyles.Top | AnchorStyles.Left;
                menu[i].Location = new Point(12, 12 + i * 44);
                menu[i].Size = new Size(Math.Max(120, pnlMenu.ClientSize.Width - 24 - SystemInformation.VerticalScrollBarWidth), 38);
                menu[i].BackColor = Color.White;
                menu[i].TextAlign = ContentAlignment.MiddleLeft;
            }
            Load += frmMain_Load;
            btnNhanVien.Click += (s, e) => MoForm(new frmNhanVien(), btnNhanVien);
            btnKhachHang.Click += (s, e) => MoForm(new frmKhachHang(), btnKhachHang);
            btnThamSo.Click += (s, e) => MoForm(new frmThamSo(), btnThamSo);

            // ---- TV2 (bỏ comment khi đã vẽ nút và có form) ----
            btnLoaiPhong.Click += (s, e) => MoForm(new frmLoaiPhong(), btnLoaiPhong);
            btnPhong.Click += (s, e) => MoForm(new frmPhong(), btnPhong);
            btnPhieuDatPhong.Click += (s, e) => MoForm(new frmPhieuDatPhong(), btnPhieuDatPhong);

            // ---- TV3 ----
            btnDichVu.Click += (s, e) => MoForm(new frmDichVu(), btnDichVu);
            btnHoaDon.Click += (s, e) => MoForm(new frmHoaDon(), btnHoaDon);
            btnPhuThu.Click += (s, e) => MoForm(new frmPhuThu(), btnPhuThu);
            btnKhuyenMai.Click += (s, e) => MoForm(new frmKhuyenMai(), btnKhuyenMai);
            FormClosing += (s, e) => { if (!CoTheDoiForm()) e.Cancel = true; };

            // ---- TV4 ----
            // btnSanPham.Click += (s, e) => MoForm(new frmSanPham(), btnSanPham);
            // btnThongKe.Click += (s, e) => MoForm(new frmThongKe(), btnThongKe);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lblTieuDe.Text = ThamSoBUS.Doc("ten_khach_san", "QUẢN LÝ KHÁCH SẠN").ToUpper();

        }
        private void MoForm(Form f, Button nut)
        {
            if (!CoTheDoiForm()) { f.Dispose(); return; }
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

        private bool CoTheDoiForm()
        {
            if (formCon is frmHoaDon hd) return hd.CoTheDong();
            if (formCon is frmPhuThu pt) return pt.CoTheDong();
            if (formCon is frmKhuyenMai km) return km.CoTheDong();
            return true;
        }
    }
}
