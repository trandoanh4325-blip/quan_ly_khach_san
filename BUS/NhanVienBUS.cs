using System;
using System.Collections;
using System.Collections.Generic;
using QLKS.Common;
using QLKS.DAO;
using QLKS.DTO;

namespace QLKS.BUS
{
    public class NhanVienBUS
    {
        private readonly NhanVienDAO dao = new NhanVienDAO();
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

        public NhanVien TimTheoMa(string ma)
        {
            foreach (NhanVien n in DanhSach) if (n.MaNV == ma) return n;
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

        // Trả về null nếu hợp lệ, ngược lại trả về câu báo lỗi
        private string Kiem(NhanVien nv, bool them)
        {
            if (Validator.Rong(nv.HoTen)) return "Họ tên không được để trống.";
            if (Validator.Rong(nv.ChucVu)) return "Chức vụ không được để trống.";
            if (!Validator.SDTHopLe(nv.SoDienThoai)) return "Số điện thoại phải gồm 10 chữ số và bắt đầu bằng 0.";
            if (them && TimTheoMa(nv.MaNV) != null) return "Mã nhân viên đã tồn tại.";
            foreach (NhanVien n in DanhSach)
                if (n.SoDienThoai == nv.SoDienThoai && n.MaNV != nv.MaNV) return "Số điện thoại đã được nhân viên khác sử dụng.";
            return null;
        }

        public bool Them(NhanVien nv, out string loi)
        {
            nv.HoTen = nv.HoTen == null ? "" : nv.HoTen.Trim();
            loi = Kiem(nv, true);
            if (loi != null) return false;
            try { dao.Insert(nv); TaiDuLieu(); return true; }
            catch (Exception ex) { loi = MyConnection.ThongBaoLoi(ex); return false; }
        }

        public bool Sua(NhanVien nv, out string loi)
        {
            nv.HoTen = nv.HoTen == null ? "" : nv.HoTen.Trim();
            loi = Kiem(nv, false);
            if (loi != null) return false;
            try { dao.Update(nv); TaiDuLieu(); return true; }
            catch (Exception ex) { loi = MyConnection.ThongBaoLoi(ex); return false; }
        }

        public bool Xoa(string ma, out string loi)
        {
            loi = null;
            try { dao.Delete(ma); TaiDuLieu(); return true; }
            catch (Exception ex) { loi = MyConnection.ThongBaoLoi(ex); return false; }
        }
    }
}
