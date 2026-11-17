using System; using System.Collections; using System.Data; using System.Linq; using QLKS.DAO; using QLKS.DTO;
namespace QLKS.BUS { public class NhaCungCapBUS {
 readonly NhaCungCapDAO dao = new NhaCungCapDAO();
 readonly ArrayList ds = new ArrayList();
 public ArrayList DanhSach { get { if (!daTai) TaiDuLieu(); return ds; } }
 bool daTai;
 public void TaiDuLieu() { ds.Clear(); foreach (DataRow r in dao.LayDanhSach().Rows) ds.Add(new NhaCungCap { MaNCC = System.Convert.ToString(r["ma_ncc"]), TenNCC = System.Convert.ToString(r["ten_ncc"]), SoDienThoai = System.Convert.ToString(r["so_dien_thoai"]), DiaChi = System.Convert.ToString(r["dia_chi"]) }); daTai=true; }
 public ArrayList TimKiem(string key) { var ketQua=new ArrayList(); key=(key??"").Trim(); foreach (NhaCungCap o in DanhSach) if (new[] {o.MaNCC,o.TenNCC,o.SoDienThoai,o.DiaChi}.Any(v => (v??"").IndexOf(key,StringComparison.OrdinalIgnoreCase)>=0)) ketQua.Add(o); return ketQua; }
 string KiemTra(NhaCungCap o) { if(o == null || string.IsNullOrWhiteSpace(o.MaNCC) || string.IsNullOrWhiteSpace(o.TenNCC)) return "Mã và tên không được để trống."; if ((o.MaNCC ?? "").Length > 10 || (o.TenNCC ?? "").Length > 150 || (o.SoDienThoai ?? "").Length > 15 || (o.DiaChi ?? "").Length > 255) return "Thông tin dài hơn giới hạn của CSDL."; if (string.IsNullOrWhiteSpace(o.SoDienThoai) || o.SoDienThoai.Any(c => !char.IsDigit(c)) || o.SoDienThoai.Length < 9) return "Số điện thoại phải có 9-15 chữ số."; return null; }
 public bool Luu(NhaCungCap o, bool them, out string loi) { loi=KiemTra(o); if(loi!=null) return false; try { if(them) dao.Them(o); else if(dao.Sua(o)==0) {loi="Không tìm thấy mã cần sửa.";return false;} TaiDuLieu(); return true; } catch(Exception ex) {loi=MyConnection.ThongBaoLoi(ex);return false;} }
 public bool Xoa(string id,out string loi) {loi=null;try { if(dao.Xoa(id)==0) {loi="Không tìm thấy mã cần xóa.";return false;} TaiDuLieu();return true; }catch(Exception ex) {loi=MyConnection.ThongBaoLoi(ex);return false;}}
 } }
