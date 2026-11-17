using System;
using System.Drawing;
using System.Windows.Forms;
using QLKS.BUS;
using QLKS.Common;
using QLKS.DTO;

namespace QLKS.GUI
{
    public class frmDichVu : Form
    {
        private readonly DichVuBUS bus = new DichVuBUS();
        private readonly ListView lv = new ListView { Dock = DockStyle.Fill, View = View.Details, FullRowSelect = true, MultiSelect = false, HideSelection = false, GridLines = true };
        private readonly TextBox tim = new TextBox { Width = 250, PlaceholderText = "Tìm dịch vụ..." };
        private readonly TextBox ma = new TextBox { ReadOnly = true, Width = 120 };
        private readonly TextBox ten = new TextBox { Width = 240, MaxLength = 100 };
        private readonly TextBox gia = new TextBox { Width = 140 };
        private readonly TextBox donVi = new TextBox { Width = 100, MaxLength = 20 };
        private readonly ucTimKiemNangCao nangCao = new ucTimKiemNangCao { Dock = DockStyle.Fill, Visible = false };
        private readonly Button them = new Button { Text = "Thêm" }, sua = new Button { Text = "Sửa" }, xoa = new Button { Text = "Xóa" }, luu = new Button { Text = "Lưu" }, huy = new Button { Text = "Hủy" };
        private bool dangThem;
        private readonly CotHienThi[] cot = { new CotHienThi("Mã dịch vụ", "MaDV", 110), new CotHienThi("Tên dịch vụ", "TenDV", 260),
            new CotHienThi("Giá tiền", "GiaTien", 150), new CotHienThi("Đơn vị tính", "DonViTinh", 130) };

        public frmDichVu()
        {
            Text = "Dịch vụ"; Font = new Font("Segoe UI", 10); Size = new Size(1050, 600);
            var root = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 5, Padding = new Padding(12) };
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            for (int i = 0; i < 5; i++) root.RowStyles.Add(new RowStyle(i == 2 ? SizeType.Percent : SizeType.AutoSize, i == 2 ? 100 : 0));
            var bar = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
            var nc = new Button { Text = "Tìm nâng cao", AutoSize = true };
            var tai = new Button { Text = "Tải lại", AutoSize = true };
            bar.Controls.AddRange(new Control[] { tim, nc, tai }); root.Controls.Add(bar, 0, 0);
            root.Controls.Add(nangCao, 0, 1); root.Controls.Add(lv, 0, 2);
            var editor = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, Padding = new Padding(0, 8, 0, 8) };
            foreach (var pair in new[] { ("Mã", ma), ("Tên dịch vụ", ten), ("Giá tiền", gia), ("Đơn vị", donVi) })
            { editor.Controls.Add(new Label { Text = pair.Item1, AutoSize = true, Margin = new Padding(4, 7, 4, 4) }); editor.Controls.Add(pair.Item2); }
            root.Controls.Add(editor, 0, 3);
            var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
            foreach (var b in new[] { them, sua, xoa, luu, huy }) { b.Height = 34; buttons.Controls.Add(b); }
            root.Controls.Add(buttons, 0, 4); Controls.Add(root);
            LvHelper.TaoCot(lv, cot); nangCao.DatTruong(cot);
            tim.TextChanged += (s, e) => LvHelper.Do(lv, bus.TimNhanh(tim.Text), cot);
            nc.Click += (s, e) => nangCao.Visible = !nangCao.Visible;
            nangCao.Dong += () => nangCao.Visible = false;
            nangCao.TimKiem += dk => LvHelper.Do(lv, bus.TimNangCao(dk), cot);
            nangCao.XoaLoc += () => LvHelper.Do(lv, bus.DanhSach, cot);
            tai.Click += (s, e) => AnToan(TaiLai);
            lv.SelectedIndexChanged += (s, e) => Dien();
            them.Click += (s, e) => AnToan(() => { dangThem = true; ma.Text = bus.SinhMa(); ten.Clear(); gia.Clear(); donVi.Clear(); CheDo(true); ten.Focus(); });
            sua.Click += (s, e) => { if (Chon() == null) { Bao("Vui lòng chọn dịch vụ."); return; } dangThem = false; CheDo(true); };
            huy.Click += (s, e) => { CheDo(false); Dien(); };
            luu.Click += (s, e) => AnToan(Luu);
            xoa.Click += (s, e) => AnToan(Xoa);
            Load += (s, e) => AnToan(TaiLai);
            CheDo(false);
        }

        private DichVu Chon() { return lv.SelectedItems.Count == 0 ? null : (DichVu)lv.SelectedItems[0].Tag; }
        private void Dien()
        {
            var d = Chon(); ma.Text = d?.MaDV ?? ""; ten.Text = d?.TenDV ?? ""; gia.Text = d?.GiaTien.ToString("0") ?? ""; donVi.Text = d?.DonViTinh ?? "";
        }
        private void CheDo(bool nhap)
        {
            ten.Enabled = gia.Enabled = donVi.Enabled = luu.Enabled = huy.Enabled = nhap;
            them.Enabled = sua.Enabled = xoa.Enabled = lv.Enabled = tim.Parent.Enabled = nangCao.Enabled = !nhap;
        }
        private void TaiLai() { bus.TaiDuLieu(); LvHelper.Do(lv, bus.TimNhanh(tim.Text), cot); Dien(); }
        private void Luu()
        {
            if (!LoaiPhongBUS.DocTien(gia.Text, out decimal tien)) { Bao("Giá tiền không hợp lệ."); return; }
            var d = new DichVu { MaDV = ma.Text, TenDV = ten.Text, GiaTien = tien, DonViTinh = donVi.Text };
            string loi;
            if (!(dangThem ? bus.Them(d, out loi) : bus.Sua(d, out loi))) { Bao(loi); return; }
            CheDo(false); TaiLai();
        }
        private void Xoa()
        {
            var d = Chon(); if (d == null) { Bao("Vui lòng chọn dịch vụ."); return; }
            if (MessageBox.Show(this, "Xóa dịch vụ " + d.TenDV + "?", "Xác nhận", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            if (!bus.Xoa(d.MaDV, out string loi)) { Bao(loi); return; } TaiLai();
        }
        private void Bao(string s) { MessageBox.Show(this, s, "Dịch vụ", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        private void AnToan(Action a) { try { a(); } catch (Exception ex) { Bao(ex.Message); } }
    }
}
