using System;
using System.Collections.Generic;

namespace QLKS.DTO
{
    public class HoaDon
    {
        public string MaHoaDon { get; set; }
        public string MaPhieu { get; set; }
        public string MaKH { get; set; }
        public string TenKH { get; set; }
        public string MaNV { get; set; }
        public DateTime NgayThanhToan { get; set; }
        public string MaKM { get; set; }
        public decimal TienDaCoc { get; set; }
        public decimal TienGiam { get; set; }
        public decimal TongTienPhong { get; set; }
        public decimal TongTienDichVu { get; set; }
        public decimal TongTienPhuThu { get; set; }
        public decimal TongTienHoaDon { get; set; }
        // Âm nghĩa là khách được hoàn lại tiền cọc.
        public decimal TienCanThanhToan { get; set; }
        public List<ChiTietDatPhong> Phong { get; set; } = new List<ChiTietDatPhong>();
        public List<ChiTietSuDungDichVu> DichVu { get; set; } = new List<ChiTietSuDungDichVu>();
        public List<ChiTietPhuThu> PhuThu { get; set; } = new List<ChiTietPhuThu>();
    }

    public class ChiTietSuDungDichVu
    {
        public string MaPhong { get; set; }
        public string MaDV { get; set; }
        public string TenDV { get; set; }
        public int SoLuongSuDung { get; set; }
        public decimal DonGiaSuDung { get; set; }
        public decimal ThanhTien { get; set; }
    }

    public class ChiTietPhuThu
    {
        public string MaPhong { get; set; }
        public string MaPhuThu { get; set; }
        public string TenPhuThu { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public decimal ThanhTien { get; set; }
        public string GhiChu { get; set; }
    }

    public class LuaChonPhuThu
    {
        public string MaPhuThu { get; set; }
        public string TenPhuThu { get; set; }
        public decimal DonGia { get; set; }
    }

    public class KhuyenMaiThanhToan
    {
        public string MaKM { get; set; }
        public string TenKM { get; set; }
        public string LoaiGiam { get; set; }
        public decimal GiaTriGiam { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public decimal TongThuToiThieu { get; set; }
        public int SoLuotToiToiThieu { get; set; }
    }
}
