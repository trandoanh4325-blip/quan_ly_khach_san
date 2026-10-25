using System.Collections;
using System.Data;
using QLKS.DTO;

namespace QLKS.DAO
{
    public class NhanVienDAO
    {
        public ArrayList GetAll()
        {
            DataTable dt = MyConnection.ExecuteQuery(
                "SELECT ma_nv, ho_ten, chuc_vu, so_dien_thoai FROM nhan_vien ORDER BY ma_nv");
            var ds = new ArrayList();
            foreach (DataRow r in dt.Rows)
            {
                ds.Add(new NhanVien
                {
                    MaNV = r["ma_nv"].ToString(),
                    HoTen = r["ho_ten"].ToString(),
                    ChucVu = r["chuc_vu"].ToString(),
                    SoDienThoai = r["so_dien_thoai"].ToString()
                });
            }
            return ds;
        }

        public bool Insert(NhanVien nv)
        {
            return MyConnection.ExecuteNonQuery(
                "INSERT INTO nhan_vien (ma_nv, ho_ten, chuc_vu, so_dien_thoai) VALUES (@ma, @ten, @cv, @sdt)",
                MyConnection.P("@ma", nv.MaNV), MyConnection.P("@ten", nv.HoTen),
                MyConnection.P("@cv", nv.ChucVu), MyConnection.P("@sdt", nv.SoDienThoai)) > 0;
        }

        public bool Update(NhanVien nv)
        {
            return MyConnection.ExecuteNonQuery(
                "UPDATE nhan_vien SET ho_ten=@ten, chuc_vu=@cv, so_dien_thoai=@sdt WHERE ma_nv=@ma",
                MyConnection.P("@ma", nv.MaNV), MyConnection.P("@ten", nv.HoTen),
                MyConnection.P("@cv", nv.ChucVu), MyConnection.P("@sdt", nv.SoDienThoai)) > 0;
        }

        public bool Delete(string ma)
        {
            return MyConnection.ExecuteNonQuery("DELETE FROM nhan_vien WHERE ma_nv=@ma",
                MyConnection.P("@ma", ma)) > 0;
        }

        public string SinhMa()
        {
            return MaTuDongDAO.Sinh("NV", "nhan_vien", "ma_nv");
        }
    }
}
