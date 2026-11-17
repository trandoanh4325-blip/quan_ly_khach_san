using System;
using System.Data;
using QLKS.DTO;
namespace QLKS.DAO {
 public class PhieuNhapDAO {
  public DataTable LayDanhSach() => MyConnection.ExecuteQuery(@"SELECT p.ma_phieu_nhap,p.ma_ncc,n.ten_ncc,p.ma_nv,v.ho_ten,p.ngay_nhap,p.tong_tien_nhap FROM phieu_nhap p JOIN nha_cung_cap n ON n.ma_ncc=p.ma_ncc JOIN nhan_vien v ON v.ma_nv=p.ma_nv ORDER BY p.ngay_nhap DESC,p.ma_phieu_nhap DESC");
  public DataTable ChiTiet(string id) => MyConnection.ExecuteQuery(@"SELECT c.ma_phieu_nhap,c.ma_sp,s.ten_sp,c.so_luong_nhap,c.don_gia_nhap,c.thanh_tien FROM chi_tiet_phieu_nhap c JOIN san_pham s ON c.ma_sp=s.ma_sp WHERE c.ma_phieu_nhap=@id ORDER BY c.ma_sp", MyConnection.P("@id",id));
  public DataTable SanPham() => MyConnection.ExecuteQuery("SELECT ma_sp,ten_sp,so_luong_ton FROM san_pham ORDER BY ten_sp");
  public DataTable NCC() => MyConnection.ExecuteQuery("SELECT ma_ncc,ten_ncc FROM nha_cung_cap ORDER BY ten_ncc");
  public DataTable NhanVien() => MyConnection.ExecuteQuery("SELECT ma_nv,ho_ten FROM nhan_vien ORDER BY ho_ten");
  public void Them(PhieuNhap p) {
   MyConnection.BeginTransaction();
   try {
    MyConnection.ExecuteNonQuery(@"INSERT INTO phieu_nhap (ma_phieu_nhap,ma_ncc,ma_nv,ngay_nhap,tong_tien_nhap) VALUES (@id,@ncc,@nv,@ngay,@tong)", MyConnection.P("@id",p.MaPhieuNhap),MyConnection.P("@ncc",p.MaNCC),MyConnection.P("@nv",p.MaNV),MyConnection.P("@ngay",p.NgayNhap),MyConnection.P("@tong",p.TongTienNhap));
    foreach(var c in p.ChiTiet) {
     MyConnection.ExecuteNonQuery(@"INSERT INTO chi_tiet_phieu_nhap (ma_phieu_nhap,ma_sp,so_luong_nhap,don_gia_nhap,thanh_tien) VALUES (@id,@sp,@sl,@gia,@tt)",MyConnection.P("@id",p.MaPhieuNhap),MyConnection.P("@sp",c.MaSP),MyConnection.P("@sl",c.SoLuongNhap),MyConnection.P("@gia",c.DonGiaNhap),MyConnection.P("@tt",c.ThanhTien));
     int affected=MyConnection.ExecuteNonQuery("UPDATE san_pham SET so_luong_ton=so_luong_ton+@sl WHERE ma_sp=@sp",MyConnection.P("@sl",c.SoLuongNhap),MyConnection.P("@sp",c.MaSP));
     if(affected!=1) throw new InvalidOperationException("Sản phẩm không tồn tại: "+c.MaSP);
    }
    MyConnection.Commit();
   } catch { MyConnection.Rollback(); throw; }
  }
 }
}
