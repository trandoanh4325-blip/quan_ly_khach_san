using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using QLKS.BUS;
using QLKS.Common;
using QLKS.DTO;

namespace QLKS.GUI
{
    public partial class frmSanPham : Form
    {
        private readonly SanPhamBUS bus = new SanPhamBUS();
        private readonly ucTimKiemNangCao ucTim = new ucTimKiemNangCao();
        private bool dangThem;
        private bool dangNhap;

        private readonly CotHienThi[] cot =
        {
            new CotHienThi("Mã SP", "MaSP", 90),
            new CotHienThi("Tên sản phẩm", "TenSP", 240),
            new CotHienThi("Mã loại", "MaLoaiSP", 100),
            new CotHienThi("Đơn vị tính", "DonViTinh", 110),
            new CotHienThi("Giá bán / đền bù", "GiaBan", 150),
            new CotHienThi("Số lượng tồn", "SoLuongTon", 110)
        };

        public frmSanPham()
        {
            InitializeComponent();
            Load += frmSanPham_Load;
            btnThem.Click += (s, e) => BatDauThem();
            btnSua.Click += (s, e) => BatDauSua();
            btnXoa.Click += (s, e) => Xoa();
            btnExcel.Click += (s, e) => ExcelExport.XuatBang(this, "san_pham", "san_pham", "SanPham");
            btnLuu.Click += (s, e) => Luu();
            btnHuy.Click += (s, e) => Huy();
            btnTaiLai.Click += (s, e) => TaiLai();
            txtTimNhanh.TextChanged += (s, e) => { if (!dangNhap) HienThi(bus.TimKiem(txtTimNhanh.Text)); };
            lvSanPham.SelectedIndexChanged += (s, e) => ChonDong();
            btnTimNangCao.Click += (s, e) => { ucTim.Visible = !ucTim.Visible; DongBoChieuCaoTim(); };
            pnlTimNangCao.Controls.Add(ucTim);
            ucTim.Dock = DockStyle.Fill;
            ucTim.Visible = false;
            ucTim.SizeChanged += (s, e) => { if (ucTim.Visible) DongBoChieuCaoTim(); };
            ucTim.Dong += () => { ucTim.Visible = false; DongBoChieuCaoTim(); };
            ucTim.TimKiem += dk => HienThi(bus.TimNangCao(dk));
            ucTim.XoaLoc += () => { txtTimNhanh.Clear(); HienThi(bus.DanhSach); };
        }

        private void frmSanPham_Load(object sender, EventArgs e)
        {
            try
            {
                LvHelper.TaoCot(lvSanPham, cot);
                ucTim.DatTruong(cot);
                DataTable loai = bus.LayLoaiSanPham();
                cboLoaiSP.DisplayMember = "ten_loai_sp";
                cboLoaiSP.ValueMember = "ma_loai_sp";
                cboLoaiSP.DataSource = loai;
                HienThi(bus.DanhSach);
                DatCheDo(false);
                LamTrongO();
            }
            catch (Exception ex) { BaoLoi("Không tải được dữ liệu: " + ex.Message); }
        }

        private void DongBoChieuCaoTim()
        {
            bangChinh.RowStyles[4].Height = ucTim.Visible ? Math.Max(ucTim.Height, 90) : 0;
        }

        private void HienThi(ArrayList ds) => LvHelper.Do(lvSanPham, ds, cot);

        private SanPham DongDangChon()
        {
            if (lvSanPham.SelectedItems.Count == 0) return null;
            return lvSanPham.SelectedItems[0].Tag as SanPham;
        }

        private void ChonDong()
        {
            if (dangNhap) return;
            SanPham sp = DongDangChon();
            if (sp == null) return;
            txtMaSP.Text = sp.MaSP;
            txtTenSP.Text = sp.TenSP;
            cboLoaiSP.SelectedValue = sp.MaLoaiSP;
            txtDonViTinh.Text = sp.DonViTinh;
            nudGiaBan.Value = Math.Min(nudGiaBan.Maximum, Math.Max(nudGiaBan.Minimum, sp.GiaBan));
            txtSoLuongTon.Text = sp.SoLuongTon.ToString();
        }

        private void LamTrongO()
        {
            txtMaSP.Clear();
            txtTenSP.Clear();
            txtDonViTinh.Clear();
            txtSoLuongTon.Text = "0";
            nudGiaBan.Value = 0;
            if (cboLoaiSP.Items.Count > 0) cboLoaiSP.SelectedIndex = 0;
        }

        private void DatCheDo(bool nhap)
        {
            dangNhap = nhap;
            txtTenSP.ReadOnly = !nhap;
            txtDonViTinh.ReadOnly = !nhap;
            cboLoaiSP.Enabled = nhap;
            nudGiaBan.Enabled = nhap;
            txtMaSP.ReadOnly = true;       // mã tự sinh
            txtSoLuongTon.ReadOnly = true; // tồn thay đổi qua nhập hàng / phụ thu
            btnThem.Enabled = !nhap;
            btnSua.Enabled = !nhap;
            btnXoa.Enabled = !nhap;
            btnLuu.Enabled = nhap;
            btnHuy.Enabled = nhap;
            btnTaiLai.Enabled = !nhap;
            lvSanPham.Enabled = !nhap;
            txtTimNhanh.Enabled = !nhap;
            btnTimNangCao.Enabled = !nhap;
        }

        private void BatDauThem()
        {
            try
            {
                LamTrongO();
                txtMaSP.Text = bus.SinhMa();
                dangThem = true;
                DatCheDo(true);
                txtTenSP.Focus();
            }
            catch (Exception ex) { BaoLoi(ex.Message); }
        }

        private void BatDauSua()
        {
            if (DongDangChon() == null) { ThongBao("Hãy chọn sản phẩm cần sửa."); return; }
            dangThem = false;
            DatCheDo(true);
            txtTenSP.Focus();
        }

        private void Luu()
        {
            if (cboLoaiSP.SelectedValue == null) { BaoLoi("Hãy chọn loại sản phẩm."); return; }
            var sp = new SanPham
            {
                MaSP = txtMaSP.Text,
                TenSP = txtTenSP.Text,
                MaLoaiSP = Convert.ToString(cboLoaiSP.SelectedValue),
                DonViTinh = txtDonViTinh.Text,
                GiaBan = nudGiaBan.Value
            };
            string loi;
            bool ok = dangThem ? bus.Them(sp, out loi) : bus.Sua(sp, out loi);
            if (!ok) { BaoLoi(loi); return; }
            DatCheDo(false);
            txtTimNhanh.Clear();
            HienThi(bus.DanhSach);
            ChonTheoMa(sp.MaSP);
            ThongBao("Lưu sản phẩm thành công.");
        }

        private void Huy()
        {
            DatCheDo(false);
            if (DongDangChon() != null) ChonDong();
            else LamTrongO();
        }

        private void Xoa()
        {
            SanPham sp = DongDangChon();
            if (sp == null) { ThongBao("Hãy chọn sản phẩm cần xóa."); return; }
            if (MessageBox.Show("Xóa sản phẩm " + sp.TenSP + "?", "Xác nhận",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            string loi;
            if (!bus.Xoa(sp.MaSP, out loi)) { BaoLoi(loi); return; }
            HienThi(bus.DanhSach);
            LamTrongO();
        }

        private void TaiLai()
        {
            try { bus.TaiDuLieu(); txtTimNhanh.Clear(); HienThi(bus.DanhSach); LamTrongO(); }
            catch (Exception ex) { BaoLoi(ex.Message); }
        }

        private void ChonTheoMa(string ma)
        {
            foreach (ListViewItem item in lvSanPham.Items)
                if ((item.Tag as SanPham)?.MaSP == ma)
                {
                    item.Selected = true;
                    item.EnsureVisible();
                    break;
                }
        }

        private void ThongBao(string text) => MessageBox.Show(text, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        private void BaoLoi(string text) => MessageBox.Show(text, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
