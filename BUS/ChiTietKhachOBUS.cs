using System.Collections;
using QLKS.DAO;
using QLKS.DTO;

namespace QLKS.BUS
{
    /// <summary>
    /// Danh sách khách ở đã load sẵn (ArrayList). Việc thêm/sửa/xóa khách đi cùng Chi tiết đặt phòng
    /// nên nằm trong ChiTietDatPhongBUS (cùng 1 transaction).
    /// </summary>
    public class ChiTietKhachOBUS
    {
        private readonly ChiTietKhachODAO dao = new ChiTietKhachODAO();
        private ArrayList ds;

        public ArrayList DanhSach
        {
            get { if (ds == null) TaiDuLieu(); return ds; }
        }

        public void TaiDuLieu()
        {
            ds = dao.GetAll();
        }

        /// <summary>Khách ở của 1 phòng trong 1 phiếu.</summary>
        public ArrayList Theo(string maPhieu, string maPhong)
        {
            var kq = new ArrayList();
            foreach (ChiTietKhachO k in DanhSach)
                if (k.MaPhieu == maPhieu && k.MaPhong == maPhong) kq.Add(k);
            return kq;
        }

        /// <summary>Khách ở của cả phiếu (mọi phòng).</summary>
        public ArrayList TheoPhieu(string maPhieu)
        {
            var kq = new ArrayList();
            foreach (ChiTietKhachO k in DanhSach)
                if (k.MaPhieu == maPhieu) kq.Add(k);
            return kq;
        }
    }
}
