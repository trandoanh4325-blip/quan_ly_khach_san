using System;
using System.Collections;
using System.Collections.Generic;
using QLKS.Common;
using QLKS.DAO;
using QLKS.DTO;

namespace QLKS.BUS
{
    public class PhuThuBUS
    {
        private readonly PhuThuDAO dao = new PhuThuDAO();
        private ArrayList ds;
        public ArrayList DanhSach { get { if (ds == null) TaiDuLieu(); return ds; } }
        public void TaiDuLieu() { ds = dao.GetAll(); }
        public string SinhMa() { return dao.SinhMa(); }
        public ArrayList SanPham() { return dao.SanPham(); }
        public ArrayList TimNhanh(string s) { return TimKiemBUS.TimNhanh(DanhSach, s); }
        public ArrayList TimNangCao(List<TieuChi> dk) { return TimKiemBUS.Loc(DanhSach, dk); }
        public bool Luu(PhuThu p, bool them, out string loi)
        {
            loi = null;
            if (p == null) { loi = "Chưa có thông tin phụ thu."; return false; }
            p.TenPhuThu = (p.TenPhuThu ?? "").Trim();
            if (p.TenPhuThu.Length == 0 || p.TenPhuThu.Length > 150) { loi = "Tên phụ thu phải có từ 1 đến 150 ký tự."; return false; }
            try
            {
                TaiDuLieu(); bool tonTai = false;
                foreach (PhuThu cu in DanhSach)
                {
                    if (cu.MaPhuThu == p.MaPhuThu) tonTai = true;
                    if ((them || cu.MaPhuThu != p.MaPhuThu) && string.Equals(cu.TenPhuThu, p.TenPhuThu, StringComparison.CurrentCultureIgnoreCase))
                    { loi = "Tên phụ thu đã tồn tại."; return false; }
                }
                if (!them && !tonTai) { loi = "Phụ thu không còn tồn tại. Vui lòng tải lại."; return false; }
                SanPhamPhuThu sp = null;
                foreach (SanPhamPhuThu item in dao.SanPham()) if (item.MaSP == p.MaSP) sp = item;
                if (sp == null) { loi = "Vui lòng chọn sản phẩm liên kết còn tồn tại."; return false; }
                if (sp.GiaBan < 0) { loi = "Sản phẩm có giá bán không hợp lệ."; return false; }
                if (them) p.MaPhuThu = dao.SinhMa();
                if (!(them ? dao.Insert(p) : dao.Update(p))) { loi = "Không lưu được phụ thu."; return false; }
                ds = null; return true;
            }
            catch (Exception ex) { loi = MyConnection.ThongBaoLoi(ex); return false; }
        }
        public bool Xoa(string ma, out string loi)
        {
            loi = null;
            try { if (!dao.Delete(ma)) { loi = "Phụ thu không còn tồn tại."; return false; } ds = null; return true; }
            catch (Exception ex) { loi = MyConnection.ThongBaoLoi(ex); return false; }
        }
    }
}
