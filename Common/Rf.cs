using System;

namespace QLKS.Common
{
    /// <summary>Đọc thuộc tính của DTO theo tên (reflection) và định dạng ra chuỗi.</summary>
    public static class Rf
    {
        public static object Get(object o, string prop)
        {
            if (o == null) return null;
            var p = o.GetType().GetProperty(prop);
            return p == null ? null : p.GetValue(o, null);
        }

        public static string Text(object v)
        {
            if (v == null) return "";
            if (v is DateTime)
            {
                var dt = (DateTime)v;
                return dt.TimeOfDay == TimeSpan.Zero ? dt.ToString("dd/MM/yyyy") : dt.ToString("dd/MM/yyyy HH:mm");
            }
            if (v is decimal) return ((decimal)v).ToString("N0");
            return v.ToString();
        }
    }
}
