using System;
using System.Collections;
using System.Data;
using QLKS.DTO;

namespace QLKS.DAO
{
    public class PhuThuDAO
    {
        public ArrayList GetAll()
        {
            var ds = new ArrayList();
            foreach (DataRow r in MyConnection.ExecuteQuery(
                "SELECT p.*,s.ten_sp,s.don_vi_tinh,s.gia_ban FROM phu_thu p JOIN san_pham s ON s.ma_sp=p.ma_sp ORDER BY p.ma_phu_thu").Rows)
                ds.Add(new PhuThu { MaPhuThu = r["ma_phu_thu"].ToString(), TenPhuThu = r["ten_phu_thu"].ToString(),
                    MaSP = r["ma_sp"].ToString(), TenSP = r["ten_sp"].ToString(), DonViTinh = r["don_vi_tinh"].ToString(), DonGia = Convert.ToDecimal(r["gia_ban"]) });
            return ds;
        }

        public ArrayList SanPham()
        {
            var ds = new ArrayList();
            foreach (DataRow r in MyConnection.ExecuteQuery("SELECT ma_sp,ten_sp,don_vi_tinh,gia_ban FROM san_pham ORDER BY ma_sp").Rows)
                ds.Add(new SanPhamPhuThu { MaSP = r["ma_sp"].ToString(), TenSP = r["ten_sp"].ToString(),
                    DonViTinh = r["don_vi_tinh"].ToString(), GiaBan = Convert.ToDecimal(r["gia_ban"]) });
            return ds;
        }
        public string SinhMa() { return MaTuDongDAO.Sinh("PT", "phu_thu", "ma_phu_thu"); }
        public bool Insert(PhuThu p)
        {
            return MyConnection.ExecuteNonQuery("INSERT INTO phu_thu (ma_phu_thu,ten_phu_thu,ma_sp) VALUES (@ma,@ten,@sp)",
                MyConnection.P("@ma", p.MaPhuThu), MyConnection.P("@ten", p.TenPhuThu), MyConnection.P("@sp", p.MaSP)) == 1;
        }
        public bool Update(PhuThu p)
        {
            return MyConnection.ExecuteNonQuery("UPDATE phu_thu SET ten_phu_thu=@ten,ma_sp=@sp WHERE ma_phu_thu=@ma",
                MyConnection.P("@ma", p.MaPhuThu), MyConnection.P("@ten", p.TenPhuThu), MyConnection.P("@sp", p.MaSP)) > 0;
        }
        public bool Delete(string ma)
        {
            return MyConnection.ExecuteNonQuery("DELETE FROM phu_thu WHERE ma_phu_thu=@ma", MyConnection.P("@ma", ma)) == 1;
        }
    }
}
