using System.Collections;
using System.Windows.Forms;
using QLKS.Common;

namespace QLKS.GUI
{
    /// <summary>Đổ ArrayList (DTO) vào ListView theo danh sách cột.</summary>
    public static class LvHelper
    {
        public static void TaoCot(ListView lv, CotHienThi[] cot)
        {
            lv.Columns.Clear();
            foreach (CotHienThi c in cot) lv.Columns.Add(c.TieuDe, c.Rong);
        }

        public static void Do(ListView lv, ArrayList ds, CotHienThi[] cot)
        {
            lv.BeginUpdate();
            lv.Items.Clear();
            foreach (object o in ds)
            {
                var it = new ListViewItem(NoiDung(o, cot[0]));
                for (int i = 1; i < cot.Length; i++) it.SubItems.Add(NoiDung(o, cot[i]));
                it.Tag = o;                 // giữ lại đối tượng DTO gốc của dòng
                lv.Items.Add(it);
            }
            lv.EndUpdate();
        }

        private static string NoiDung(object o, CotHienThi cot)
        {
            object value = Rf.Get(o, cot.Prop);
            if (cot.DinhDang != null && value is System.IFormattable f) return f.ToString(cot.DinhDang, null);
            return Rf.Text(value);
        }
    }
}
