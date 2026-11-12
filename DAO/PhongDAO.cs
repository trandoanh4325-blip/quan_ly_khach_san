using System;
using System.Collections;
using System.Data;
using QLKS.DTO;

namespace QLKS.DAO
{
    public class PhongDAO
    {
        public ArrayList GetAll()
        {
            DataTable dt = MyConnection.ExecuteQuery(
                "SELECT p.ma_phong, p.ten_phong, p.ma_loai, l.ten_loai, p.trang_thai, p.so_nguoi_toi_da " +
                "FROM phong p JOIN loai_phong l ON p.ma_loai = l.ma_loai ORDER BY p.ma_phong");
            var ds = new ArrayList();
            foreach (DataRow r in dt.Rows)
            {
                ds.Add(new Phong
                {
                    MaPhong = r["ma_phong"].ToString(),
                    TenPhong = r["ten_phong"].ToString(),
                    MaLoai = r["ma_loai"].ToString(),
                    TenLoai = r["ten_loai"].ToString(),
                    TrangThai = r["trang_thai"].ToString(),
                    SoNguoiToiDa = Convert.ToInt32(r["so_nguoi_toi_da"])
                });
            }
            return ds;
        }

        

        public bool Update(Phong p)
        {
            // KHÔNG cập nhật trang_thai: trạng thái chỉ đổi qua DoiTrangThai (check-in / thanh toán)
            return MyConnection.ExecuteNonQuery(
                "UPDATE phong SET ten_phong=@ten, ma_loai=@loai, so_nguoi_toi_da=@n WHERE ma_phong=@ma",
                MyConnection.P("@ma", p.MaPhong), MyConnection.P("@ten", p.TenPhong),
                MyConnection.P("@loai", p.MaLoai), MyConnection.P("@n", p.SoNguoiToiDa)) > 0;
        }

        public bool Delete(string ma)
        {
            return MyConnection.ExecuteNonQuery("DELETE FROM phong WHERE ma_phong=@ma",
                MyConnection.P("@ma", ma)) > 0;
        }

        public string SinhMa()
        {
            return MaTuDongDAO.Sinh("P", "phong", "ma_phong");
        }

        /// <summary>
        /// Tính lại tong_so_phong và so_luong_dang_co (= số phòng Trống) của 1 loại phòng.
        /// Gọi sau mọi thay đổi làm đổi số phòng hoặc trạng thái phòng.
        /// </summary>
        public void CapNhatLoaiPhong(string maLoai)
        {
            MyConnection.ExecuteNonQuery(
                "UPDATE loai_phong SET " +
                "tong_so_phong = (SELECT COUNT(*) FROM phong WHERE ma_loai=@m), " +
                "so_luong_dang_co = (SELECT COUNT(*) FROM phong WHERE ma_loai=@m AND trang_thai='Trống') " +
                "WHERE ma_loai=@m",
                MyConnection.P("@m", maLoai));
        }

        /// <summary>
        /// Dùng cho check-in ("Đã đủ") và sau thanh toán ("Trống"): đổi trạng thái phòng
        /// rồi tự cập nhật "số lượng đang có" của loại phòng. Nên gọi trong BeginTransaction/Commit.
        /// </summary>
        public void DoiTrangThai(string maPhong, string trangThai)
        {
            MyConnection.ExecuteNonQuery("UPDATE phong SET trang_thai=@tt WHERE ma_phong=@ma",
                MyConnection.P("@tt", trangThai), MyConnection.P("@ma", maPhong));
            object loai = MyConnection.ExecuteScalar("SELECT ma_loai FROM phong WHERE ma_phong=@ma",
                MyConnection.P("@ma", maPhong));
            if (loai != null && loai != DBNull.Value) CapNhatLoaiPhong(loai.ToString());
        }
    }
}
