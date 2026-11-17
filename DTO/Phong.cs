namespace QLKS.DTO
{
    public class Phong
    {
        public string MaPhong { get; set; }
        public string TenPhong { get; set; }
        public string MaLoai { get; set; }
        public string TenLoai { get; set; }      // chỉ để hiển thị (JOIN loai_phong)
        public string TrangThai { get; set; }    // "Trống" / "Đã đủ" - read-only trên form, do check-in / thanh toán đổi
        public int SoNguoiToiDa { get; set; }    // 2 / 4 / 6 / 8
    }
}
