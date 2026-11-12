using System;
using System.Collections;
using System.Data;
using QLKS.DTO;

namespace QLKS.DAO
{
    public class ChiTietDatPhongDAO
    {
        public ArrayList GetAll()
        {
            DataTable dt = MyConnection.ExecuteQuery(
                "SELECT c.ma_phieu, c.ma_phong, p.ten_phong, p.so_nguoi_toi_da, l.don_gia_ngay, l.don_gia_gio, " +
                "c.so_khach, c.gia_phong, c.ngay_gio_checkout, c.trang_thai, c.so_ngay_o, c.so_gio_o, c.thanh_tien " +
                "FROM chi_tiet_dat_phong c " +
                "JOIN phong p ON c.ma_phong = p.ma_phong " +
                "JOIN loai_phong l ON p.ma_loai = l.ma_loai " +
                "ORDER BY c.ma_phieu, c.ma_phong");
            var ds = new ArrayList();
            foreach (DataRow r in dt.Rows)
            {
                ds.Add(new ChiTietDatPhong
                {
                    MaPhieu = r["ma_phieu"].ToString(),
                    MaPhong = r["ma_phong"].ToString(),
                    TenPhong = r["ten_phong"].ToString(),
                    SoNguoiToiDa = Convert.ToInt32(r["so_nguoi_toi_da"]),
                    DonGiaNgay = Convert.ToDecimal(r["don_gia_ngay"]),
                    DonGiaGio = Convert.ToDecimal(r["don_gia_gio"]),
                    SoKhach = Convert.ToInt32(r["so_khach"]),
                    GiaPhong = Convert.ToDecimal(r["gia_phong"]),
                    NgayGioCheckout = r["ngay_gio_checkout"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["ngay_gio_checkout"]),
                    TrangThai = r["trang_thai"].ToString(),
                    SoNgayO = Convert.ToInt32(r["so_ngay_o"]),
                    SoGioO = Convert.ToInt32(r["so_gio_o"]),
                    ThanhTien = Convert.ToDecimal(r["thanh_tien"])
                });
            }
            return ds;
        }

        public bool Insert(ChiTietDatPhong ct)
        {
            // ngay_gio_checkout để NULL, trạng thái luôn bắt đầu là "Đang ở"
            return MyConnection.ExecuteNonQuery(
                "INSERT INTO chi_tiet_dat_phong (ma_phieu, ma_phong, so_khach, gia_phong, ngay_gio_checkout, trang_thai, so_ngay_o, so_gio_o, thanh_tien) " +
                "VALUES (@pd, @ph, @sk, @gia, NULL, 'Đang ở', @ngay, @gio, @tt)",
                MyConnection.P("@pd", ct.MaPhieu), MyConnection.P("@ph", ct.MaPhong), MyConnection.P("@sk", ct.SoKhach),
                MyConnection.P("@gia", ct.GiaPhong), MyConnection.P("@ngay", ct.SoNgayO),
                MyConnection.P("@gio", ct.SoGioO), MyConnection.P("@tt", ct.ThanhTien)) > 0;
        }

        public bool Update(ChiTietDatPhong ct)
        {
            // KHÔNG cập nhật trang_thai / ngay_gio_checkout: do thanh toán (TV3) đổi
            return MyConnection.ExecuteNonQuery(
                "UPDATE chi_tiet_dat_phong SET so_khach=@sk, gia_phong=@gia, so_ngay_o=@ngay, so_gio_o=@gio, thanh_tien=@tt " +
                "WHERE ma_phieu=@pd AND ma_phong=@ph",
                MyConnection.P("@pd", ct.MaPhieu), MyConnection.P("@ph", ct.MaPhong), MyConnection.P("@sk", ct.SoKhach),
                MyConnection.P("@gia", ct.GiaPhong), MyConnection.P("@ngay", ct.SoNgayO),
                MyConnection.P("@gio", ct.SoGioO), MyConnection.P("@tt", ct.ThanhTien)) > 0;
        }

        public bool Delete(string maPhieu, string maPhong)
        {
            // chi_tiet_khach_o của phòng này bị xóa theo (ON DELETE CASCADE)
            return MyConnection.ExecuteNonQuery(
                "DELETE FROM chi_tiet_dat_phong WHERE ma_phieu=@pd AND ma_phong=@ph",
                MyConnection.P("@pd", maPhieu), MyConnection.P("@ph", maPhong)) > 0;
        }

        /// <summary>
        /// Đếm số phiếu KHÁC đang giữ phòng này (trạng thái "Đang ở") mà khoảng [check-in, check-out] giao với [tu, den].
        /// Dùng để chặn đặt trùng phòng trong cùng khoảng thời gian.
        /// </summary>
        public int DemTrungThoiGian(string maPhong, string maPhieuBoQua, DateTime tu, DateTime den)
        {
            object kq = MyConnection.ExecuteScalar(
                "SELECT COUNT(*) FROM chi_tiet_dat_phong c JOIN phieu_dat_phong p ON c.ma_phieu = p.ma_phieu " +
                "WHERE c.ma_phong=@ph AND c.ma_phieu<>@pd AND c.trang_thai='Đang ở' " +
                "AND p.ngay_checkin < @den AND COALESCE(p.ngay_checkout, '9999-12-31') > @tu",
                MyConnection.P("@ph", maPhong), MyConnection.P("@pd", maPhieuBoQua),
                MyConnection.P("@tu", tu), MyConnection.P("@den", den));
            return Convert.ToInt32(kq);
        }
    }
}
