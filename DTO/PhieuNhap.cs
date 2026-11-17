using System; using System.Collections.Generic;
namespace QLKS.DTO {
 public class PhieuNhap { public string MaPhieuNhap {get;set;} public string MaNCC {get;set;} public string MaNV {get;set;} public DateTime NgayNhap {get;set;} public decimal TongTienNhap {get;set;} public List<ChiTietPhieuNhap> ChiTiet {get;set;} = new List<ChiTietPhieuNhap>(); }
 public class ChiTietPhieuNhap { public string MaPhieuNhap {get;set;} public string MaSP {get;set;} public int SoLuongNhap {get;set;} public decimal DonGiaNhap {get;set;} public decimal ThanhTien => SoLuongNhap * DonGiaNhap; }
}
