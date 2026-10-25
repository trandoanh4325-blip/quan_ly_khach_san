using System;

namespace QLKS.DAO
{
    /// <summary>Sinh mã tự động dạng tiền tố + số, ví dụ NV001, KH012.</summary>
    public static class MaTuDongDAO
    {
        public static string Sinh(string tienTo, string bang, string cot, int doDaiSo = 3)
        {
            string sql = "SELECT COALESCE(MAX(CAST(SUBSTRING(" + cot + ", " + (tienTo.Length + 1) +
                         ") AS UNSIGNED)), 0) FROM " + bang + " WHERE " + cot + " LIKE @p";
            object kq = MyConnection.ExecuteScalar(sql, MyConnection.P("@p", tienTo + "%"));
            long so = Convert.ToInt64(kq) + 1;
            return tienTo + so.ToString().PadLeft(doDaiSo, '0');
        }
    }
}
