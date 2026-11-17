using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using QLKS.Common;
using QLKS.DAO;
using QLKS.DTO;

namespace QLKS.BUS
{
    // Mọi tìm kiếm thực hiện trên ArrayList đã nạp, không truy vấn SQL để tìm.
    public class SanPhamBUS
    {
        private readonly SanPhamDAO dao = new SanPhamDAO();
        private ArrayList ds;

        public ArrayList DanhSach
        {
            get { if (ds == null) TaiDuLieu(); return ds; }
        }

        public void TaiDuLieu()
        {
            DataTable dt = dao.LayDanhSach();
            var moi = new ArrayList();
            foreach (DataRow row in dt.Rows)
                moi.Add(new SanPham
                {
                    MaSP = Convert.ToString(row["ma_sp"]),
                    TenSP = Convert.ToString(row["ten_sp"]),
                    MaLoaiSP = Convert.ToString(row["ma_loai_sp"]),
                    DonViTinh = Convert.ToString(row["don_vi_tinh"]),
                    GiaBan = Convert.ToDecimal(row["gia_ban"]),
                    SoLuongTon = Convert.ToInt32(row["so_luong_ton"])
                });
            ds = moi;
        }

        public DataTable LayDanhSach()
        {
            TaiDuLieu();
            // Tương thích chữ ký đã dùng trước đó; dữ liệu hiển thị lấy từ DanhSach.
            return dao.LayDanhSach();
        }

        public string SinhMa() => MaTuDongDAO.Sinh("SP", "san_pham", "ma_sp");
        public DataTable LayLoaiSanPham() => dao.LayLoaiSanPham();

        public SanPham TimTheoMa(string ma)
        {
            foreach (SanPham sp in DanhSach)
                if (string.Equals(sp.MaSP, ma, StringComparison.OrdinalIgnoreCase)) return sp;
            return null;
        }

        public ArrayList TimKiem(string tuKhoa) => TimKiemBUS.TimNhanh(DanhSach, tuKhoa);
        public ArrayList TimNangCao(List<TieuChi> dk) => TimKiemBUS.Loc(DanhSach, dk);

        private string KiemTra(SanPham sp, bool them)
        {
            if (sp == null) return "Thông tin sản phẩm không hợp lệ.";
            if (string.IsNullOrWhiteSpace(sp.MaSP)) return "Mã sản phẩm không được để trống.";
            if (string.IsNullOrWhiteSpace(sp.TenSP)) return "Tên sản phẩm không được để trống.";
            if (string.IsNullOrWhiteSpace(sp.MaLoaiSP)) return "Vui lòng chọn loại sản phẩm.";
            if (string.IsNullOrWhiteSpace(sp.DonViTinh)) return "Đơn vị tính không được để trống.";
            if (sp.GiaBan < 0) return "Giá bán không được âm.";
            if (sp.MaSP.Length > 10 || sp.TenSP.Length > 100 ||
                sp.MaLoaiSP.Length > 10 || sp.DonViTinh.Length > 20)
                return "Dữ liệu vượt quá độ dài cho phép của CSDL.";
            if (them && TimTheoMa(sp.MaSP) != null) return "Mã sản phẩm đã tồn tại.";
            return null;
        }

        public bool Them(SanPham sp, out string loi)
        {
            if (sp != null) ChuanHoa(sp);
            loi = KiemTra(sp, true);
            if (loi != null) return false;
            try { if (dao.Them(sp) < 1) { loi = "Không thêm được sản phẩm."; return false; } TaiDuLieu(); return true; }
            catch (Exception ex) { loi = MyConnection.ThongBaoLoi(ex); return false; }
        }

        public bool Sua(SanPham sp, out string loi)
        {
            if (sp != null) ChuanHoa(sp);
            loi = KiemTra(sp, false);
            if (loi != null) return false;
            if (TimTheoMa(sp.MaSP) == null) { loi = "Không tìm thấy sản phẩm cần sửa."; return false; }
            try { dao.Sua(sp); TaiDuLieu(); return true; }
            catch (Exception ex) { loi = MyConnection.ThongBaoLoi(ex); return false; }
        }

        public bool Xoa(string maSP, out string loi)
        {
            loi = null;
            if (string.IsNullOrWhiteSpace(maSP)) { loi = "Vui lòng chọn sản phẩm cần xóa."; return false; }
            try { if (dao.Xoa(maSP) < 1) { loi = "Sản phẩm không tồn tại."; return false; } TaiDuLieu(); return true; }
            catch (Exception ex) { loi = MyConnection.ThongBaoLoi(ex); return false; }
        }

        private static void ChuanHoa(SanPham sp)
        {
            sp.MaSP = (sp.MaSP ?? "").Trim();
            sp.TenSP = (sp.TenSP ?? "").Trim();
            sp.MaLoaiSP = (sp.MaLoaiSP ?? "").Trim();
            sp.DonViTinh = (sp.DonViTinh ?? "").Trim();
        }
    }
}
