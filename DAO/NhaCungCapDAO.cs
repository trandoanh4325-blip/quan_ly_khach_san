using System.Data; using QLKS.DTO;
namespace QLKS.DAO { public class NhaCungCapDAO {
 public DataTable LayDanhSach() => MyConnection.ExecuteQuery("SELECT ma_ncc, ten_ncc, so_dien_thoai, dia_chi FROM nha_cung_cap ORDER BY ma_ncc");
 public int Them(NhaCungCap o) => MyConnection.ExecuteNonQuery("INSERT INTO nha_cung_cap (ma_ncc, ten_ncc, so_dien_thoai, dia_chi) VALUES (@ma_ncc, @ten_ncc, @so_dien_thoai, @dia_chi)", MyConnection.P("@ma_ncc", o.MaNCC),
                MyConnection.P("@ten_ncc", o.TenNCC),
                MyConnection.P("@so_dien_thoai", o.SoDienThoai),
                MyConnection.P("@dia_chi", o.DiaChi));
 public int Sua(NhaCungCap o) => MyConnection.ExecuteNonQuery("UPDATE nha_cung_cap SET ten_ncc=@ten_ncc, so_dien_thoai=@so_dien_thoai, dia_chi=@dia_chi WHERE ma_ncc=@id", MyConnection.P("@ten_ncc", o.TenNCC),
                MyConnection.P("@so_dien_thoai", o.SoDienThoai),
                MyConnection.P("@dia_chi", o.DiaChi), MyConnection.P("@id",o.MaNCC));
 public int Xoa(string id) => MyConnection.ExecuteNonQuery("DELETE FROM nha_cung_cap WHERE ma_ncc=@id", MyConnection.P("@id",id));
 } }
