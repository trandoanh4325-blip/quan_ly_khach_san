using System;
using System.Collections;
using System.Data;
using QLKS.DTO;

namespace QLKS.DAO
{
    public class DichVuDAO
    {
        public ArrayList GetAll()
        {
            var ds = new ArrayList();
            foreach (DataRow r in MyConnection.ExecuteQuery("SELECT * FROM dich_vu ORDER BY ma_dv").Rows)
                ds.Add(new DichVu { MaDV = r["ma_dv"].ToString(), TenDV = r["ten_dv"].ToString(),
                    GiaTien = Convert.ToDecimal(r["gia_tien"]), DonViTinh = r["don_vi_tinh"].ToString() });
            return ds;
        }

        public string SinhMa() { return MaTuDongDAO.Sinh("DV", "dich_vu", "ma_dv"); }

        public bool Insert(DichVu d)
        {
            return MyConnection.ExecuteNonQuery(
                "INSERT INTO dich_vu VALUES (@ma,@ten,@gia,@dv)", MyConnection.P("@ma", d.MaDV),
                MyConnection.P("@ten", d.TenDV), MyConnection.P("@gia", d.GiaTien), MyConnection.P("@dv", d.DonViTinh)) == 1;
        }

        public bool Update(DichVu d)
        {
            return MyConnection.ExecuteNonQuery(
                "UPDATE dich_vu SET ten_dv=@ten,gia_tien=@gia,don_vi_tinh=@dv WHERE ma_dv=@ma",
                MyConnection.P("@ma", d.MaDV), MyConnection.P("@ten", d.TenDV),
                MyConnection.P("@gia", d.GiaTien), MyConnection.P("@dv", d.DonViTinh)) > 0;
        }

        public bool Delete(string ma)
        {
            return MyConnection.ExecuteNonQuery("DELETE FROM dich_vu WHERE ma_dv=@ma", MyConnection.P("@ma", ma)) == 1;
        }
    }
}
