using System;
using System.Collections;
using System.Data;
using QLKS.DTO;

namespace QLKS.DAO
{
    public class ChiTietKhachODAO
    {
        public ArrayList GetAll()
        {
            DataTable dt = MyConnection.ExecuteQuery(
                "SELECT ma_khach_o, ma_phieu, ma_phong, ho_ten, cccd FROM chi_tiet_khach_o ORDER BY ma_phieu, ma_phong, ma_khach_o");
            var ds = new ArrayList();
            foreach (DataRow r in dt.Rows)
            {
                ds.Add(new ChiTietKhachO
                {
                    MaKhachO = r["ma_khach_o"].ToString(),
                    MaPhieu = r["ma_phieu"].ToString(),
                    MaPhong = r["ma_phong"].ToString(),
                    HoTen = r["ho_ten"].ToString(),
                    CCCD = r["cccd"].ToString()
                });
            }
            return ds;
        }

        public bool Insert(ChiTietKhachO k)
        {
            return MyConnection.ExecuteNonQuery(
                "INSERT INTO chi_tiet_khach_o (ma_khach_o, ma_phieu, ma_phong, ho_ten, cccd) VALUES (@ma, @pd, @ph, @ten, @cccd)",
                MyConnection.P("@ma", k.MaKhachO), MyConnection.P("@pd", k.MaPhieu), MyConnection.P("@ph", k.MaPhong),
                MyConnection.P("@ten", k.HoTen), MyConnection.P("@cccd", k.CCCD)) > 0;
        }

        /// <summary>Xóa toàn bộ khách của 1 phòng trong 1 phiếu (dùng khi sửa danh sách khách).</summary>
        public void XoaTheoPhong(string maPhieu, string maPhong)
        {
            MyConnection.ExecuteNonQuery("DELETE FROM chi_tiet_khach_o WHERE ma_phieu=@pd AND ma_phong=@ph",
                MyConnection.P("@pd", maPhieu), MyConnection.P("@ph", maPhong));
        }

        public string SinhMa()
        {
            return MaTuDongDAO.Sinh("KO", "chi_tiet_khach_o", "ma_khach_o", 4);
        }

        /// <summary>
        /// Số lần CCCD này đang ở trong phòng "Đang ở" của phiếu KHÁC.
        /// Một CCCD không được đang ở hai phòng cùng lúc; người đã checkout ở đợt sau vẫn hợp lệ.
        /// </summary>
        public int DemDangO(string cccd, string maPhieuBoQua)
        {
            object kq = MyConnection.ExecuteScalar(
                "SELECT COUNT(*) FROM chi_tiet_khach_o k " +
                "JOIN chi_tiet_dat_phong c ON c.ma_phieu = k.ma_phieu AND c.ma_phong = k.ma_phong " +
                "WHERE k.cccd=@c AND c.trang_thai='Đang ở' AND k.ma_phieu<>@pd",
                MyConnection.P("@c", cccd), MyConnection.P("@pd", maPhieuBoQua));
            return Convert.ToInt32(kq);
        }
    }
}
