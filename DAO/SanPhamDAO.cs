
using System.Data;
using MySql.Data.MySqlClient;
using QLKS.DTO;

namespace QLKS.DAO
{
    public class SanPhamDAO
    {
        // Lấy toàn bộ sản phẩm
        public DataTable LayDanhSach()
        {
            string sql = @"SELECT ma_sp, ten_sp, ma_loai_sp,
                                  don_vi_tinh, gia_ban, so_luong_ton
                           FROM san_pham
                           ORDER BY ma_sp";

            return MyConnection.ExecuteQuery(sql);
        }

        // Nạp danh mục loại sản phẩm cho ComboBox (không phải tìm kiếm).
        public DataTable LayLoaiSanPham()
        {
            return MyConnection.ExecuteQuery(
                "SELECT ma_loai_sp, ten_loai_sp FROM loai_san_pham ORDER BY ten_loai_sp");
        }

        // Thêm sản phẩm
        public int Them(SanPham sp)
        {
            string sql = @"INSERT INTO san_pham
                           (ma_sp, ten_sp, ma_loai_sp,
                            don_vi_tinh, gia_ban, so_luong_ton)
                           VALUES
                           (@ma, @ten, @loai, @dvt, @gia, 0)";

            return MyConnection.ExecuteNonQuery(sql,
                MyConnection.P("@ma", sp.MaSP),
                MyConnection.P("@ten", sp.TenSP),
                MyConnection.P("@loai", sp.MaLoaiSP),
                MyConnection.P("@dvt", sp.DonViTinh),
                MyConnection.P("@gia", sp.GiaBan));
        }

        // Sửa thông tin sản phẩm, không sửa tồn kho
        public int Sua(SanPham sp)
        {
            string sql = @"UPDATE san_pham
                           SET ten_sp = @ten,
                               ma_loai_sp = @loai,
                               don_vi_tinh = @dvt,
                               gia_ban = @gia
                           WHERE ma_sp = @ma";

            return MyConnection.ExecuteNonQuery(sql,
                MyConnection.P("@ma", sp.MaSP),
                MyConnection.P("@ten", sp.TenSP),
                MyConnection.P("@loai", sp.MaLoaiSP),
                MyConnection.P("@dvt", sp.DonViTinh),
                MyConnection.P("@gia", sp.GiaBan));
        }

        // Xóa sản phẩm
        public int Xoa(string maSP)
        {
            string sql = "DELETE FROM san_pham WHERE ma_sp = @ma";

            return MyConnection.ExecuteNonQuery(sql,
                MyConnection.P("@ma", maSP));
        }
    }
}
