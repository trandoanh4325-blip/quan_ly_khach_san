using System;
using System.Collections;
using System.Windows.Forms;
using QLKS.BUS;
using QLKS.Common;
using QLKS.DTO;
using System.Windows.Forms;
using System.Drawing;

namespace QLKS.GUI
{
    public partial class frmKhachHang : Form
    {
        private readonly KhachHangBUS bus = new KhachHangBUS();
        private bool dangThem;
        private int daDay;
        private bool dangSapXep;
        private readonly ucTimKiemNangCao ucTim = new ucTimKiemNangCao();

        private readonly CotHienThi[] cot =
        {
            new CotHienThi("Mã KH", "MaKH", 80),
            new CotHienThi("Họ và tên", "HoTen", 220),
            new CotHienThi("CCCD", "CCCD", 140),
            new CotHienThi("Giới tính", "GioiTinh", 90),
            new CotHienThi("Tổng thu", "TongThu", 130),
            new CotHienThi("Số lượt tới", "SoLuotToi", 100)
        };

        public frmKhachHang()
        {
            InitializeComponent();
            Load += frmKhachHang_Load;
            txtTimNhanh.TextChanged += (s, e) => HienThi(bus.TimNhanh(txtTimNhanh.Text));
            btnTimNangCao.Click += (s, e) => BatTatTimNangCao();
            lvKhachHang.SelectedIndexChanged += lvKhachHang_SelectedIndexChanged;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnLuu.Click += btnLuu_Click;
            btnHuy.Click += btnHuy_Click;
            ucTim.Dock = DockStyle.Top;
            Controls.Add(ucTim);
            ucTim.BringToFront();
        }

        private void frmKhachHang_Load(object sender, EventArgs e)
        {
            LvHelper.TaoCot(lvKhachHang, cot);
            ucTim.DatTruong(cot);
            ucTim.Visible = false;
            ucTim.Dong += () => { ucTim.Visible = false; SapXepTimNangCao(); };
            ucTim.SizeChanged += (s2, e2) => { if (ucTim.Visible) SapXepTimNangCao(); };
            ucTim.TimKiem += dk => HienThi(bus.TimNangCao(dk));
            ucTim.XoaLoc += () => HienThi(bus.DanhSach);

            cboGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });

            HienThi(bus.DanhSach);
            LamTrongO();
            DatCheDo(false);
        }

        private void BatTatTimNangCao()
        {
            ucTim.Visible = !ucTim.Visible;
            if (ucTim.Visible) ucTim.BringToFront();
            SapXepTimNangCao();
        }

        private void SapXepTimNangCao()
        {
            if (dangSapXep) return;
            dangSapXep = true;
            try
            {
                int can = ucTim.Visible ? ucTim.Height : 0;
                if (ucTim.Visible)
                {
                    ucTim.Location = new Point(lvKhachHang.Left, lvKhachHang.Top - daDay);
                    ucTim.Width = lvKhachHang.Width;
                }
                int delta = can - daDay;
                lvKhachHang.Top += delta;
                lvKhachHang.Height -= delta;
                daDay = can;
            }
            finally
            {
                dangSapXep = false;
            }
        }

        private void HienThi(ArrayList ds)
        {
            LvHelper.Do(lvKhachHang, ds, cot);
        }

        private void DatCheDo(bool nhap)
        {
            txtHoTen.Enabled = nhap;
            txtCCCD.Enabled = nhap;
            cboGioiTinh.Enabled = nhap;
            btnThem.Enabled = !nhap;
            btnSua.Enabled = !nhap;
            btnXoa.Enabled = !nhap;
            btnLuu.Enabled = nhap;
            btnHuy.Enabled = nhap;
            lvKhachHang.Enabled = !nhap;
            txtTimNhanh.Enabled = !nhap;
            btnTimNangCao.Enabled = !nhap;
        }

        private void LamTrongO()
        {
            txtMaKH.Text = bus.SinhMa();
            txtHoTen.Clear();
            txtCCCD.Clear();
            cboGioiTinh.SelectedIndex = 0;
            txtTongThu.Text = "0";
            txtSoLuotToi.Text = "0";
        }

        private KhachHang DongDangChon()
        {
            if (lvKhachHang.SelectedItems.Count == 0) return null;
            return (KhachHang)lvKhachHang.SelectedItems[0].Tag;
        }

        private void lvKhachHang_SelectedIndexChanged(object sender, EventArgs e)
        {
            KhachHang kh = DongDangChon();
            if (kh == null) return;
            txtMaKH.Text = kh.MaKH;
            txtHoTen.Text = kh.HoTen;
            txtCCCD.Text = kh.CCCD;
            cboGioiTinh.Text = kh.GioiTinh;
            txtTongThu.Text = kh.TongThu.ToString("N0");
            txtSoLuotToi.Text = kh.SoLuotToi.ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            dangThem = true;
            LamTrongO();
            DatCheDo(true);
            txtHoTen.Focus();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (DongDangChon() == null)
            {
                MessageBox.Show("Vui lòng chọn dòng cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            dangThem = false;
            DatCheDo(true);
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            var kh = new KhachHang
            {
                MaKH = txtMaKH.Text,
                HoTen = txtHoTen.Text,
                CCCD = txtCCCD.Text,
                GioiTinh = cboGioiTinh.Text
            };
            string loi;
            bool ok = dangThem ? bus.Them(kh, out loi) : bus.Sua(kh, out loi);
            if (!ok)
            {
                MessageBox.Show(loi, "Không thể lưu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            txtTimNhanh.Text = "";
            HienThi(bus.DanhSach);
            DatCheDo(false);
            MessageBox.Show("Lưu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            DatCheDo(false);
            if (DongDangChon() != null) lvKhachHang_SelectedIndexChanged(null, null);
            else LamTrongO();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            KhachHang kh = DongDangChon();
            if (kh == null)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có chắc muốn xóa khách hàng " + kh.HoTen + "?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            string loi;
            if (!bus.Xoa(kh.MaKH, out loi))
            {
                MessageBox.Show(loi, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            HienThi(bus.DanhSach);
            LamTrongO();
        }
    }
}
