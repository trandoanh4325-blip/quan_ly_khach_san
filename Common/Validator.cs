using System.Text.RegularExpressions;

namespace QLKS.Common
{
    /// <summary>Các hàm kiểm tra dữ liệu nhập dùng chung cho mọi module.</summary>
    public static class Validator
    {
        public static bool Rong(string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        /// <summary>CCCD: đúng 12 chữ số.</summary>
        public static bool CCCDHopLe(string s)
        {
            return s != null && Regex.IsMatch(s.Trim(), @"^\d{12}$");
        }

        /// <summary>Số điện thoại VN: 10 chữ số, bắt đầu bằng 0.</summary>
        public static bool SDTHopLe(string s)
        {
            return s != null && Regex.IsMatch(s.Trim(), @"^0\d{9}$");
        }

        /// <summary>Tên tham số: chữ thường không dấu, số, gạch dưới.</summary>
        public static bool TenThamSoHopLe(string s)
        {
            return s != null && Regex.IsMatch(s.Trim(), @"^[a-z0-9_]+$");
        }
    }
}
