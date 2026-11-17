using System; using System.Collections; using System.Data; using System.Linq; using QLKS.DAO; using QLKS.DTO;
namespace QLKS.BUS { public class LoaiSanPhamBUS {
 readonly LoaiSanPhamDAO dao = new LoaiSanPhamDAO();
 readonly ArrayList ds = new ArrayList();
 public ArrayList DanhSach { get { if (!daTai) TaiDuLieu(); return ds; } }
 bool daTai;
 public void TaiDuLieu() { ds.Clear(); foreach (DataRow r in dao.LayDanhSach().Rows) ds.Add(new LoaiSanPham { MaLoaiSP = System.Convert.ToString(r["ma_loai_sp"]), TenLoaiSP = System.Convert.ToString(r["ten_loai_sp"]) }); daTai=true; }
 public ArrayList TimKiem(string key) { var ketQua=new ArrayList(); key=(key??"").Trim(); foreach (LoaiSanPham o in DanhSach) if (new[] {o.MaLoaiSP,o.TenLoaiSP}.Any(v => (v??"").IndexOf(key,StringComparison.OrdinalIgnoreCase)>=0)) ketQua.Add(o); return ketQua; }
 string KiemTra(LoaiSanPham o) { if(o == null || string.IsNullOrWhiteSpace(o.MaLoaiSP) || string.IsNullOrWhiteSpace(o.TenLoaiSP)) return "Mã và tên không được để trống."; if ((o.MaLoaiSP ?? "").Length > 10 || (o.TenLoaiSP ?? "").Length > 100) return "Thông tin dài hơn giới hạn của CSDL.";  return null; }
 public bool Luu(LoaiSanPham o, bool them, out string loi) { loi=KiemTra(o); if(loi!=null) return false; try { if(them) dao.Them(o); else if(dao.Sua(o)==0) {loi="Không tìm thấy mã cần sửa.";return false;} TaiDuLieu(); return true; } catch(Exception ex) {loi=MyConnection.ThongBaoLoi(ex);return false;} }
 public bool Xoa(string id,out string loi) {loi=null;try { if(dao.Xoa(id)==0) {loi="Không tìm thấy mã cần xóa.";return false;} TaiDuLieu();return true; }catch(Exception ex) {loi=MyConnection.ThongBaoLoi(ex);return false;}}
 } }
