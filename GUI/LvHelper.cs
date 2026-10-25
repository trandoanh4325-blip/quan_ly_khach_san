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
                var it = new ListViewItem(Rf.Text(Rf.Get(o, cot[0].Prop)));
                for (int i = 1; i < cot.Length; i++) it.SubItems.Add(Rf.Text(Rf.Get(o, cot[i].Prop)));
                it.Tag = o;                 // giữ lại đối tượng DTO gốc của dòng
                lv.Items.Add(it);
            }
            lv.EndUpdate();
        }
    }
}
