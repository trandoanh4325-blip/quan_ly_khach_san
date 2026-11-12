using System;

namespace QLKS.DTO
{
    public class PhieuDatPhong
    {
        public const string DaThanhToan = "Đã thanh toán";
        public const string ChuaThanhToan = "Chưa thanh toán";

        public string MaPhieu { get; set; }
        public string MaKH { get; set; }
        public string TenKH { get; set; }          // chỉ để hiển thị (JOIN khach_hang)
        public string MaNV { get; set; }
        public string TenNV { get; set; }          // chỉ để hiển thị (JOIN nhan_vien)
        public DateTime NgayTao { get; set; }
        public DateTime NgayCheckin { get; set; }
        public DateTime? NgayCheckout { get; set; }   // check-out dự kiến, tự tính từ check-in + số ngày/giờ
        public decimal TienCoc { get; set; }
        public string TrangThaiCoc { get; set; }      // "Chưa cọc" / "Đã cọc"
        public int SoNgayODuKien { get; set; }
        public int OTheoGio { get; set; }             // 1 = Có, 2 = Không
        public int SoGioNghi { get; set; }
        public decimal TongTienDuKien { get; set; }   // read-only: tổng thành tiền các phòng
        public string TinhTrang { get; set; }         // suy ra từ việc đã có hóa đơn cho phiếu hay chưa

        public string HinhThuc
        {
            get { return OTheoGio == 1 ? "Theo giờ" : "Theo ngày"; }
        }
    }
}
