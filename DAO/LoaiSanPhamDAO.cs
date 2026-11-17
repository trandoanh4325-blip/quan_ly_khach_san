using System.Data; using QLKS.DTO;
namespace QLKS.DAO { public class LoaiSanPhamDAO {
 public DataTable LayDanhSach() => MyConnection.ExecuteQuery("SELECT ma_loai_sp, ten_loai_sp FROM loai_san_pham ORDER BY ma_loai_sp");
 public int Them(LoaiSanPham o) => MyConnection.ExecuteNonQuery("INSERT INTO loai_san_pham (ma_loai_sp, ten_loai_sp) VALUES (@ma_loai_sp, @ten_loai_sp)", MyConnection.P("@ma_loai_sp", o.MaLoaiSP),
                MyConnection.P("@ten_loai_sp", o.TenLoaiSP));
 public int Sua(LoaiSanPham o) => MyConnection.ExecuteNonQuery("UPDATE loai_san_pham SET ten_loai_sp=@ten_loai_sp WHERE ma_loai_sp=@id", MyConnection.P("@ten_loai_sp", o.TenLoaiSP), MyConnection.P("@id",o.MaLoaiSP));
 public int Xoa(string id) => MyConnection.ExecuteNonQuery("DELETE FROM loai_san_pham WHERE ma_loai_sp=@id", MyConnection.P("@id",id));
 } }
