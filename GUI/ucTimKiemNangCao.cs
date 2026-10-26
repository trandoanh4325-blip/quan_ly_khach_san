using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using QLKS.BUS;
using QLKS.Common;

namespace QLKS.GUI
{
    /// <summary>
    /// Component tìm kiếm nâng cao dùng chung (mục 8 yêu cầu).
    /// Mỗi dòng: [AND/OR] [NOT] [Field] [Phép toán] [Giá trị]. Dòng đầu không có AND/OR.
    /// Cách dùng: ucTim.DatTruong(cot); ucTim.TimKiem += dk => ... bus.TimNangCao(dk);
    /// </summary>
    public class ucTimKiemNangCao : UserControl
    {
        public event Action<List<TieuChi>> TimKiem;
        public event Action XoaLoc;
        public event Action Dong;
        private const int RowH = 36;
        private readonly List<Panel> dong = new List<Panel>();
        private readonly Panel pnlNut = new Panel();
        private CotHienThi[] truong = new CotHienThi[0];

        public ucTimKiemNangCao()
        {
            BorderStyle = BorderStyle.FixedSingle;
            BackColor = Color.WhiteSmoke;

            pnlNut.Height = 42;
            pnlNut.Width = 900;
            pnlNut.Left = 0;

            var btnThem = new Button { Text = "+ Thêm điều kiện", Left = 6, Top = 6, Width = 150, Height = 30 };
            var btnTim = new Button { Text = "Tìm", Left = 162, Top = 6, Width = 80, Height = 30 };
            var btnXoa = new Button { Text = "Xóa lọc", Left = 248, Top = 6, Width = 90, Height = 30 };
            var btnDong = new Button { Text = "Đóng", Left = 344, Top = 6, Width = 80, Height = 30 };
            
            btnThem.Click += (s, e) => ThemDong();
            btnTim.Click += (s, e) => { if (TimKiem != null) TimKiem(LayTieuChi()); };
            btnXoa.Click += (s, e) =>
            {
                DatLai();
                if (XoaLoc != null) XoaLoc();
            };
            btnDong.Click += (s, e) =>
            {
                if (Dong != null) Dong();
            };
            pnlNut.Controls.Add(btnThem);
            pnlNut.Controls.Add(btnTim);
            pnlNut.Controls.Add(btnXoa);
            pnlNut.Controls.Add(btnDong);
            
            Controls.Add(pnlNut);
            Height = pnlNut.Height + RowH;
        }

        /// <summary>Khai báo các field cho phép lọc (thường dùng lại mảng Cot của form).</summary>
        public void DatTruong(CotHienThi[] ds)
        {
            truong = ds;
            DatLai();
        }

        private void DatLai()
        {
            foreach (Panel p in dong) { Controls.Remove(p); p.Dispose(); }
            dong.Clear();
            ThemDong();
        }

        private void ThemDong()
        {
            var p = new Panel { Height = RowH, Width = 900, Left = 0 };

            var cboLogic = new ComboBox { Left = 6, Top = 6, Width = 62, DropDownStyle = ComboBoxStyle.DropDownList };
            cboLogic.Items.AddRange(new object[] { "AND", "OR" });
            cboLogic.SelectedIndex = 0;

            var chkNot = new CheckBox { Text = "NOT", Left = 76, Top = 8, Width = 58 };

            var cboTruong = new ComboBox { Left = 138, Top = 6, Width = 170, DropDownStyle = ComboBoxStyle.DropDownList };
            foreach (CotHienThi c in truong) cboTruong.Items.Add(c.TieuDe);
            if (cboTruong.Items.Count > 0) cboTruong.SelectedIndex = 0;

            var cboPhep = new ComboBox { Left = 314, Top = 6, Width = 80, DropDownStyle = ComboBoxStyle.DropDownList };
            cboPhep.Items.AddRange(TimKiemBUS.PhepToan);
            cboPhep.SelectedIndex = 0;

            var txtGT = new TextBox { Left = 400, Top = 6, Width = 200 };

            var btnBo = new Button { Text = "×", Left = 606, Top = 5, Width = 32, Height = 26 };
            btnBo.Click += (s, e) =>
            {
                if (dong.Count <= 1)
                {
                    ((TextBox)((Control[])p.Tag)[4]).Clear();
                    return;
                }
                dong.Remove(p);
                Controls.Remove(p);
                p.Dispose();
                Relayout();
            };

            p.Tag = new Control[] { cboLogic, chkNot, cboTruong, cboPhep, txtGT };
            p.Controls.AddRange(new Control[] { cboLogic, chkNot, cboTruong, cboPhep, txtGT, btnBo });
            dong.Add(p);
            Controls.Add(p);
            Relayout();
        }

        private void Relayout()
        {
            for (int i = 0; i < dong.Count; i++)
            {
                dong[i].Top = i * RowH;
                var c = (Control[])dong[i].Tag;
                c[0].Visible = i > 0;           // dòng đầu không có AND/OR
            }
            pnlNut.Top = dong.Count * RowH;
            Height = pnlNut.Top + pnlNut.Height + 2;
        }

        private List<TieuChi> LayTieuChi()
        {
            var ds = new List<TieuChi>();
            foreach (Panel p in dong)
            {
                var c = (Control[])p.Tag;
                string gt = ((TextBox)c[4]).Text.Trim();
                int idx = ((ComboBox)c[2]).SelectedIndex;
                if (gt.Length == 0 || idx < 0) continue;     // bỏ qua dòng chưa nhập giá trị
                ds.Add(new TieuChi
                {
                    Logic = ((ComboBox)c[0]).Text,
                    Not = ((CheckBox)c[1]).Checked,
                    Field = truong[idx].Prop,
                    Op = ((ComboBox)c[3]).Text,
                    Value = gt
                });
            }
            return ds;
        }
    }
}
