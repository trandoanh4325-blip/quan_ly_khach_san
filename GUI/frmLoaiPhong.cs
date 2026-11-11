using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using QLKS.BUS;
using QLKS.Common;
using QLKS.DTO;

namespace QLKS.GUI
{
    public partial class frmLoaiPhong : Form
    {
        private readonly LoaiPhongBUS bus = new LoaiPhongBUS();
        private bool dangThem;
        private int daDay;
        private bool dangSapXep;
        private readonly ucTimKiemNangCao ucTim = new ucTimKiemNangCao();

        private readonly CotHienThi[] cot =
        {
            new CotHienThi("Mã loại", "MaLoai", 90),
            new CotHienThi("Tên loại phòng", "TenLoai", 200),
            new CotHienThi("Đơn giá ngày", "DonGiaNgay", 140),
            new CotHienThi("Đơn giá giờ", "DonGiaGio", 140),
            new CotHienThi("Số lượng đang có", "SoLuongDangCo", 140),
            new CotHienThi("Tổng số phòng", "TongSoPhong", 130)
        };

        public frmLoaiPhong()
        {
            InitializeComponent();
            Load += frmLoaiPhong_Load;
            txtTimNhanh.TextChanged += (s, e) => HienThi(bus.TimNhanh(txtTimNhanh.Text));
            btnTimNangCao.Click += (s, e) => BatTatTimNangCao();
            lvLoaiPhong.SelectedIndexChanged += lvLoaiPhong_SelectedIndexChanged;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnLuu.Click += btnLuu_Click;
            btnHuy.Click += btnHuy_Click;
            ucTim.Dock = DockStyle.Top;
            Controls.Add(ucTim);
            ucTim.BringToFront();
        }

        private void frmLoaiPhong_Load(object sender, EventArgs e)
        {
            LvHelper.TaoCot(lvLoaiPhong, cot);
            ucTim.DatTruong(cot);
            ucTim.Visible = false;
            ucTim.Dong += () => { ucTim.Visible = false; SapXepTimNangCao(); };
            ucTim.SizeChanged += (s2, e2) => { if (ucTim.Visible) SapXepTimNangCao(); };
            ucTim.TimKiem += dk => HienThi(bus.TimNangCao(dk));   // BUS, trên ArrayList
            ucTim.XoaLoc += () => HienThi(bus.DanhSach);

            bus.TaiDuLieu();   // lấy số liệu mới nhất (số phòng có thể vừa đổi ở form Phòng)
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
                    ucTim.Location = new Point(lvLoaiPhong.Left, lvLoaiPhong.Top - daDay);
                    ucTim.Width = lvLoaiPhong.Width;
                }
                int delta = can - daDay;
                lvLoaiPhong.Top += delta;
                lvLoaiPhong.Height -= delta;
                daDay = can;
            }
            finally
            {
                dangSapXep = false;
            }
        }

        private void HienThi(ArrayList ds)
        {
            LvHelper.Do(lvLoaiPhong, ds, cot);
        }

        private void DatCheDo(bool nhap)
        {
            txtTenLoai.Enabled = nhap;
            txtDonGiaNgay.Enabled = nhap;
            txtDonGiaGio.Enabled = nhap;
            btnThem.Enabled = !nhap;
            btnSua.Enabled = !nhap;
            btnXoa.Enabled = !nhap;
            btnLuu.Enabled = nhap;
            btnHuy.Enabled = nhap;
            lvLoaiPhong.Enabled = !nhap;
            txtTimNhanh.Enabled = !nhap;
            btnTimNangCao.Enabled = !nhap;
        }

        private void LamTrongO()
        {
            txtMaLoai.Text = bus.SinhMa();
            txtTenLoai.Clear();
            txtDonGiaNgay.Clear();
            txtDonGiaGio.Clear();
            txtSoLuongDangCo.Text = "0";
            txtTongSoPhong.Text = "0";
        }

        private LoaiPhong DongDangChon()
        {
            if (lvLoaiPhong.SelectedItems.Count == 0) return null;
            return (LoaiPhong)lvLoaiPhong.SelectedItems[0].Tag;
        }

        private void lvLoaiPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoaiPhong lp = DongDangChon();
            if (lp == null) return;
            txtMaLoai.Text = lp.MaLoai;
            txtTenLoai.Text = lp.TenLoai;
            txtDonGiaNgay.Text = lp.DonGiaNgay.ToString("0");
            txtDonGiaGio.Text = lp.DonGiaGio.ToString("0");
            txtSoLuongDangCo.Text = lp.SoLuongDangCo.ToString();
            txtTongSoPhong.Text = lp.TongSoPhong.ToString();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            dangThem = true;
            LamTrongO();
            DatCheDo(true);
            txtTenLoai.Focus();
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
            txtTenLoai.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            decimal ngay, gio;
            if (!LoaiPhongBUS.DocTien(txtDonGiaNgay.Text, out ngay)) ngay = -1;   // BUS sẽ báo lỗi
            if (!LoaiPhongBUS.DocTien(txtDonGiaGio.Text, out gio)) gio = -1;

            var lp = new LoaiPhong
            {
                MaLoai = txtMaLoai.Text,
                TenLoai = txtTenLoai.Text,
                DonGiaNgay = ngay,
                DonGiaGio = gio
            };
            string loi;
            bool ok = dangThem ? bus.Them(lp, out loi) : bus.Sua(lp, out loi);
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
            if (DongDangChon() != null) lvLoaiPhong_SelectedIndexChanged(null, null);
            else LamTrongO();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            LoaiPhong lp = DongDangChon();
            if (lp == null)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có chắc muốn xóa loại phòng " + lp.TenLoai + "?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            string loi;
            if (!bus.Xoa(lp.MaLoai, out loi))
            {
                MessageBox.Show(loi, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            HienThi(bus.DanhSach);
            LamTrongO();
        }
    }
}
