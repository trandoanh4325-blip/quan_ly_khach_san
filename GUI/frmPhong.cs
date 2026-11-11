using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using QLKS.BUS;
using QLKS.Common;
using QLKS.DTO;

namespace QLKS.GUI
{
    public partial class frmPhong : Form
    {
        private readonly PhongBUS bus = new PhongBUS();
        private readonly LoaiPhongBUS loaiBus = new LoaiPhongBUS();
        private LoaiPhong loaiChon;          // loại phòng đang chọn bằng nút "..."
        private bool dangThem;
        private int daDay;
        private bool dangSapXep;
        private readonly ucTimKiemNangCao ucTim = new ucTimKiemNangCao();

        private readonly CotHienThi[] cot =
        {
            new CotHienThi("Mã phòng", "MaPhong", 100),
            new CotHienThi("Tên phòng", "TenPhong", 180),
            new CotHienThi("Loại phòng", "TenLoai", 160),
            new CotHienThi("Số người tối đa", "SoNguoiToiDa", 130),
            new CotHienThi("Trạng thái", "TrangThai", 120)
        };

        // cột của bảng chọn khi bấm nút "..."
        private readonly CotHienThi[] cotLoai =
        {
            new CotHienThi("Mã loại", "MaLoai", 90),
            new CotHienThi("Tên loại phòng", "TenLoai", 200),
            new CotHienThi("Đơn giá ngày", "DonGiaNgay", 140),
            new CotHienThi("Đơn giá giờ", "DonGiaGio", 140),
            new CotHienThi("Số lượng đang có", "SoLuongDangCo", 140)
        };

        public frmPhong()
        {
            InitializeComponent();
            Load += frmPhong_Load;
            txtTimNhanh.TextChanged += (s, e) => HienThi(bus.TimNhanh(txtTimNhanh.Text));
            btnTimNangCao.Click += (s, e) => BatTatTimNangCao();
            lvPhong.SelectedIndexChanged += lvPhong_SelectedIndexChanged;
            btnChonLoai.Click += btnChonLoai_Click;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnLuu.Click += btnLuu_Click;
            btnHuy.Click += btnHuy_Click;
            ucTim.Dock = DockStyle.Top;
            Controls.Add(ucTim);
            ucTim.BringToFront();
        }

        private void frmPhong_Load(object sender, EventArgs e)
        {
            LvHelper.TaoCot(lvPhong, cot);
            ucTim.DatTruong(cot);
            ucTim.Visible = false;
            ucTim.Dong += () => { ucTim.Visible = false; SapXepTimNangCao(); };
            ucTim.SizeChanged += (s2, e2) => { if (ucTim.Visible) SapXepTimNangCao(); };
            ucTim.TimKiem += dk => HienThi(bus.TimNangCao(dk));   // BUS, trên ArrayList
            ucTim.XoaLoc += () => HienThi(bus.DanhSach);

            foreach (int n in PhongBUS.SoNguoiChoPhep) cboSoNguoi.Items.Add(n.ToString());

            bus.TaiDuLieu();   // lấy trạng thái phòng mới nhất
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
                    ucTim.Location = new Point(lvPhong.Left, lvPhong.Top - daDay);
                    ucTim.Width = lvPhong.Width;
                }
                int delta = can - daDay;
                lvPhong.Top += delta;
                lvPhong.Height -= delta;
                daDay = can;
            }
            finally
            {
                dangSapXep = false;
            }
        }

        private void HienThi(ArrayList ds)
        {
            LvHelper.Do(lvPhong, ds, cot);
        }

        private void DatCheDo(bool nhap)
        {
            txtMaPhong.Enabled = nhap && dangThem;   // mã chỉ nhập được khi thêm mới
            txtTenPhong.Enabled = nhap;
            btnChonLoai.Enabled = nhap;
            cboSoNguoi.Enabled = nhap;
            btnThem.Enabled = !nhap;
            btnSua.Enabled = !nhap;
            btnXoa.Enabled = !nhap;
            btnLuu.Enabled = nhap;
            btnHuy.Enabled = nhap;
            lvPhong.Enabled = !nhap;
            txtTimNhanh.Enabled = !nhap;
            btnTimNangCao.Enabled = !nhap;
        }

        private void LamTrongO()
        {
            txtMaPhong.Text = bus.SinhMa();     // gợi ý mã, có thể sửa khi thêm (vd P501)
            txtTenPhong.Clear();
            loaiChon = null;
            txtLoaiPhong.Clear();
            cboSoNguoi.SelectedIndex = 0;
            txtTrangThai.Text = "Trống";
        }

        private Phong DongDangChon()
        {
            if (lvPhong.SelectedItems.Count == 0) return null;
            return (Phong)lvPhong.SelectedItems[0].Tag;
        }

        private void lvPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            Phong p = DongDangChon();
            if (p == null) return;
            txtMaPhong.Text = p.MaPhong;
            txtTenPhong.Text = p.TenPhong;
            loaiChon = loaiBus.TimTheoMa(p.MaLoai);
            txtLoaiPhong.Text = p.TenLoai;
            cboSoNguoi.Text = p.SoNguoiToiDa.ToString();
            txtTrangThai.Text = p.TrangThai;
        }

        // Nút "...": mở bảng tìm kiếm -> chọn -> quay lại, điền thông tin (biến static SelectionContext)
        private void btnChonLoai_Click(object sender, EventArgs e)
        {
            loaiBus.TaiDuLieu();
            LoaiPhong lp = frmChon.Chon<LoaiPhong>("Chọn loại phòng", loaiBus.DanhSach, cotLoai);
            if (lp == null) return;
            loaiChon = lp;
            txtLoaiPhong.Text = lp.TenLoai;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            dangThem = true;
            LamTrongO();
            DatCheDo(true);
            txtMaPhong.Focus();
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
            txtTenPhong.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            int soNguoi;
            if (!int.TryParse(cboSoNguoi.Text, out soNguoi)) soNguoi = 0;

            var p = new Phong
            {
                MaPhong = txtMaPhong.Text,
                TenPhong = txtTenPhong.Text,
                MaLoai = loaiChon == null ? "" : loaiChon.MaLoai,
                SoNguoiToiDa = soNguoi
            };
            string loi;
            bool ok = dangThem ? bus.Them(p, out loi) : bus.Sua(p, out loi);
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
            if (DongDangChon() != null) lvPhong_SelectedIndexChanged(null, null);
            else LamTrongO();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            Phong p = DongDangChon();
            if (p == null)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có chắc muốn xóa " + p.TenPhong + "?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            string loi;
            if (!bus.Xoa(p.MaPhong, out loi))
            {
                MessageBox.Show(loi, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            HienThi(bus.DanhSach);
            LamTrongO();
        }
    }
}
