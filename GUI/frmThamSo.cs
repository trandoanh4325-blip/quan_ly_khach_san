// =====================================================================
// CODE-BEHIND cho form frmThamSo (vẽ bằng Designer).
// Control cần có (đặt đúng tên):
//   lvThamSo, ucTim, txtTimNhanh, btnTimNangCao,
//   txtTenThamSo, txtGiaTri, txtMoTa,
//   btnThem, btnSua, btnXoa, btnLuu, btnHuy
// KHÔNG double-click vào control: các sự kiện đã được gắn sẵn trong hàm khởi tạo.
// =====================================================================
using System;
using System.Collections;
using System.Windows.Forms;
using QLKS.BUS;
using QLKS.Common;
using QLKS.DTO;

namespace QLKS.GUI
{
    public partial class frmThamSo : Form
    {
        private readonly ThamSoBUS bus = new ThamSoBUS();
        private bool dangThem;
        private readonly ucTimKiemNangCao ucTim = new ucTimKiemNangCao();
        private readonly CotHienThi[] cot =
        {
            new CotHienThi("Tên tham số", "TenThamSo", 200),
            new CotHienThi("Giá trị", "GiaTri", 220),
            new CotHienThi("Mô tả", "MoTa", 420)
        };

        public frmThamSo()
        {
            InitializeComponent();
            Load += frmThamSo_Load;
            txtTimNhanh.TextChanged += (s, e) => HienThi(bus.TimNhanh(txtTimNhanh.Text));
            btnTimNangCao.Click += (s, e) => ucTim.Visible = !ucTim.Visible;
            lvThamSo.SelectedIndexChanged += lvThamSo_SelectedIndexChanged;
            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnXoa.Click += btnXoa_Click;
            btnLuu.Click += btnLuu_Click;
            btnHuy.Click += btnHuy_Click;
            ucTim.Dock = DockStyle.Top;
            Controls.Add(ucTim);
            ucTim.BringToFront();
        }

        private void frmThamSo_Load(object sender, EventArgs e)
        {
            LvHelper.TaoCot(lvThamSo, cot);
            ucTim.DatTruong(cot);
            ucTim.Visible = false;
            ucTim.TimKiem += dk => HienThi(bus.TimNangCao(dk));
            ucTim.XoaLoc += () => HienThi(bus.DanhSach);

            HienThi(bus.DanhSach);
            LamTrongO();
            DatCheDo(false);
        }

        private void HienThi(ArrayList ds)
        {
            LvHelper.Do(lvThamSo, ds, cot);
        }

        // Tên tham số là khóa: chỉ cho nhập khi THÊM MỚI, khi sửa thì khóa lại
        private void DatCheDo(bool nhap)
        {
            txtTenThamSo.Enabled = nhap && dangThem;
            txtGiaTri.Enabled = nhap;
            txtMoTa.Enabled = nhap;
            btnThem.Enabled = !nhap;
            btnSua.Enabled = !nhap;
            btnXoa.Enabled = !nhap;
            btnLuu.Enabled = nhap;
            btnHuy.Enabled = nhap;
            lvThamSo.Enabled = !nhap;
            txtTimNhanh.Enabled = !nhap;
            btnTimNangCao.Enabled = !nhap;
        }

        private void LamTrongO()
        {
            txtTenThamSo.Clear();
            txtGiaTri.Clear();
            txtMoTa.Clear();
        }

        private ThamSo DongDangChon()
        {
            if (lvThamSo.SelectedItems.Count == 0) return null;
            return (ThamSo)lvThamSo.SelectedItems[0].Tag;
        }

        private void lvThamSo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ThamSo t = DongDangChon();
            if (t == null) return;
            txtTenThamSo.Text = t.TenThamSo;
            txtGiaTri.Text = t.GiaTri;
            txtMoTa.Text = t.MoTa;
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            dangThem = true;
            LamTrongO();
            DatCheDo(true);
            txtTenThamSo.Focus();
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
            txtGiaTri.Focus();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            var t = new ThamSo
            {
                TenThamSo = txtTenThamSo.Text.Trim(),
                GiaTri = txtGiaTri.Text.Trim(),
                MoTa = txtMoTa.Text.Trim()
            };
            string loi;
            bool ok = dangThem ? bus.Them(t, out loi) : bus.Sua(t, out loi);
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
            if (DongDangChon() != null) lvThamSo_SelectedIndexChanged(null, null);
            else LamTrongO();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            ThamSo t = DongDangChon();
            if (t == null)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có chắc muốn xóa tham số " + t.TenThamSo + "?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            string loi;
            if (!bus.Xoa(t.TenThamSo, out loi))
            {
                MessageBox.Show(loi, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            HienThi(bus.DanhSach);
            LamTrongO();
        }
    }
}
