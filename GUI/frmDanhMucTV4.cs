using System; using System.Collections; using System.Drawing; using System.Windows.Forms; using QLKS.BUS; using QLKS.DTO; using QLKS.DAO;using QLKS.Common;using System.Collections.Generic;
namespace QLKS.GUI {
    public class frmDanhMucTV4 : Form
    {
        readonly bool laLoai; readonly LoaiSanPhamBUS loai = new LoaiSanPhamBUS(); readonly NhaCungCapBUS ncc = new NhaCungCapBUS();
        readonly ListView lv = new ListView(); readonly TextBox tim = new TextBox(); readonly TextBox[] txt;
        readonly string[] nhan; readonly ucTimKiemNangCao ucTim = new ucTimKiemNangCao(); bool dangThem; readonly Button btnLuu = new Button(), btnHuy = new Button();
        public frmDanhMucTV4(bool loaiSanPham)
        {
            laLoai = loaiSanPham; nhan = laLoai ? new[] { "Mã loại SP", "Tên loại sản phẩm" } : new[] { "Mã nhà cung cấp", "Tên nhà cung cấp", "Số điện thoại", "Địa chỉ" };
            txt = new TextBox[nhan.Length]; Text = laLoai ? "QUẢN LÝ LOẠI SẢN PHẨM" : "QUẢN LÝ NHÀ CUNG CẤP"; Font = new Font("Segoe UI", 10); BackColor = Color.White; AutoScroll = true;
            var head = new Label { Text = Text, Font = new Font("Segoe UI", 17, FontStyle.Bold), Dock = DockStyle.Top, Height = 54, TextAlign = ContentAlignment.MiddleCenter }; Controls.Add(head);
            var top = new Panel { Dock = DockStyle.Top, Height = laLoai ? 130 : 185 }; Controls.Add(top); top.BringToFront();
            for (int i = 0; i < nhan.Length; i++) { int col = i % 2, row = i / 2; int x = 15 + col * 475, y = 12 + row * 52; top.Controls.Add(new Label { Text = nhan[i], Left = x, Top = y + 5, Width = 145 }); txt[i] = new TextBox { Left = x + 148, Top = y, Width = 285, MaxLength = laLoai ? (i == 0 ? 10 : 100) : (i == 0 ? 10 : i == 1 ? 150 : i == 2 ? 15 : 255) }; top.Controls.Add(txt[i]); }
            var menu = new FlowLayoutPanel { Dock = DockStyle.Top, Height = 46, Padding = new Padding(8) }; Controls.Add(menu); menu.BringToFront();
            Button Nut(string label, Action action) { var b = new Button { Text = label, Width = 103, Height = 30 }; b.Click += (s, e) => action(); menu.Controls.Add(b); return b; }
            Nut("Thêm", Them); Nut("Sửa", Sua); Nut("Xóa", Xoa); Nut("Excel", () => ExcelExport.XuatBang(this, laLoai ? "loai_san_pham" : "nha_cung_cap", laLoai ? "loai_san_pham" : "nha_cung_cap", laLoai ? "LoaiSanPham" : "NhaCungCap")); btnLuu.Text = "Lưu"; btnLuu.Width = 100; btnLuu.Click += (s, e) => Luu(); menu.Controls.Add(btnLuu); btnHuy.Text = "Hủy"; btnHuy.Width = 100; btnHuy.Click += (s, e) => Huy(); menu.Controls.Add(btnHuy); Nut("Tải lại", Tai);
            var search = new Panel { Dock = DockStyle.Top, Height = 48 }; Controls.Add(search); search.BringToFront(); search.Controls.Add(new Label { Text = "Tìm kiếm:", Left = 16, Top = 12, Width = 90 }); tim.SetBounds(112, 9, 400, 29); search.Controls.Add(tim); tim.TextChanged += (s, e) => HienThi();
            var advanced = new Button { Text = "Tìm nâng cao", Left = 535, Top = 9, Width = 140 }; search.Controls.Add(advanced);
            var filter = new Panel { Dock = DockStyle.Top, Height = 175 }; filter.Controls.Add(ucTim); ucTim.Dock = DockStyle.Fill; ucTim.Visible = false;
            var cotTim = new List<CotHienThi>();
            if (laLoai) { cotTim.Add(new CotHienThi("Mã loại SP", "MaLoaiSP", 120)); cotTim.Add(new CotHienThi("Tên loại sản phẩm", "TenLoaiSP", 240)); }
            else { cotTim.Add(new CotHienThi("Mã NCC", "MaNCC", 120)); cotTim.Add(new CotHienThi("Tên NCC", "TenNCC", 220)); cotTim.Add(new CotHienThi("Số điện thoại", "SoDienThoai", 140)); cotTim.Add(new CotHienThi("Địa chỉ", "DiaChi", 220)); }
            ucTim.DatTruong(cotTim.ToArray()); ucTim.TimKiem += dk => HienThiArray(TimKiemBUS.Loc(laLoai ? loai.DanhSach : ncc.DanhSach, dk)); ucTim.XoaLoc += () => HienThi(); ucTim.Dong += () => ucTim.Visible = false;
            lv.Dock = DockStyle.Fill; lv.View = View.Details; lv.FullRowSelect = true; lv.GridLines = true; lv.MultiSelect = false; for (int i = 0; i < nhan.Length; i++) lv.Columns.Add(nhan[i], i == 1 ? 300 : 180); lv.SelectedIndexChanged += (s, e) => Chon(); Controls.Add(lv); lv.BringToFront();
            // Dock ordering: re-add in layout order (top header, fields, actions, search, fill list)
            Controls.Clear(); Controls.Add(lv); Controls.Add(filter); Controls.Add(search); Controls.Add(menu); Controls.Add(top); Controls.Add(head); filter.Visible = false; advanced.Click += (s, e) => { filter.Visible = !filter.Visible; ucTim.Visible = filter.Visible; }; ucTim.Dong += () => filter.Visible = false;
            Load += (s, e) => Tai(); ChinhSua(false);
        }
        void ChinhSua(bool on) { foreach (var t in txt) t.ReadOnly = !on; btnLuu.Enabled = btnHuy.Enabled = on; }
        void Tai() { try { if (laLoai) loai.TaiDuLieu(); else ncc.TaiDuLieu(); HienThi(); } catch (Exception e) { Bao(e.Message); } }
        void HienThi() { HienThiArray(laLoai ? loai.TimKiem(tim.Text) : ncc.TimKiem(tim.Text)); }
        void HienThiArray(ArrayList ds) { lv.Items.Clear(); foreach (var obj in ds) { string[] data; if (laLoai) { var o = (LoaiSanPham)obj; data = new[] { o.MaLoaiSP, o.TenLoaiSP }; } else { var o = (NhaCungCap)obj; data = new[] { o.MaNCC, o.TenNCC, o.SoDienThoai, o.DiaChi }; } var item = new ListViewItem(data); lv.Items.Add(item); } }
        void Chon() { if (lv.SelectedItems.Count == 0 || btnLuu.Enabled) return; for (int i = 0; i < txt.Length; i++) txt[i].Text = lv.SelectedItems[0].SubItems[i].Text; }
        void Them() { foreach (var t in txt) t.Clear(); dangThem = true; ChinhSua(true); txt[0].Text = laLoai ? MaTuDongDAO.Sinh("LSP", "loai_san_pham", "ma_loai_sp") : MaTuDongDAO.Sinh("NCC", "nha_cung_cap", "ma_ncc"); txt[0].ReadOnly = true; txt[1].Focus(); }
        void Sua() { if (lv.SelectedItems.Count == 0) { Bao("Chọn một dòng để sửa."); return; } dangThem = false; ChinhSua(true); txt[0].ReadOnly = true; }
        void Huy() { ChinhSua(false); Chon(); }
        void Luu() { string loi; bool ok; if (laLoai) ok = loai.Luu(new LoaiSanPham { MaLoaiSP = txt[0].Text, TenLoaiSP = txt[1].Text }, dangThem, out loi); else ok = ncc.Luu(new NhaCungCap { MaNCC = txt[0].Text, TenNCC = txt[1].Text, SoDienThoai = txt[2].Text, DiaChi = txt[3].Text }, dangThem, out loi); if (!ok) { Bao(loi); return; } ChinhSua(false); HienThi(); MessageBox.Show("Đã lưu thành công."); }
        void Xoa() { if (lv.SelectedItems.Count == 0) { Bao("Chọn một dòng để xóa."); return; } if (MessageBox.Show("Xóa mã " + txt[0].Text + "?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return; string loi; bool ok = laLoai ? loai.Xoa(txt[0].Text, out loi) : ncc.Xoa(txt[0].Text, out loi); if (!ok) Bao(loi); else { HienThi(); foreach (var t in txt) t.Clear(); } }

        private void InitializeComponent()
        {

        }

        void Bao(string msg) => MessageBox.Show(msg, "Quản lý danh mục", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }
}
