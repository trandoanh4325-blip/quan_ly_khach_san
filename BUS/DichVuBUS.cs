using System;
using System.Collections;
using System.Collections.Generic;
using QLKS.Common;
using QLKS.DAO;
using QLKS.DTO;

namespace QLKS.BUS
{
    public class DichVuBUS
    {
        private readonly DichVuDAO dao = new DichVuDAO();
        private ArrayList ds;
        public ArrayList DanhSach { get { if (ds == null) TaiDuLieu(); return ds; } }
        public void TaiDuLieu() { ds = dao.GetAll(); }
        public string SinhMa() { return dao.SinhMa(); }
        public ArrayList TimNhanh(string tuKhoa) { return TimKiemBUS.TimNhanh(DanhSach, tuKhoa); }
        public ArrayList TimNangCao(List<TieuChi> dk) { return TimKiemBUS.Loc(DanhSach, dk); }
        public DichVu TimTheoMa(string ma)
        {
            foreach (DichVu d in DanhSach) if (d.MaDV == ma) return d;
            return null;
        }

        private bool Luu(DichVu d, bool them, out string loi)
        {
            loi = null;
            d.TenDV = (d.TenDV ?? "").Trim();
            d.DonViTinh = (d.DonViTinh ?? "").Trim();
            if (d.TenDV.Length == 0 || d.TenDV.Length > 100) loi = "Tên dịch vụ phải có từ 1 đến 100 ký tự.";
            else if (d.DonViTinh.Length == 0 || d.DonViTinh.Length > 20) loi = "Đơn vị tính phải có từ 1 đến 20 ký tự.";
            else if (d.GiaTien <= 0 || d.GiaTien > 999999999999999m || decimal.Truncate(d.GiaTien) != d.GiaTien)
                loi = "Giá dịch vụ phải là số nguyên dương, tối đa 15 chữ số.";
            if (loi != null) return false;
            try
            {
                TaiDuLieu();
                foreach (DichVu cu in DanhSach)
                    if (cu.MaDV != d.MaDV && string.Equals(cu.TenDV, d.TenDV, StringComparison.CurrentCultureIgnoreCase))
                    { loi = "Tên dịch vụ đã tồn tại."; return false; }
                if (them) d.MaDV = dao.SinhMa();
                else if (TimTheoMa(d.MaDV) == null) { loi = "Dịch vụ không còn tồn tại."; return false; }
                if (!(them ? dao.Insert(d) : dao.Update(d))) { loi = "Không lưu được dịch vụ."; return false; }
                TaiDuLieu();
                return true;
            }
            catch (Exception ex) { loi = MyConnection.ThongBaoLoi(ex); return false; }
        }

        public bool Them(DichVu d, out string loi) { return Luu(d, true, out loi); }
        public bool Sua(DichVu d, out string loi) { return Luu(d, false, out loi); }
        public bool Xoa(string ma, out string loi)
        {
            loi = null;
            try
            {
                if (!dao.Delete(ma)) { loi = "Dịch vụ không còn tồn tại."; return false; }
                TaiDuLieu(); return true;
            }
            catch (Exception ex) { loi = MyConnection.ThongBaoLoi(ex); return false; }
        }
    }
}
