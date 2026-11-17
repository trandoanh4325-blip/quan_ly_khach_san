namespace QLKS.DTO
{
    public class LoaiPhong
    {
        public string MaLoai { get; set; }
        public string TenLoai { get; set; }
        public decimal DonGiaNgay { get; set; }
        public decimal DonGiaGio { get; set; }
        public int SoLuongDangCo { get; set; }   // read-only: = số phòng đang trống, tự cập nhật
        public int TongSoPhong { get; set; }     // read-only: tự đếm từ bảng phòng
    }
}
