using System.Collections;
using System.Data;
using QLKS.DTO;

namespace QLKS.DAO
{
    public class ThamSoDAO
    {
        public ArrayList GetAll()
        {
            DataTable dt = MyConnection.ExecuteQuery(
                "SELECT ten_tham_so, gia_tri, mo_ta FROM tham_so ORDER BY ten_tham_so");
            var ds = new ArrayList();
            foreach (DataRow r in dt.Rows)
            {
                ds.Add(new ThamSo
                {
                    TenThamSo = r["ten_tham_so"].ToString(),
                    GiaTri = r["gia_tri"].ToString(),
                    MoTa = r["mo_ta"].ToString()
                });
            }
            return ds;
        }

        public bool Insert(ThamSo t)
        {
            return MyConnection.ExecuteNonQuery(
                "INSERT INTO tham_so (ten_tham_so, gia_tri, mo_ta) VALUES (@ten, @gt, @mt)",
                MyConnection.P("@ten", t.TenThamSo), MyConnection.P("@gt", t.GiaTri),
                MyConnection.P("@mt", t.MoTa)) > 0;
        }

        public bool Update(ThamSo t)
        {
            return MyConnection.ExecuteNonQuery(
                "UPDATE tham_so SET gia_tri=@gt, mo_ta=@mt WHERE ten_tham_so=@ten",
                MyConnection.P("@ten", t.TenThamSo), MyConnection.P("@gt", t.GiaTri),
                MyConnection.P("@mt", t.MoTa)) > 0;
        }

        public bool Delete(string ten)
        {
            return MyConnection.ExecuteNonQuery("DELETE FROM tham_so WHERE ten_tham_so=@ten",
                MyConnection.P("@ten", ten)) > 0;
        }
    }
}
