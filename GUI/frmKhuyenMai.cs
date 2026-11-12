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
    public class frmKhuyenMai : Form
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

        private readonly KhuyenMaiBUS bus = new KhuyenMaiBUS();
        private readonly TextBox ten = new TextBox { Width = 300, MaxLength = 150 };
        private readonly ComboBox loai = new ComboBox { Width = 130, DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly NumericUpDown giam = So(9999999999999m), thu = So(999999999999999m), luot = So(int.MaxValue);
        private readonly DateTimePicker tu = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy", Width = 130 };
        private readonly DateTimePicker den = new DateTimePicker { Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy", Width = 130 };
        private readonly Label donVi = new Label { AutoSize = true };
        private static NumericUpDown So(decimal max) { return new NumericUpDown { Minimum = 0, Maximum = max, ThousandsSeparator = true, Width = 160 }; }
        public frmKhuyenMai()
        {
            XayDung("Danh mục Khuyến mãi", "Điều kiện khách hàng tính theo tổng thu và lượt đến trước lần thanh toán đang lập.", new[] {
            new CotHienThi("Mã KM", "MaKM", 90), new CotHienThi("Tên khuyến mãi", "TenKM", 260), new CotHienThi("Loại giảm", "LoaiGiam", 110),
            new CotHienThi("Giá trị (% / đ)", "GiaTriGiam", 120, "#,##0.##"), new CotHienThi("Bắt đầu", "NgayBatDau", 110), new CotHienThi("Kết thúc", "NgayKetThuc", 110),
            new CotHienThi("Tổng thu tối thiểu", "TongThuToiThieu", 150), new CotHienThi("Lượt đến tối thiểu", "SoLuotToiToiThieu", 130) });
            loai.Items.AddRange(new object[] { "Phần trăm", "Số tiền" });
            ThemTruong("Tên khuyến mãi", ten); ThemTruong("Loại giảm", loai); ThemTruong("Giá trị giảm", giam); ThemTruong("Đơn vị", donVi);
            ThemTruong("Từ ngày", tu); ThemTruong("Đến ngày", den); ThemTruong("Tổng thu tối thiểu (đ)", thu); ThemTruong("Lượt đến tối thiểu", luot);
            loai.SelectedIndexChanged += (s, e) => DoiLoai();
        }
        private void DoiLoai()
        {
            bool phanTram = loai.SelectedIndex == 0;
            // Không tự cắt giá trị đã nhập khi đổi loại; BUS sẽ báo lỗi nếu vượt 100%.
            giam.DecimalPlaces = phanTram ? 2 : 0; giam.Increment = phanTram ? 0.5m : 1000;
            donVi.Text = phanTram ? "% (tối đa 100)" : "đồng";
        }
        private ArrayList LayDanhSach(bool taiLai) { if (taiLai) bus.TaiDuLieu(); return bus.DanhSach; }
        private ArrayList TimNhanh(string s) { return bus.TimNhanh(s); }
        private ArrayList TimNangCao(List<TieuChi> dk) { return bus.TimNangCao(dk); }
        private string SinhMa() { return bus.SinhMa(); }
        private void Dien(object item)
        {
            var k = item as KhuyenMai; ma.Text = k?.MaKM ?? ""; ten.Text = k?.TenKM ?? "";
            loai.SelectedItem = k?.LoaiGiam ?? "Phần trăm"; DoiLoai();
            giam.Value = Math.Clamp(k?.GiaTriGiam ?? 0, giam.Minimum, giam.Maximum);
            thu.Value = Math.Clamp(k?.TongThuToiThieu ?? 0, thu.Minimum, thu.Maximum);
            luot.Value = Math.Clamp(k?.SoLuotToiToiThieu ?? 0, luot.Minimum, luot.Maximum);
            tu.Value = k?.NgayBatDau ?? DateTime.Today; den.Value = k?.NgayKetThuc ?? DateTime.Today.AddMonths(1);
        }
        private bool Luu(bool them, out string loi)
        {
            return bus.Luu(new KhuyenMai { MaKM = ma.Text, TenKM = ten.Text, LoaiGiam = loai.SelectedItem?.ToString(),
                GiaTriGiam = giam.Value, NgayBatDau = tu.Value, NgayKetThuc = den.Value, TongThuToiThieu = thu.Value, SoLuotToiToiThieu = (int)luot.Value }, them, out loi);
        }
        private bool Xoa(object item, out string loi) { return bus.Xoa(((KhuyenMai)item).MaKM, out loi); }
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
