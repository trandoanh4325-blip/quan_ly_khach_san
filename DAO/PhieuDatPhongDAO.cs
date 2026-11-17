using System;
using System.Collections;
using System.Data;
using QLKS.DTO;

namespace QLKS.DAO
{
    public class PhieuDatPhongDAO
    {
        public ArrayList GetAll()
        {
            DataTable dt = MyConnection.ExecuteQuery(
                "SELECT p.ma_phieu, p.ma_kh, k.ho_ten AS ten_kh, p.ma_nv, n.ho_ten AS ten_nv, " +
                "p.ngay_tao, p.ngay_checkin, p.ngay_checkout, p.tien_coc, p.trang_thai_coc, " +
                "p.so_ngay_o_du_kien, p.o_theo_gio, p.so_gio_nghi, p.tong_tien_du_kien, " +
                "EXISTS(SELECT 1 FROM hoa_don h WHERE h.ma_phieu = p.ma_phieu) AS da_co_hd " +
                "FROM phieu_dat_phong p " +
                "JOIN khach_hang k ON p.ma_kh = k.ma_kh " +
                "JOIN nhan_vien n ON p.ma_nv = n.ma_nv " +
                "ORDER BY p.ma_phieu");
            var ds = new ArrayList();
            foreach (DataRow r in dt.Rows)
            {
                ds.Add(new PhieuDatPhong
                {
                    MaPhieu = r["ma_phieu"].ToString(),
                    MaKH = r["ma_kh"].ToString(),
                    TenKH = r["ten_kh"].ToString(),
                    MaNV = r["ma_nv"].ToString(),
                    TenNV = r["ten_nv"].ToString(),
                    NgayTao = Convert.ToDateTime(r["ngay_tao"]),
                    NgayCheckin = Convert.ToDateTime(r["ngay_checkin"]),
                    NgayCheckout = r["ngay_checkout"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["ngay_checkout"]),
                    TienCoc = Convert.ToDecimal(r["tien_coc"]),
                    TrangThaiCoc = r["trang_thai_coc"].ToString(),
                    SoNgayODuKien = Convert.ToInt32(r["so_ngay_o_du_kien"]),
                    OTheoGio = Convert.ToInt32(r["o_theo_gio"]),
                    SoGioNghi = Convert.ToInt32(r["so_gio_nghi"]),
                    TongTienDuKien = Convert.ToDecimal(r["tong_tien_du_kien"]),
                    TinhTrang = Convert.ToInt32(r["da_co_hd"]) > 0 ? PhieuDatPhong.DaThanhToan : PhieuDatPhong.ChuaThanhToan
                });
            }
            return ds;
        }

        public bool Insert(PhieuDatPhong p)
        {
            // tong_tien_du_kien luôn bắt đầu từ 0, chỉ đổi khi thêm/sửa/xóa chi tiết đặt phòng
            return MyConnection.ExecuteNonQuery(
                "INSERT INTO phieu_dat_phong (ma_phieu, ma_kh, ma_nv, ngay_tao, ngay_checkin, ngay_checkout, tien_coc, " +
                "trang_thai_coc, so_ngay_o_du_kien, o_theo_gio, so_gio_nghi, tong_tien_du_kien) " +
                "VALUES (@ma, @kh, @nv, @tao, @ci, @co, @coc, @ttcoc, @ngay, @gio, @sogio, 0)",
                MyConnection.P("@ma", p.MaPhieu), MyConnection.P("@kh", p.MaKH), MyConnection.P("@nv", p.MaNV),
                MyConnection.P("@tao", p.NgayTao), MyConnection.P("@ci", p.NgayCheckin), MyConnection.P("@co", p.NgayCheckout),
                MyConnection.P("@coc", p.TienCoc), MyConnection.P("@ttcoc", p.TrangThaiCoc),
                MyConnection.P("@ngay", p.SoNgayODuKien), MyConnection.P("@gio", p.OTheoGio),
                MyConnection.P("@sogio", p.SoGioNghi)) > 0;
        }

        public bool Update(PhieuDatPhong p)
        {
            // KHÔNG cập nhật ngay_tao và tong_tien_du_kien (tổng do CapNhatTongTien tính từ chi tiết)
            return MyConnection.ExecuteNonQuery(
                "UPDATE phieu_dat_phong SET ma_kh=@kh, ma_nv=@nv, ngay_checkin=@ci, ngay_checkout=@co, tien_coc=@coc, " +
                "trang_thai_coc=@ttcoc, so_ngay_o_du_kien=@ngay, o_theo_gio=@gio, so_gio_nghi=@sogio WHERE ma_phieu=@ma",
                MyConnection.P("@ma", p.MaPhieu), MyConnection.P("@kh", p.MaKH), MyConnection.P("@nv", p.MaNV),
                MyConnection.P("@ci", p.NgayCheckin), MyConnection.P("@co", p.NgayCheckout),
                MyConnection.P("@coc", p.TienCoc), MyConnection.P("@ttcoc", p.TrangThaiCoc),
                MyConnection.P("@ngay", p.SoNgayODuKien), MyConnection.P("@gio", p.OTheoGio),
                MyConnection.P("@sogio", p.SoGioNghi)) > 0;
        }

        public bool Delete(string ma)
        {
            // chi_tiet_dat_phong và chi_tiet_khach_o bị xóa theo (ON DELETE CASCADE)
            return MyConnection.ExecuteNonQuery("DELETE FROM phieu_dat_phong WHERE ma_phieu=@ma",
                MyConnection.P("@ma", ma)) > 0;
        }

        public string SinhMa()
        {
            return MaTuDongDAO.Sinh("PD", "phieu_dat_phong", "ma_phieu");
        }

        /// <summary>Tổng tiền dự kiến của phiếu = tổng thành tiền các phòng trong phiếu.</summary>
        public void CapNhatTongTien(string maPhieu)
        {
            MyConnection.ExecuteNonQuery(
                "UPDATE phieu_dat_phong SET tong_tien_du_kien = " +
                "COALESCE((SELECT SUM(thanh_tien) FROM chi_tiet_dat_phong WHERE ma_phieu=@m), 0) WHERE ma_phieu=@m",
                MyConnection.P("@m", maPhieu));
        }
    }
}
