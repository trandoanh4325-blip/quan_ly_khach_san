using System;
using System.Collections;
using System.Data;
using QLKS.DTO;

namespace QLKS.DAO
{
    public class LoaiPhongDAO
    {
        public ArrayList GetAll()
        {
            DataTable dt = MyConnection.ExecuteQuery(
                "SELECT ma_loai, ten_loai, don_gia_ngay, don_gia_gio, so_luong_dang_co, tong_so_phong " +
                "FROM loai_phong ORDER BY ma_loai");
            var ds = new ArrayList();
            foreach (DataRow r in dt.Rows)
            {
                ds.Add(new LoaiPhong
                {
                    MaLoai = r["ma_loai"].ToString(),
                    TenLoai = r["ten_loai"].ToString(),
                    DonGiaNgay = Convert.ToDecimal(r["don_gia_ngay"]),
                    DonGiaGio = Convert.ToDecimal(r["don_gia_gio"]),
                    SoLuongDangCo = Convert.ToInt32(r["so_luong_dang_co"]),
                    TongSoPhong = Convert.ToInt32(r["tong_so_phong"])
                });
            }
            return ds;
        }

        public bool Insert(LoaiPhong lp)
        {
            // so_luong_dang_co và tong_so_phong luôn bắt đầu từ 0, chỉ đổi khi thêm/xóa/đổi trạng thái phòng
            return MyConnection.ExecuteNonQuery(
                "INSERT INTO loai_phong (ma_loai, ten_loai, don_gia_ngay, don_gia_gio, so_luong_dang_co, tong_so_phong) " +
                "VALUES (@ma, @ten, @ngay, @gio, 0, 0)",
                MyConnection.P("@ma", lp.MaLoai), MyConnection.P("@ten", lp.TenLoai),
                MyConnection.P("@ngay", lp.DonGiaNgay), MyConnection.P("@gio", lp.DonGiaGio)) > 0;
        }

        public bool Update(LoaiPhong lp)
        {
            // KHÔNG cập nhật so_luong_dang_co / tong_so_phong: hai field này không được sửa tay
            return MyConnection.ExecuteNonQuery(
                "UPDATE loai_phong SET ten_loai=@ten, don_gia_ngay=@ngay, don_gia_gio=@gio WHERE ma_loai=@ma",
                MyConnection.P("@ma", lp.MaLoai), MyConnection.P("@ten", lp.TenLoai),
                MyConnection.P("@ngay", lp.DonGiaNgay), MyConnection.P("@gio", lp.DonGiaGio)) > 0;
        }

        public bool Delete(string ma)
        {
            return MyConnection.ExecuteNonQuery("DELETE FROM loai_phong WHERE ma_loai=@ma",
                MyConnection.P("@ma", ma)) > 0;
        }

        public string SinhMa()
        {
            return MaTuDongDAO.Sinh("LP", "loai_phong", "ma_loai");
        }
    }
}
