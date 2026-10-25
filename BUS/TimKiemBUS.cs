using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;
using QLKS.Common;

namespace QLKS.BUS
{
    /// <summary>
    /// Tìm kiếm trên ArrayList (viết trong BUS theo yêu cầu GV). Mọi BUS gọi lại các hàm này
    /// với danh sách DTO đã load sẵn, không viết thêm truy vấn SQL cho việc tìm kiếm.
    /// </summary>
    public static class TimKiemBUS
    {
        public static readonly string[] PhepToan = { "=", "<>", ">", ">=", "<", "<=", "chứa" };

        // ------------------------------------------------------------------
        // Tìm nhanh: từ khóa xuất hiện ở BẤT KỲ thuộc tính nào (không phân biệt hoa/thường, có/không dấu)
        // ------------------------------------------------------------------
        public static ArrayList TimNhanh(ArrayList ds, string tuKhoa)
        {
            if (Validator.Rong(tuKhoa)) return new ArrayList(ds);
            string k = BoDau(tuKhoa.Trim()).ToLowerInvariant();
            var kq = new ArrayList();
            foreach (object o in ds)
            {
                foreach (PropertyInfo p in o.GetType().GetProperties())
                {
                    string s = Rf.Text(p.GetValue(o, null));
                    if (BoDau(s).ToLowerInvariant().Contains(k))
                    {
                        kq.Add(o);
                        break;
                    }
                }
            }
            return kq;
        }

        // ------------------------------------------------------------------
        // Tìm nâng cao: AND, OR, NOT và = <> > >= < <= trên 1 hoặc nhiều field.
        // Quy ước: AND được tính trước OR. Ví dụ  A AND B OR C  ==  (A AND B) OR C
        // NOT chỉ đảo kết quả của đúng điều kiện đó.
        // ------------------------------------------------------------------
        public static ArrayList Loc(ArrayList ds, List<TieuChi> dk)
        {
            if (dk == null || dk.Count == 0) return new ArrayList(ds);
            var kq = new ArrayList();
            foreach (object o in ds)
            {
                bool tong = false;                  // kết quả các nhóm OR đã xong
                bool nhom = Khop(o, dk[0]);         // nhóm AND hiện tại
                for (int i = 1; i < dk.Count; i++)
                {
                    bool r = Khop(o, dk[i]);
                    if (dk[i].Logic == "OR")
                    {
                        tong = tong || nhom;
                        nhom = r;
                    }
                    else
                    {
                        nhom = nhom && r;
                    }
                }
                tong = tong || nhom;
                if (tong) kq.Add(o);
            }
            return kq;
        }

        private static bool Khop(object o, TieuChi c)
        {
            object v = Rf.Get(o, c.Field);
            bool kq = SoSanh(v, c.Op, c.Value == null ? "" : c.Value.Trim());
            return c.Not ? !kq : kq;
        }

        private static bool SoSanh(object v, string op, string nhap)
        {
            if (op == "chứa")
                return BoDau(Rf.Text(v)).ToLowerInvariant().Contains(BoDau(nhap).ToLowerInvariant());

            int cmp;
            DateTime ngayNhap;
            decimal soNhap;
            if (v is DateTime && DateTime.TryParse(nhap, CultureInfo.CurrentCulture, DateTimeStyles.None, out ngayNhap))
            {
                cmp = ((DateTime)v).Date.CompareTo(ngayNhap.Date);
            }
            else if (LaSo(v) && TryDecimal(nhap, out soNhap))
            {
                cmp = Convert.ToDecimal(v).CompareTo(soNhap);
            }
            else
            {
                cmp = string.Compare(Rf.Text(v), nhap, StringComparison.CurrentCultureIgnoreCase);
            }

            switch (op)
            {
                case "=": return cmp == 0;
                case "<>": return cmp != 0;
                case ">": return cmp > 0;
                case ">=": return cmp >= 0;
                case "<": return cmp < 0;
                case "<=": return cmp <= 0;
            }
            return false;
        }

        private static bool LaSo(object v)
        {
            return v is int || v is long || v is short || v is byte || v is decimal || v is double || v is float;
        }

        private static bool TryDecimal(string s, out decimal x)
        {
            return decimal.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, out x)
                || decimal.TryParse(s.Replace(',', '.'), NumberStyles.Number, CultureInfo.InvariantCulture, out x);
        }

        /// <summary>Bỏ dấu tiếng Việt: "Nguyễn" -> "Nguyen".</summary>
        public static string BoDau(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            string n = s.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            foreach (char ch in n)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark) sb.Append(ch);
            }
            return sb.ToString().Normalize(NormalizationForm.FormC).Replace('đ', 'd').Replace('Đ', 'D');
        }
    }
}
