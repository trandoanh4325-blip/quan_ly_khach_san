using QLKS.DAO;

namespace QLKS.BUS
{
    /// <summary>Cầu nối để lớp GUI khởi tạo / đóng kết nối mà không gọi thẳng DAO.</summary>
    public static class KetNoiBUS
    {
        public static void KhoiTao()
        {
            MyConnection.Open();
        }

        public static void Dong()
        {
            MyConnection.Close();
        }
    }
}
