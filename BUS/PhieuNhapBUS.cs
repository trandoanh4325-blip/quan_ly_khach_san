using System; using System.Data; using System.Collections.Generic; using System.Linq; using QLKS.DAO; using QLKS.DTO;
namespace QLKS.BUS { public class PhieuNhapBUS {
 readonly PhieuNhapDAO dao=new PhieuNhapDAO();
 public DataTable LayDanhSach()=>dao.LayDanhSach();
 public DataTable ChiTiet(string id)=>dao.ChiTiet(id);
 public DataTable SanPham()=>dao.SanPham();
 public DataTable NCC()=>dao.NCC();
 public DataTable NhanVien()=>dao.NhanVien();
 public string SinhMa()=>MaTuDongDAO.Sinh("PN","phieu_nhap","ma_phieu_nhap");
 public bool Them(PhieuNhap p,out string loi) {
  loi=null;
  if(p==null || string.IsNullOrWhiteSpace(p.MaPhieuNhap) || p.MaPhieuNhap.Length>10 || string.IsNullOrWhiteSpace(p.MaNCC) || string.IsNullOrWhiteSpace(p.MaNV)) {loi="Thiếu mã phiếu, nhà cung cấp hoặc nhân viên.";return false;}
  if(p.ChiTiet==null||p.ChiTiet.Count==0) {loi="Phiếu nhập phải có ít nhất một sản phẩm.";return false;}
  if(p.ChiTiet.Any(c=>string.IsNullOrWhiteSpace(c.MaSP)||c.SoLuongNhap<=0||c.DonGiaNhap<0)) {loi="Chi tiết phiếu nhập không hợp lệ.";return false;}
  if(p.ChiTiet.Select(c=>c.MaSP).Distinct(StringComparer.OrdinalIgnoreCase).Count()!=p.ChiTiet.Count) {loi="Một sản phẩm không được lặp trong cùng phiếu.";return false;}
  try { checked {p.TongTienNhap=p.ChiTiet.Sum(c=>c.ThanhTien);} if (p.TongTienNhap>999999999999999m) {loi="Tổng tiền vượt giới hạn CSDL.";return false;} dao.Them(p);return true; }
  catch(Exception ex) {loi=MyConnection.ThongBaoLoi(ex);return false;}
 }
} }
