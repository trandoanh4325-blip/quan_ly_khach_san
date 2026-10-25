namespace QLKS.Common
{
    /// <summary>
    /// Biến static dùng cho nút "..." (mục 4 yêu cầu): form tìm kiếm ghi đối tượng được chọn vào đây,
    /// form đang làm việc đọc ra rồi điền vào các ô. Dùng xong phải gọi Xoa().
    /// </summary>
    public static class SelectionContext
    {
        public static object DoiTuongDaChon;

        public static void Xoa()
        {
            DoiTuongDaChon = null;
        }
    }
}
