using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;
using QLKS.BUS;
using QLKS.Common;

namespace QLKS.GUI
{
   
    public class frmChon : Form
    {
        private readonly ArrayList nguon;
        private readonly CotHienThi[] cot;
        private ListView lv;
        private TextBox txtTim;
        private ucTimKiemNangCao ucTim;

        private frmChon(string tieuDe, ArrayList ds, CotHienThi[] cot)
        {
            this.nguon = ds;
            this.cot = cot;
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
            using (var f = new frmChon(tieuDe, ds, cot))
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

        private void XayDung()
        {
            var tl = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 4, Padding = new Padding(8) };
            tl.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            tl.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tl.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tl.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
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

            var nut = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
            var btnDong = new Button { Text = "Đóng", Width = 100, Height = 36 };
            btnDong.Click += (s, e) => { DialogResult = DialogResult.Cancel; };
            var btnChon = new Button { Text = "Chọn", Width = 100, Height = 36 };
            btnChon.Click += (s, e) => ChonDong();
            nut.Controls.Add(btnDong);
            nut.Controls.Add(btnChon);
            tl.Controls.Add(nut, 0, 3);

            Controls.Add(tl);
            Hien(nguon);
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
