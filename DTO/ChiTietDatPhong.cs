using System;

namespace QLKS.DTO
{
    public class ChiTietDatPhong
    {
        public const string DangO = "Đang ở";
        public const string DaCheckout = "Đã checkout";

        public string MaPhieu { get; set; }
        public string MaPhong { get; set; }
        public string TenPhong { get; set; }         // chỉ để hiển thị (JOIN phong)
        public int SoNguoiToiDa { get; set; }        // của phòng, dùng để kiểm tra số khách
        public decimal DonGiaNgay { get; set; }      // đơn giá hiện hành của loại phòng
        public decimal DonGiaGio { get; set; }
        public int SoKhach { get; set; }
        public decimal GiaPhong { get; set; }        // đơn giá áp dụng (ngày hoặc giờ theo phiếu)
        public DateTime? NgayGioCheckout { get; set; }   // để trống đến khi checkout (TV3 ghi lúc thanh toán)
        public string TrangThai { get; set; }        // "Đang ở" / "Đã checkout"
        public int SoNgayO { get; set; }
        public int SoGioO { get; set; }
        public decimal ThanhTien { get; set; }       // read-only
    }
}
