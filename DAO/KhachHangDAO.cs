using System;
using System.Collections;
using System.Data;
using QLKS.DTO;

namespace QLKS.DAO
{
    public class KhachHangDAO
    {
        public ArrayList GetAll()
        {
            DataTable dt = MyConnection.ExecuteQuery(
                "SELECT ma_kh, ho_ten, cccd, gioi_tinh, tong_thu, so_luot_toi FROM khach_hang ORDER BY ma_kh");
            var ds = new ArrayList();
            foreach (DataRow r in dt.Rows)
            {
                ds.Add(new KhachHang
                {
                    MaKH = r["ma_kh"].ToString(),
                    HoTen = r["ho_ten"].ToString(),
                    CCCD = r["cccd"].ToString(),
                    GioiTinh = r["gioi_tinh"].ToString(),
                    TongThu = Convert.ToDecimal(r["tong_thu"]),
                    SoLuotToi = Convert.ToInt32(r["so_luot_toi"])
                });
            }
            return ds;
        }

        public bool Insert(KhachHang kh)
        {
            // tong_thu và so_luot_toi luôn bắt đầu từ 0, chỉ thay đổi sau khi thanh toán hóa đơn
            return MyConnection.ExecuteNonQuery(
                "INSERT INTO khach_hang (ma_kh, ho_ten, cccd, gioi_tinh, tong_thu, so_luot_toi) VALUES (@ma, @ten, @cccd, @gt, 0, 0)",
                MyConnection.P("@ma", kh.MaKH), MyConnection.P("@ten", kh.HoTen),
                MyConnection.P("@cccd", kh.CCCD), MyConnection.P("@gt", kh.GioiTinh)) > 0;
        }

        public bool Update(KhachHang kh)
        {
            // KHÔNG cập nhật tong_thu / so_luot_toi: hai field này không được sửa tay
            return MyConnection.ExecuteNonQuery(
                "UPDATE khach_hang SET ho_ten=@ten, cccd=@cccd, gioi_tinh=@gt WHERE ma_kh=@ma",
                MyConnection.P("@ma", kh.MaKH), MyConnection.P("@ten", kh.HoTen),
                MyConnection.P("@cccd", kh.CCCD), MyConnection.P("@gt", kh.GioiTinh)) > 0;
        }

        public bool Delete(string ma)
        {
            return MyConnection.ExecuteNonQuery("DELETE FROM khach_hang WHERE ma_kh=@ma",
                MyConnection.P("@ma", ma)) > 0;
        }

        public string SinhMa()
        {
            return MaTuDongDAO.Sinh("KH", "khach_hang", "ma_kh");
        }
    }
}
