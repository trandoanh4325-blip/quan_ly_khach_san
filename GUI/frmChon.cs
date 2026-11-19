using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using QLKS.BUS;
using QLKS.Common;
using QLKS.DTO;

namespace QLKS.GUI
{
   
    public class frmChon : Form
    {
        private readonly ArrayList nguon;
        private readonly CotHienThi[] cot;
        private ListView lv;
        private TextBox txtTim;
        private ucTimKiemNangCao ucTim;
        private readonly bool laKhachHang;
        private readonly bool coSoLuong;
        private NumericUpDown nudSoLuong;

        private frmChon(string tieuDe, ArrayList ds, CotHienThi[] cot, bool laKhachHang, bool coSoLuong)
        {
            this.nguon = ds;
            this.cot = cot;
            this.laKhachHang = laKhachHang;
            this.coSoLuong = coSoLuong;
            Text = tieuDe;
            Font = new Font("Segoe UI", 10F);
            Width = 900;
            Height = 560;
            StartPosition = FormStartPosition.CenterParent;
            XayDung();
        }

        public static T Chon<T>(string tieuDe, ArrayList ds, params CotHienThi[] cot) where T : class
        {
            SelectionContext.Xoa();
            using (var f = new frmChon(tieuDe, ds, cot, typeof(T) == typeof(KhachHang), false))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    T kq = SelectionContext.DoiTuongDaChon as T;
                    SelectionContext.Xoa();
                    return kq;
                }
            }
            return null;
        }

        public static T Chon<T>(string tieuDe, ArrayList ds, out int soLuong, params CotHienThi[] cot) where T : class
        {
            soLuong = 1;
            SelectionContext.Xoa();
            using (var f = new frmChon(tieuDe, ds, cot, false, true))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    soLuong = (int)f.nudSoLuong.Value;
                    T kq = SelectionContext.DoiTuongDaChon as T;
                    SelectionContext.Xoa();
                    return kq;
                }
            }
            return null;
        }

        private void XayDung()
        {
            var tl = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = laKhachHang ? 5 : 4, Padding = new Padding(8) };
            tl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            tl.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tl.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tl.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            if (laKhachHang) tl.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
            tl.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));

            var bar = new Panel { Height = 42, Dock = DockStyle.Fill };
            bar.Controls.Add(new Label { Text = "Tìm nhanh:", Left = 4, Top = 11, AutoSize = true });
            txtTim = new TextBox { Left = 90, Top = 8, Width = 300 };
            txtTim.TextChanged += (s, e) => Hien(TimKiemBUS.TimNhanh(nguon, txtTim.Text));
            var btnNC = new Button { Text = "Tìm nâng cao", Left = 400, Top = 6, Width = 140, Height = 30 };
            btnNC.Click += (s, e) => ucTim.Visible = !ucTim.Visible;
            bar.Controls.Add(txtTim);
            bar.Controls.Add(btnNC);
            tl.Controls.Add(bar, 0, 0);

            ucTim = new ucTimKiemNangCao { Dock = DockStyle.Fill, Visible = false };
            ucTim.DatTruong(cot);
            ucTim.TimKiem += dk => Hien(TimKiemBUS.Loc(nguon, dk));
            ucTim.XoaLoc += () => Hien(nguon);
            ucTim.Dong += () => ucTim.Visible = false;
            tl.Controls.Add(ucTim, 0, 1);

            lv = new ListView
            {
                Dock = DockStyle.Fill, View = View.Details, FullRowSelect = true,
                GridLines = true, MultiSelect = false, HideSelection = false
            };
            LvHelper.TaoCot(lv, cot);
            lv.DoubleClick += (s, e) => ChonDong();
            tl.Controls.Add(lv, 0, 2);

            if (laKhachHang)
            {
                var nhanh = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
                Button NutNhanh(string text, Action action)
                {
                    var b = new Button { Text = text, Width = 105, Height = 30 };
                    b.Click += (s, e) => action();
                    nhanh.Controls.Add(b);
                    return b;
                }
                NutNhanh("Thêm", ThemKhachHang);
                NutNhanh("Sửa", SuaKhachHang);
                NutNhanh("Xóa", XoaKhachHang);
                NutNhanh("Tải lại", TaiLaiKhachHang);
                tl.Controls.Add(nhanh, 0, 3);
            }

            var nut = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
            var btnDong = new Button { Text = "Đóng", Width = 100, Height = 36 };
            btnDong.Click += (s, e) => { DialogResult = DialogResult.Cancel; };
            var btnChon = new Button { Text = "Chọn", Width = 100, Height = 36 };
            btnChon.Click += (s, e) => ChonDong();
            nut.Controls.Add(btnDong);
            nut.Controls.Add(btnChon);
            if (coSoLuong)
            {
                nudSoLuong = new NumericUpDown { Minimum = 1, Maximum = 1000000, Value = 1, Width = 90, Height = 32, ThousandsSeparator = true };
                nut.Controls.Add(nudSoLuong);
                nut.Controls.Add(new Label { Text = "Số lượng:", AutoSize = true, Padding = new Padding(0, 9, 0, 0) });
            }
            tl.Controls.Add(nut, 0, laKhachHang ? 4 : 3);

            Controls.Add(tl);
            Hien(nguon);
        }

        private KhachHang KhachDangChon()
        {
            return lv.SelectedItems.Count == 0 ? null : lv.SelectedItems[0].Tag as KhachHang;
        }

        private void ThemKhachHang()
        {
            using (var f = new frmKhachHang()) f.ShowDialog(this);
            TaiLaiKhachHang();
        }

        private void SuaKhachHang()
        {
            KhachHang kh = KhachDangChon();
            if (kh == null) { MessageBox.Show(this, "Vui lòng chọn khách hàng cần sửa.", "Thông báo"); return; }
            using (var f = new frmKhachHang(kh.MaKH)) f.ShowDialog(this);
            TaiLaiKhachHang();
        }

        private void XoaKhachHang()
        {
            KhachHang kh = KhachDangChon();
            if (kh == null) { MessageBox.Show(this, "Vui lòng chọn khách hàng cần xóa.", "Thông báo"); return; }
            if (MessageBox.Show(this, "Xóa khách hàng " + kh.MaKH + "?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            var bus = new KhachHangBUS();
            if (!bus.Xoa(kh.MaKH, out string loi)) MessageBox.Show(this, loi, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TaiLaiKhachHang();
        }

        private void TaiLaiKhachHang()
        {
            var bus = new KhachHangBUS();
            bus.TaiDuLieu();
            nguon.Clear();
            foreach (KhachHang kh in bus.DanhSach) nguon.Add(kh);
            Hien(TimKiemBUS.TimNhanh(nguon, txtTim.Text));
        }

        private void Hien(ArrayList ds)
        {
            LvHelper.Do(lv, ds, cot);
        }

        private void ChonDong()
        {
            if (lv.SelectedItems.Count == 0)
            {
                MessageBox.Show("Vui lòng chọn 1 dòng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            SelectionContext.DoiTuongDaChon = lv.SelectedItems[0].Tag;   // biến static
            DialogResult = DialogResult.OK;
        }
    }
}
