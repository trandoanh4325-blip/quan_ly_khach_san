using System;
using System.Collections;
using System.Windows.Forms;
using QLKS.BUS;
using QLKS.Common;
using QLKS.DTO;

namespace QLKS.GUI
{
    public partial class frmNhanVien : Form
    {
        private readonly NhanVienBUS bus = new NhanVienBUS();
        private bool dangThem;
        private readonly ucTimKiemNangCao ucTim = new ucTimKiemNangCao();

        private readonly CotHienThi[] cot =
        {
            new CotHienThi("Mã NV", "MaNV", 90),
            new CotHienThi("Họ và tên", "HoTen", 240),
            new CotHienThi("Chức vụ", "ChucVu", 160),
            new CotHienThi("Số điện thoại", "SoDienThoai", 150)
        };

        public frmNhanVien()
        {
            InitializeComponent();
            Load += frmNhanVien_Load;
            txtTimNhanh.TextChanged += (s, e) => HienThi(bus.TimNhanh(txtTimNhanh.Text));
            btnTimNangCao.Click += (s, e) => ucTim.Visible = !ucTim.Visible;
            lvNhanVien.SelectedIndexChanged += lvNhanVien_SelectedIndexChanged;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnLuu.Click += btnLuu_Click;
            btnHuy.Click += btnHuy_Click;
            ucTim.Dock = DockStyle.Top;
            Controls.Add(ucTim);
            ucTim.BringToFront();
        }

        private void frmNhanVien_Load(object sender, EventArgs e)
        {
            LvHelper.TaoCot(lvNhanVien, cot);
            ucTim.DatTruong(cot);
            ucTim.Visible = false;
            ucTim.TimKiem += dk => HienThi(bus.TimNangCao(dk));//BUS, trên ArrayList
            ucTim.XoaLoc += () => HienThi(bus.DanhSach);

            cboChucVu.Items.AddRange(new object[] { "Quản lý", "Lễ tân", "Kế toán", "Thủ kho", "Buồng phòng" });

            HienThi(bus.DanhSach);
            LamTrongO();
            DatCheDo(false);
        }

        private void HienThi(ArrayList ds)
        {
            LvHelper.Do(lvNhanVien, ds, cot);
        }
        private void DatCheDo(bool nhap)
        {
            txtHoTen.Enabled = nhap;
            cboChucVu.Enabled = nhap;
            txtSDT.Enabled = nhap;
            btnThem.Enabled = !nhap;
            btnSua.Enabled = !nhap;
            btnXoa.Enabled = !nhap;
            btnLuu.Enabled = nhap;
            btnHuy.Enabled = nhap;
            lvNhanVien.Enabled = !nhap;
            txtTimNhanh.Enabled = !nhap;
            btnTimNangCao.Enabled = !nhap;
        }

        private void LamTrongO()
        {
            txtMaNV.Text = bus.SinhMa();     
            txtHoTen.Clear();
            cboChucVu.Text = "";
            txtSDT.Clear();
        }

        private NhanVien DongDangChon()
        {
            if (lvNhanVien.SelectedItems.Count == 0) return null;
            return (NhanVien)lvNhanVien.SelectedItems[0].Tag;
        }

        private void lvNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            NhanVien nv = DongDangChon();
            if (nv == null) return;
            txtMaNV.Text = nv.MaNV;
            txtHoTen.Text = nv.HoTen;
            cboChucVu.Text = nv.ChucVu;
            txtSDT.Text = nv.SoDienThoai;
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
            var nv = new NhanVien
            {
                MaNV = txtMaNV.Text,
                HoTen = txtHoTen.Text,
                ChucVu = cboChucVu.Text,
                SoDienThoai = txtSDT.Text.Trim()
            };
            string loi;
            bool ok = dangThem ? bus.Them(nv, out loi) : bus.Sua(nv, out loi);
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
            if (DongDangChon() != null) lvNhanVien_SelectedIndexChanged(null, null);
            else LamTrongO();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            NhanVien nv = DongDangChon();
            if (nv == null)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có chắc muốn xóa nhân viên " + nv.HoTen + "?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            string loi;
            if (!bus.Xoa(nv.MaNV, out loi))
            {
                MessageBox.Show(loi, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            HienThi(bus.DanhSach);
            LamTrongO();
        }
    }
}
