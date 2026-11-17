using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using QLKS.Common;
using QLKS.DAO;
using QLKS.DTO;

namespace QLKS.BUS
{
    public class LoaiPhongBUS
    {
        private readonly LoaiPhongDAO dao = new LoaiPhongDAO();
        private ArrayList ds;

        /// <summary>Danh sách đã load (ArrayList). Mọi tìm kiếm chạy trên danh sách này.</summary>
        public ArrayList DanhSach
        {
            get { if (ds == null) TaiDuLieu(); return ds; }
        }

        public void TaiDuLieu()
        {
            ds = dao.GetAll();
        }

        public string SinhMa()
        {
            return dao.SinhMa();
        }

        public LoaiPhong TimTheoMa(string ma)
        {
            foreach (LoaiPhong l in DanhSach) if (l.MaLoai == ma) return l;
            return null;
        }

        public ArrayList TimNhanh(string tuKhoa)
        {
            return TimKiemBUS.TimNhanh(DanhSach, tuKhoa);
        }

        public ArrayList TimNangCao(List<TieuChi> dk)
        {
            return TimKiemBUS.Loc(DanhSach, dk);
        }

        /// <summary>Đọc số tiền người dùng gõ ("400000", "400.000", "400,000"). Sai định dạng trả về false.</summary>
        public static bool DocTien(string s, out decimal v)
        {
            v = 0;
            if (s == null) return false;
            s = s.Trim().Replace(".", "").Replace(",", "").Replace(" ", "");
            return decimal.TryParse(s, NumberStyles.None, CultureInfo.InvariantCulture, out v);
        }

        // Trả về null nếu hợp lệ, ngược lại trả về câu báo lỗi
        private string Kiem(LoaiPhong lp, bool them)
        {
            if (Validator.Rong(lp.TenLoai)) return "Tên loại phòng không được để trống.";
            if (lp.DonGiaNgay <= 0) return "Đơn giá ngày phải là số lớn hơn 0.";
            if (lp.DonGiaGio <= 0) return "Đơn giá giờ phải là số lớn hơn 0.";
            if (them && TimTheoMa(lp.MaLoai) != null) return "Mã loại phòng đã tồn tại.";
            foreach (LoaiPhong l in DanhSach)
                if (string.Equals(l.TenLoai, lp.TenLoai, StringComparison.CurrentCultureIgnoreCase) && l.MaLoai != lp.MaLoai)
                    return "Tên loại phòng đã được loại " + l.MaLoai + " sử dụng.";
            return null;
        }

        public bool Them(LoaiPhong lp, out string loi)
        {
            lp.TenLoai = lp.TenLoai == null ? "" : lp.TenLoai.Trim();
            loi = Kiem(lp, true);
            if (loi != null) return false;
            try { dao.Insert(lp); TaiDuLieu(); return true; }
            catch (Exception ex) { loi = MyConnection.ThongBaoLoi(ex); return false; }
        }

        public bool Sua(LoaiPhong lp, out string loi)
        {
            lp.TenLoai = lp.TenLoai == null ? "" : lp.TenLoai.Trim();
            loi = Kiem(lp, false);
            if (loi != null) return false;
            try { dao.Update(lp); TaiDuLieu(); return true; }
            catch (Exception ex) { loi = MyConnection.ThongBaoLoi(ex); return false; }
        }

        public bool Xoa(string ma, out string loi)
        {
            loi = null;
            LoaiPhong lp = TimTheoMa(ma);
            if (lp != null && lp.TongSoPhong > 0)
            {
                loi = "Loại phòng này đang có " + lp.TongSoPhong + " phòng, hãy xóa hoặc chuyển các phòng đó trước.";
                return false;
            }
            try { dao.Delete(ma); TaiDuLieu(); return true; }
            catch (Exception ex) { loi = MyConnection.ThongBaoLoi(ex); return false; }
        }
    }
}
