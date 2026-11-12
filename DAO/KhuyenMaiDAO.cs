using System;
using System.Collections;
using System.Data;
using QLKS.DTO;

namespace QLKS.DAO
{
    public class KhuyenMaiDAO
    {
        public ArrayList GetAll()
        {
            var ds = new ArrayList();
            foreach (DataRow r in MyConnection.ExecuteQuery("SELECT * FROM khuyen_mai ORDER BY ma_km").Rows)
                ds.Add(new KhuyenMai { MaKM = r["ma_km"].ToString(), TenKM = r["ten_km"].ToString(), LoaiGiam = r["loai_giam"].ToString(),
                    GiaTriGiam = Convert.ToDecimal(r["gia_tri_giam"]), NgayBatDau = Convert.ToDateTime(r["ngay_bat_dau"]),
                    NgayKetThuc = Convert.ToDateTime(r["ngay_ket_thuc"]), TongThuToiThieu = Convert.ToDecimal(r["tong_thu_toi_thieu"]),
                    SoLuotToiToiThieu = Convert.ToInt32(r["so_luot_toi_toi_thieu"]) });
            return ds;
        }
        public string SinhMa() { return MaTuDongDAO.Sinh("KM", "khuyen_mai", "ma_km"); }
        public bool Luu(KhuyenMai k, bool them)
        {
            string sql = them
                ? "INSERT INTO khuyen_mai (ma_km,ten_km,loai_giam,gia_tri_giam,ngay_bat_dau,ngay_ket_thuc,tong_thu_toi_thieu,so_luot_toi_toi_thieu) VALUES (@ma,@ten,@loai,@giam,@tu,@den,@thu,@luot)"
                : "UPDATE khuyen_mai SET ten_km=@ten,loai_giam=@loai,gia_tri_giam=@giam,ngay_bat_dau=@tu,ngay_ket_thuc=@den,tong_thu_toi_thieu=@thu,so_luot_toi_toi_thieu=@luot WHERE ma_km=@ma";
            return MyConnection.ExecuteNonQuery(sql, MyConnection.P("@ma", k.MaKM), MyConnection.P("@ten", k.TenKM),
                MyConnection.P("@loai", k.LoaiGiam), MyConnection.P("@giam", k.GiaTriGiam), MyConnection.P("@tu", k.NgayBatDau.Date),
                MyConnection.P("@den", k.NgayKetThuc.Date), MyConnection.P("@thu", k.TongThuToiThieu), MyConnection.P("@luot", k.SoLuotToiToiThieu)) > 0;
        }
        public bool Delete(string ma)
        {
            return MyConnection.ExecuteNonQuery("DELETE FROM khuyen_mai WHERE ma_km=@ma", MyConnection.P("@ma", ma)) == 1;
        }
    }
}
