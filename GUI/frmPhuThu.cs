using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using QLKS.BUS;
using QLKS.Common;
using QLKS.DTO;

namespace QLKS.GUI
{
    public class frmPhuThu : Form
    {
        private readonly ListView lv = new ListView { Dock = DockStyle.Fill, View = View.Details, FullRowSelect = true, MultiSelect = false, HideSelection = false, GridLines = true };
        private readonly TextBox tim = new TextBox { Width = 250, PlaceholderText = "Tìm nhanh..." };
        private readonly ucTimKiemNangCao nangCao = new ucTimKiemNangCao { Dock = DockStyle.Fill, Visible = false };
        private readonly FlowLayoutPanel bar = Hang(), buttons = Hang();
        private readonly FlowLayoutPanel editor = Hang();
        private readonly TextBox ma = new TextBox { Width = 120, ReadOnly = true };
        private readonly Button them = new Button { Text = "Thêm" }, sua = new Button { Text = "Sửa" }, xoa = new Button { Text = "Xóa" }, luu = new Button { Text = "Lưu" }, huy = new Button { Text = "Hủy" };
        private CotHienThi[] cot;
        private bool nhap, dangThem;
        private List<TieuChi> dieuKien;
        private object DongChon { get { return lv.SelectedItems.Count == 0 ? null : lv.SelectedItems[0].Tag; } }

        private readonly PhuThuBUS bus = new PhuThuBUS();
        private readonly TextBox ten = new TextBox { Width = 280, MaxLength = 150 };
        private readonly TextBox sanPham = new TextBox { Width = 240, ReadOnly = true };
        private readonly TextBox gia = new TextBox { Width = 130, ReadOnly = true };
        private readonly TextBox donVi = new TextBox { Width = 90, ReadOnly = true };
        private string maSP;
        public frmPhuThu()
        {
            XayDung("Danh mục Phụ thu", "Chọn sản phẩm liên kết; đơn giá phụ thu lấy từ giá bán sản phẩm.", new[] {
            new CotHienThi("Mã phụ thu", "MaPhuThu", 100), new CotHienThi("Tên phụ thu", "TenPhuThu", 260),
            new CotHienThi("Mã sản phẩm", "MaSP", 110), new CotHienThi("Sản phẩm liên kết", "TenSP", 220),
            new CotHienThi("Đơn giá (đ)", "DonGia", 130), new CotHienThi("Đơn vị", "DonViTinh", 90) });
            ThemTruong("Tên phụ thu", ten); ThemTruong("Sản phẩm", sanPham);
            var chon = new Button { Text = "Chọn sản phẩm…", AutoSize = true, Height = 34 }; editor.Controls.Add(chon);
            ThemTruong("Đơn giá (đ)", gia); ThemTruong("Đơn vị", donVi);
            chon.Click += (s, e) => AnToan(() =>
            {
                var ds = bus.SanPham(); if (ds.Count == 0) { Bao("Chưa có sản phẩm. Cần có sản phẩm trước khi tạo phụ thu."); return; }
                var sp = frmChon.Chon<SanPhamPhuThu>("Chọn sản phẩm cho phụ thu", ds, new CotHienThi("Mã", "MaSP", 100),
                    new CotHienThi("Sản phẩm", "TenSP", 280), new CotHienThi("Giá bán (đ)", "GiaBan", 150), new CotHienThi("Đơn vị", "DonViTinh", 100));
                if (sp == null) return;
                maSP = sp.MaSP; sanPham.Text = sp.MaSP + " - " + sp.TenSP; gia.Text = sp.GiaBan.ToString("N0"); donVi.Text = sp.DonViTinh;
            });
        }
        private ArrayList LayDanhSach(bool taiLai) { if (taiLai) bus.TaiDuLieu(); return bus.DanhSach; }
        private ArrayList TimNhanh(string s) { return bus.TimNhanh(s); }
        private ArrayList TimNangCao(List<TieuChi> dk) { return bus.TimNangCao(dk); }
        private string SinhMa() { return bus.SinhMa(); }
        private void Dien(object item)
        {
            var p = item as PhuThu; ma.Text = p?.MaPhuThu ?? ""; ten.Text = p?.TenPhuThu ?? ""; maSP = p?.MaSP;
            sanPham.Text = p == null ? "" : p.MaSP + " - " + p.TenSP; gia.Text = p?.DonGia.ToString("N0") ?? ""; donVi.Text = p?.DonViTinh ?? "";
        }
        private bool Luu(bool them, out string loi) { return bus.Luu(new PhuThu { MaPhuThu = ma.Text, TenPhuThu = ten.Text, MaSP = maSP }, them, out loi); }
        private bool Xoa(object item, out string loi) { return bus.Xoa(((PhuThu)item).MaPhuThu, out loi); }
        private void XayDung(string title, string huongDan, CotHienThi[] columns)
        {
            Text = title; Font = new Font("Segoe UI", 10); Size = new Size(1080, 650); cot = columns;
            var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 6, Padding = new Padding(12) };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < 6; i++) root.RowStyles.Add(new RowStyle(i == 3 ? SizeType.Percent : SizeType.AutoSize, i == 3 ? 100 : 0));
            var help = new Label { Text = title + " — " + huongDan, AutoSize = true, Dock = DockStyle.Fill, Padding = new Padding(0, 0, 0, 10) };
            root.Controls.Add(help, 0, 0);
            var nc = new Button { Text = "Tìm nâng cao", AutoSize = true }; var tai = new Button { Text = "Tải lại", AutoSize = true };
            bar.Controls.AddRange(new Control[] { tim, nc, tai }); root.Controls.Add(bar, 0, 1);
            root.Controls.Add(nangCao, 0, 2); root.Controls.Add(lv, 0, 3);
            editor.Padding = new Padding(0, 8, 0, 8); ThemTruong("Mã tự sinh", ma); root.Controls.Add(editor, 0, 4);
            foreach (var b in new[] { them, sua, xoa, luu, huy }) { b.AutoSize = true; b.Height = 34; buttons.Controls.Add(b); }
            root.Controls.Add(buttons, 0, 5); Controls.Add(root);
            LvHelper.TaoCot(lv, cot); nangCao.DatTruong(cot);
            nc.Click += (s, e) => nangCao.Visible = !nangCao.Visible;
            nangCao.Dong += () => nangCao.Visible = false;
            nangCao.TimKiem += dk => AnToan(() => { dieuKien = dk; Hien(false); });
            nangCao.XoaLoc += () => AnToan(() => { dieuKien = null; tim.Clear(); Hien(false); });
            tim.TextChanged += (s, e) => AnToan(() => { dieuKien = null; Hien(false); });
            tai.Click += (s, e) => AnToan(() => Hien(true));
            lv.SelectedIndexChanged += (s, e) => { if (!nhap) AnToan(() => Dien(DongChon)); };
            them.Click += (s, e) => AnToan(() => { dangThem = true; Dien(null); ma.Text = SinhMa(); CheDo(true); });
            sua.Click += (s, e) => { if (DongChon == null) { Bao("Vui lòng chọn dòng cần sửa."); return; } dangThem = false; CheDo(true); };
            huy.Click += (s, e) => AnToan(() => { CheDo(false); Dien(DongChon); });
            luu.Click += (s, e) => AnToan(() => { if (!Luu(dangThem, out string loi)) { Bao(loi); return; } CheDo(false); Hien(true); });
            xoa.Click += (s, e) => AnToan(() =>
            {
                var item = DongChon; if (item == null) { Bao("Vui lòng chọn dòng cần xóa."); return; }
                if (MessageBox.Show(this, "Xóa danh mục đang chọn? Danh mục đã dùng trong hóa đơn không thể xóa.", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                if (!Xoa(item, out string loi)) { Bao(loi); return; } Hien(true);
            });
            Load += (s, e) => AnToan(() => { CheDo(false); Hien(true); });
            FormClosing += (s, e) => { if (!CoTheDong()) e.Cancel = true; };
        }

        private static FlowLayoutPanel Hang() { return new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, WrapContents = true }; }
        private void ThemTruong(string label, Control c)
        {
            // Mỗi nhãn đi cùng ô nhập khi xuống dòng ở cửa sổ hẹp.
            var group = new FlowLayoutPanel { AutoSize = true, WrapContents = false, Margin = new Padding(0, 3, 12, 3) };
            group.Controls.Add(new Label { Text = label, AutoSize = true, Margin = new Padding(0, 7, 6, 0) }); group.Controls.Add(c); editor.Controls.Add(group);
        }
        private void CheDo(bool value)
        {
            nhap = value; editor.Enabled = luu.Enabled = huy.Enabled = value;
            lv.Enabled = bar.Enabled = nangCao.Enabled = them.Enabled = sua.Enabled = xoa.Enabled = !value;
        }
        private void Hien(bool taiLai)
        {
            LayDanhSach(taiLai);
            LvHelper.Do(lv, dieuKien == null ? TimNhanh(tim.Text) : TimNangCao(dieuKien), cot);
            Dien(null);
        }
        private void Bao(string message) { MessageBox.Show(this, message, Text, MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void AnToan(Action a) { try { a(); } catch (Exception ex) { Bao(ex.Message); } }
        public bool CoTheDong()
        {
            return !nhap || MessageBox.Show(this, "Bạn đang nhập danh mục chưa lưu. Bỏ các thay đổi và rời màn hình?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}
