using System;
using System.Collections;
using System.Collections.Generic;
using QLKS.Common;
using QLKS.DAO;
using QLKS.DTO;

namespace QLKS.BUS
{
    public class KhuyenMaiBUS
    {
        private readonly KhuyenMaiDAO dao = new KhuyenMaiDAO();
        private ArrayList ds;
        public ArrayList DanhSach { get { if (ds == null) TaiDuLieu(); return ds; } }
        public void TaiDuLieu() { ds = dao.GetAll(); }
        public string SinhMa() { return dao.SinhMa(); }
        public ArrayList TimNhanh(string s) { return TimKiemBUS.TimNhanh(DanhSach, s); }
        public ArrayList TimNangCao(List<TieuChi> dk) { return TimKiemBUS.Loc(DanhSach, dk); }
        public static string Kiem(KhuyenMai k)
        {
            if (k == null) return "Chưa có thông tin khuyến mãi.";
            if (string.IsNullOrWhiteSpace(k.TenKM) || k.TenKM.Trim().Length > 150) return "Tên khuyến mãi phải có từ 1 đến 150 ký tự.";
            if (k.LoaiGiam != "Phần trăm" && k.LoaiGiam != "Số tiền") return "Vui lòng chọn loại giảm hợp lệ.";
            if (k.GiaTriGiam <= 0) return "Giá trị giảm phải lớn hơn 0.";
            if (k.LoaiGiam == "Phần trăm" && (k.GiaTriGiam > 100 || decimal.Round(k.GiaTriGiam, 2) != k.GiaTriGiam))
                return "Phần trăm giảm tối đa 100%, tối đa 2 chữ số thập phân.";
            if (k.LoaiGiam == "Số tiền" && (k.GiaTriGiam > 9999999999999m || decimal.Truncate(k.GiaTriGiam) != k.GiaTriGiam))
                return "Số tiền giảm phải là số nguyên dương, tối đa 13 chữ số.";
            if (k.NgayKetThuc.Date < k.NgayBatDau.Date) return "Ngày kết thúc không được trước ngày bắt đầu.";
            if (k.TongThuToiThieu < 0 || k.TongThuToiThieu > 999999999999999m || decimal.Truncate(k.TongThuToiThieu) != k.TongThuToiThieu)
                return "Tổng thu tối thiểu phải là số nguyên không âm, tối đa 15 chữ số.";
            if (k.SoLuotToiToiThieu < 0) return "Số lượt đến tối thiểu không được âm.";
            return null;
        }
        public bool Luu(KhuyenMai k, bool them, out string loi)
        {
            loi = Kiem(k); if (loi != null) return false;
            k.TenKM = k.TenKM.Trim(); k.NgayBatDau = k.NgayBatDau.Date; k.NgayKetThuc = k.NgayKetThuc.Date;
            try
            {
                TaiDuLieu(); bool tonTai = false;
                foreach (KhuyenMai cu in DanhSach)
                {
                    if (cu.MaKM == k.MaKM) tonTai = true;
                    if ((them || cu.MaKM != k.MaKM) && string.Equals(cu.TenKM, k.TenKM, StringComparison.CurrentCultureIgnoreCase))
                    { loi = "Tên khuyến mãi đã tồn tại."; return false; }
                }
                if (!them && !tonTai) { loi = "Khuyến mãi không còn tồn tại. Vui lòng tải lại."; return false; }
                if (them) k.MaKM = dao.SinhMa();
                if (!dao.Luu(k, them)) { loi = "Không lưu được khuyến mãi."; return false; }
                ds = null; return true;
            }
            catch (Exception ex) { loi = MyConnection.ThongBaoLoi(ex); return false; }
        }
        public bool Xoa(string ma, out string loi)
        {
            loi = null;
            try { if (!dao.Delete(ma)) { loi = "Khuyến mãi không còn tồn tại."; return false; } ds = null; return true; }
            catch (Exception ex) { loi = MyConnection.ThongBaoLoi(ex); return false; }
        }
    }
}
