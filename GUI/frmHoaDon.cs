using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using QLKS.BUS;
using QLKS.Common;
using QLKS.DTO;

namespace QLKS.GUI
{
    public class frmHoaDon : Form
    {
        private readonly HoaDonBUS bus = new HoaDonBUS();
        private readonly TabControl tabs = new TabControl { Dock = DockStyle.Fill };
        private readonly ListView lichSu = DanhSach();
        private readonly ListView phong = DanhSach(), dichVu = DanhSach(), phuThu = DanhSach();
        private readonly TextBox phieu = new TextBox { ReadOnly = true, Width = 220 };
        private readonly TextBox nv = new TextBox { ReadOnly = true, Width = 200 };
        private readonly TextBox km = new TextBox { ReadOnly = true, Width = 200, Text = "Không áp dụng" };
        private readonly DateTimePicker ngay = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm:ss", Width = 190 };
        private readonly Label tong = new Label { AutoSize = true, Padding = new Padding(8), Font = new Font("Segoe UI", 11, FontStyle.Bold) };
        private readonly Button thanhToan = new Button { Text = "Thanh toán", AutoSize = true, Height = 36 };
        private readonly NumericUpDown sl = new NumericUpDown { Minimum = 1, Maximum = 1000000, Value = 1, Width = 85 };
        private readonly TextBox ghiChu = new TextBox { Width = 200, MaxLength = 255, PlaceholderText = "Ghi chú phụ thu" };
        private HoaDon nhap = new HoaDon(), daXem;
        private readonly CotHienThi[] cotPhong = { new CotHienThi("Phòng", "MaPhong", 85), new CotHienThi("Tên phòng", "TenPhong", 150),
            new CotHienThi("Đơn giá", "GiaPhong", 120), new CotHienThi("Ngày", "SoNgayO", 65), new CotHienThi("Giờ", "SoGioO", 65), new CotHienThi("Thành tiền", "ThanhTien", 140) };
        private readonly CotHienThi[] cotDV = { new CotHienThi("Phòng", "MaPhong", 85), new CotHienThi("Dịch vụ", "TenDV", 180),
            new CotHienThi("Số lượng", "SoLuongSuDung", 85), new CotHienThi("Đơn giá", "DonGiaSuDung", 120), new CotHienThi("Thành tiền", "ThanhTien", 140) };
        private readonly CotHienThi[] cotPT = { new CotHienThi("Phòng", "MaPhong", 85), new CotHienThi("Phụ thu", "TenPhuThu", 180),
            new CotHienThi("Số lượng", "SoLuong", 85), new CotHienThi("Đơn giá", "DonGia", 120), new CotHienThi("Thành tiền", "ThanhTien", 140), new CotHienThi("Ghi chú", "GhiChu", 180) };
        private readonly CotHienThi[] cotHD = { new CotHienThi("Hóa đơn", "MaHoaDon", 100), new CotHienThi("Phiếu đặt", "MaPhieu", 100),
            new CotHienThi("Khách hàng", "TenKH", 190), new CotHienThi("Thanh toán lúc", "NgayThanhToan", 160), new CotHienThi("Tổng hóa đơn", "TongTienHoaDon", 150),
            new CotHienThi("Đã cọc", "TienDaCoc", 120), new CotHienThi("Còn thu / hoàn (-)", "TienCanThanhToan", 160) };

        public frmHoaDon()
        {
            Text = "Hóa đơn và thanh toán"; Font = new Font("Segoe UI", 10); Size = new Size(1100, 650);
            var lap = new TabPage("Lập hóa đơn / checkout"); var xem = new TabPage("Hóa đơn đã thanh toán");
            tabs.TabPages.AddRange(new[] { lap, xem }); Controls.Add(tabs);
            var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 5, Padding = new Padding(8) };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < 5; i++) root.RowStyles.Add(new RowStyle(i == 2 ? SizeType.Percent : SizeType.AutoSize, i == 2 ? 100 : 0));
            lap.Controls.Add(root);
            var bar = Hang(); bar.Controls.Add(phieu); Nut(bar, "Chọn phiếu...", ChonPhieu); bar.Controls.Add(nv); Nut(bar, "Nhân viên...", ChonNV); bar.Controls.Add(ngay);
            root.Controls.Add(bar, 0, 0);
            var khuyenMai = Hang(); khuyenMai.Controls.Add(km); Nut(khuyenMai, "Khuyến mãi...", ChonKM); Nut(khuyenMai, "Bỏ khuyến mãi", () => { nhap.MaKM = null; km.Text = "Không áp dụng"; VoHieu(); });
            khuyenMai.Controls.Add(new Label { Text = "Số lượng:", AutoSize = true, Padding = new Padding(0, 7, 0, 0) }); khuyenMai.Controls.Add(sl); khuyenMai.Controls.Add(ghiChu);
            Nut(khuyenMai, "Thêm dịch vụ...", ThemDV); Nut(khuyenMai, "Thêm phụ thu...", ThemPT); root.Controls.Add(khuyenMai, 0, 1);
            var chiTiet = new TabControl { Dock = DockStyle.Fill };
            foreach (var pair in new[] { ("Phòng (chọn phòng trước khi thêm khoản thu)", phong), ("Dịch vụ", dichVu), ("Phụ thu", phuThu) })
            { var page = new TabPage(pair.Item1); page.Controls.Add(pair.Item2); chiTiet.TabPages.Add(page); }
            root.Controls.Add(chiTiet, 0, 2);
            var actions = Hang(); Nut(actions, "Xóa dịch vụ đã chọn", () => { if (dichVu.SelectedItems.Count > 0) { nhap.DichVu.Remove((ChiTietSuDungDichVu)dichVu.SelectedItems[0].Tag); VoHieu(); HienNhap(); } });
            Nut(actions, "Xóa phụ thu đã chọn", () => { if (phuThu.SelectedItems.Count > 0) { nhap.PhuThu.Remove((ChiTietPhuThu)phuThu.SelectedItems[0].Tag); VoHieu(); HienNhap(); } });
            Nut(actions, "Tính lại / xem trước", Tinh); actions.Controls.Add(thanhToan); root.Controls.Add(actions, 0, 3); root.Controls.Add(tong, 0, 4);
            thanhToan.Click += (s, e) => AnToan(ThanhToan); ngay.ValueChanged += (s, e) => VoHieu(); thanhToan.Enabled = false;
            var history = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
            history.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            history.RowStyles.Add(new RowStyle(SizeType.AutoSize)); history.RowStyles.Add(new RowStyle(SizeType.AutoSize)); history.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            var filter = Hang(); var tim = new TextBox { Width = 260, PlaceholderText = "Tìm hóa đơn..." }; filter.Controls.Add(tim);
            Nut(filter, "Tải lại", TaiLichSu); Nut(filter, "Xem chi tiết", XemChiTiet);
            var nangCao = new ucTimKiemNangCao { Dock = DockStyle.Fill, Visible = false }; nangCao.DatTruong(cotHD);
            Nut(filter, "Tìm nâng cao", () => nangCao.Visible = !nangCao.Visible);
            nangCao.Dong += () => nangCao.Visible = false;
            nangCao.TimKiem += dk => LvHelper.Do(lichSu, bus.TimNangCao(dk), cotHD);
            nangCao.XoaLoc += () => LvHelper.Do(lichSu, bus.DanhSach, cotHD);
            history.Controls.Add(filter, 0, 0); history.Controls.Add(nangCao, 0, 1); history.Controls.Add(lichSu, 0, 2); xem.Controls.Add(history);
            tim.TextChanged += (s, e) => AnToan(() => LvHelper.Do(lichSu, bus.TimNhanh(tim.Text), cotHD));
            lichSu.DoubleClick += (s, e) => AnToan(XemChiTiet);
            LvHelper.TaoCot(phong, cotPhong); LvHelper.TaoCot(dichVu, cotDV); LvHelper.TaoCot(phuThu, cotPT); LvHelper.TaoCot(lichSu, cotHD);
            Load += (s, e) => AnToan(TaiLichSu);
            FormClosing += (s, e) => { if (CoNhap() && !XacNhanBo()) e.Cancel = true; };
        }

        private static ListView DanhSach() { return new ListView { Dock = DockStyle.Fill, View = View.Details, FullRowSelect = true, MultiSelect = false, HideSelection = false, GridLines = true }; }
        private static FlowLayoutPanel Hang() { return new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true }; }
        private void Nut(FlowLayoutPanel p, string text, Action a) { var b = new Button { Text = text, AutoSize = true, Height = 34 }; b.Click += (s, e) => AnToan(a); p.Controls.Add(b); }
        private void Bao(string s) { MessageBox.Show(this, s, "Hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void AnToan(Action a) { try { a(); } catch (Exception ex) { Bao(ex.Message); } }
        private void VoHieu() { daXem = null; thanhToan.Enabled = false; tong.Text = "Nhấn Tính lại / xem trước để kiểm tra số tiền. Các khoản thu chưa được lưu."; }
        private bool CoNhap() { return nhap.DichVu.Count > 0 || nhap.PhuThu.Count > 0; }
        private bool XacNhanBo() { return MessageBox.Show(this, "Các khoản dịch vụ/phụ thu chưa thanh toán sẽ không được lưu. Tiếp tục?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes; }

        private void ChonPhieu()
        {
            if (CoNhap() && !XacNhanBo()) return;
            var p = frmChon.Chon<PhieuDatPhong>("Chọn phiếu chưa thanh toán", bus.PhieuChuaThanhToan(), new CotHienThi("Mã phiếu", "MaPhieu", 110), new CotHienThi("Khách hàng", "TenKH", 200), new CotHienThi("Nhận phòng", "NgayCheckin", 170));
            if (p == null) return;
            nhap = new HoaDon { MaPhieu = p.MaPhieu, MaNV = p.MaNV }; phieu.Text = p.MaPhieu + " - " + p.TenKH; nv.Text = p.MaNV + " - " + p.TenNV; km.Text = "Không áp dụng"; ngay.Value = DateTime.Now;
            foreach (ChiTietDatPhong ct in new ChiTietDatPhongBUS().TheoPhieu(p.MaPhieu)) nhap.Phong.Add(ct);
            VoHieu(); HienNhap();
        }
        private void ChonNV()
        {
            var n = frmChon.Chon<NhanVien>("Nhân viên thanh toán", new NhanVienBUS().DanhSach, new CotHienThi("Mã", "MaNV", 100), new CotHienThi("Họ tên", "HoTen", 220));
            if (n == null) return; nhap.MaNV = n.MaNV; nv.Text = n.MaNV + " - " + n.HoTen; VoHieu();
        }
        private void ChonKM()
        {
            var k = frmChon.Chon<KhuyenMaiThanhToan>("Chọn khuyến mãi (điều kiện được kiểm tra khi tính tiền)", bus.KhuyenMai(), new CotHienThi("Mã", "MaKM", 85), new CotHienThi("Khuyến mãi", "TenKM", 250), new CotHienThi("Loại giảm", "LoaiGiam", 120), new CotHienThi("Giá trị", "GiaTriGiam", 100), new CotHienThi("Từ ngày", "NgayBatDau", 100), new CotHienThi("Đến ngày", "NgayKetThuc", 100));
            if (k == null) return; nhap.MaKM = k.MaKM; km.Text = k.TenKM; VoHieu();
        }
        private string PhongChon()
        {
            if (phong.SelectedItems.Count == 0) throw new InvalidOperationException("Chọn một phòng trong tab Phòng trước khi thêm khoản thu.");
            return ((ChiTietDatPhong)phong.SelectedItems[0].Tag).MaPhong;
        }
        private void ThemDV()
        {
            string ma = PhongChon(); var d = frmChon.Chon<DichVu>("Chọn dịch vụ", new DichVuBUS().DanhSach, new CotHienThi("Mã", "MaDV", 100), new CotHienThi("Dịch vụ", "TenDV", 240), new CotHienThi("Giá", "GiaTien", 130));
            if (d == null) return;
            var cu = nhap.DichVu.Find(x => x.MaPhong == ma && x.MaDV == d.MaDV);
            if (cu != null) { cu.SoLuongSuDung = checked(cu.SoLuongSuDung + (int)sl.Value); cu.ThanhTien = cu.SoLuongSuDung * cu.DonGiaSuDung; }
            else nhap.DichVu.Add(new ChiTietSuDungDichVu { MaPhong = ma, MaDV = d.MaDV, TenDV = d.TenDV, SoLuongSuDung = (int)sl.Value, DonGiaSuDung = d.GiaTien, ThanhTien = sl.Value * d.GiaTien });
            VoHieu(); HienKhoanThu();
        }
        private void ThemPT()
        {
            string ma = PhongChon(); var p = frmChon.Chon<LuaChonPhuThu>("Chọn phụ thu", bus.PhuThu(), new CotHienThi("Mã", "MaPhuThu", 100), new CotHienThi("Phụ thu", "TenPhuThu", 280), new CotHienThi("Giá", "DonGia", 130));
            if (p == null) return;
            var cu = nhap.PhuThu.Find(x => x.MaPhong == ma && x.MaPhuThu == p.MaPhuThu);
            if (cu != null) { cu.SoLuong = checked(cu.SoLuong + (int)sl.Value); cu.ThanhTien = cu.SoLuong * cu.DonGia; cu.GhiChu = ghiChu.Text; }
            else nhap.PhuThu.Add(new ChiTietPhuThu { MaPhong = ma, MaPhuThu = p.MaPhuThu, TenPhuThu = p.TenPhuThu, SoLuong = (int)sl.Value, DonGia = p.DonGia, ThanhTien = sl.Value * p.DonGia, GhiChu = ghiChu.Text });
            VoHieu(); HienKhoanThu();
        }
        private void HienKhoanThu() { LvHelper.Do(dichVu, new ArrayList(nhap.DichVu), cotDV); LvHelper.Do(phuThu, new ArrayList(nhap.PhuThu), cotPT); }
        private void HienNhap() { LvHelper.Do(phong, new ArrayList(nhap.Phong), cotPhong); HienKhoanThu(); }
        private void Tinh()
        {
            VoHieu(); nhap.NgayThanhToan = ngay.Value;
            if (!bus.XemTruoc(nhap, out var h, out string loi)) { Bao(loi); return; }
            nhap = h; daXem = h; HienNhap(); tong.Text = Tong(h); thanhToan.Enabled = true;
        }
        private static string Tong(HoaDon h)
        {
            return $"Phòng: {h.TongTienPhong:N0}   Dịch vụ: {h.TongTienDichVu:N0}   Phụ thu: {h.TongTienPhuThu:N0}   Giảm: {h.TienGiam:N0}\nTổng hóa đơn: {h.TongTienHoaDon:N0}   Đã cọc: {h.TienDaCoc:N0}   " +
                (h.TienCanThanhToan < 0 ? $"Hoàn khách: {-h.TienCanThanhToan:N0} đ" : $"Cần thu: {h.TienCanThanhToan:N0} đ");
        }
        private void ThanhToan()
        {
            if (daXem == null) return;
            if (MessageBox.Show(this, Tong(daXem) + "\nXác nhận đã thu/hoàn tiền và checkout toàn bộ phòng?", "Thanh toán", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            thanhToan.Enabled = false;
            if (!bus.ThanhToan(daXem, out var h, out string loi)) { VoHieu(); Bao(loi); return; }
            nhap = new HoaDon(); daXem = null; phieu.Clear(); nv.Clear(); km.Text = "Không áp dụng"; HienNhap(); tong.Text = "Đã thanh toán " + h.MaHoaDon;
            Bao("Thanh toán thành công. Mã hóa đơn: " + h.MaHoaDon); TaiLichSu(); tabs.SelectedIndex = 1;
        }
        private void TaiLichSu() { bus.TaiDuLieu(); LvHelper.Do(lichSu, bus.DanhSach, cotHD); }
        private void XemChiTiet()
        {
            if (lichSu.SelectedItems.Count == 0) { Bao("Chọn hóa đơn cần xem."); return; }
            var h = (HoaDon)lichSu.SelectedItems[0].Tag; bus.DocChiTiet(h);
            using (var f = new Form { Text = "Hóa đơn " + h.MaHoaDon, Size = new Size(950, 620), StartPosition = FormStartPosition.CenterParent, Font = Font })
            {
                var root = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1 };
                root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100)); root.RowStyles.Add(new RowStyle(SizeType.AutoSize)); root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                root.Controls.Add(new Label { AutoSize = true, Text = $"{h.MaHoaDon} | Phiếu {h.MaPhieu} | {h.TenKH} | NV: {h.MaNV} | {h.NgayThanhToan:dd/MM/yyyy HH:mm:ss}\nKM: {h.MaKM ?? "Không áp dụng"}\n" + Tong(h), Padding = new Padding(8) }, 0, 0);
                var detail = new TabControl { Dock = DockStyle.Fill };
                var sources = new[] { new ArrayList(h.Phong), new ArrayList(h.DichVu), new ArrayList(h.PhuThu) };
                var columns = new[] { cotPhong, cotDV, cotPT }; var titles = new[] { "Phòng", "Dịch vụ", "Phụ thu" };
                for (int i = 0; i < 3; i++) { var page = new TabPage(titles[i]); var lv = DanhSach(); LvHelper.TaoCot(lv, columns[i]); LvHelper.Do(lv, sources[i], columns[i]); page.Controls.Add(lv); detail.TabPages.Add(page); }
                root.Controls.Add(detail, 0, 1); f.Controls.Add(root); f.ShowDialog(this);
            }
        }

        // frmMain chuyển trang bằng Dispose, vì vậy vẫn kiểm tra khoản thu chưa lưu trước khi rời trang.
        public bool CoTheDong() { return !CoNhap() || XacNhanBo(); }
    }
}
