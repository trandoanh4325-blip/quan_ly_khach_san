using System;
using System.Collections;
using System.Collections.Generic;
using QLKS.Common;
using QLKS.DAO;
using QLKS.DTO;

namespace QLKS.BUS
{
    public class PhongBUS
    {
        public static readonly int[] SoNguoiChoPhep = { 2, 4, 6, 8 };

        private readonly PhongDAO dao = new PhongDAO();
        private ArrayList ds;

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

        public Phong TimTheoMa(string ma)
        {
            foreach (Phong p in DanhSach) if (p.MaPhong == ma) return p;
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

        private string Kiem(Phong p, bool them)
        {
            if (Validator.Rong(p.MaPhong)) return "Mã phòng không được để trống.";
            if (p.MaPhong.Length > 10) return "Mã phòng tối đa 10 ký tự.";
            if (them && TimTheoMa(p.MaPhong) != null) return "Mã phòng đã tồn tại.";
            if (Validator.Rong(p.TenPhong)) return "Tên phòng không được để trống.";
            if (Validator.Rong(p.MaLoai)) return "Vui lòng chọn loại phòng (nút ...).";
            if (Array.IndexOf(SoNguoiChoPhep, p.SoNguoiToiDa) < 0) return "Số người tối đa chỉ được chọn 2, 4, 6 hoặc 8.";
            foreach (Phong x in DanhSach)
                if (string.Equals(x.TenPhong, p.TenPhong, StringComparison.CurrentCultureIgnoreCase) && x.MaPhong != p.MaPhong)
                    return "Tên phòng đã được phòng " + x.MaPhong + " sử dụng.";
            return null;
        }

        public bool Them(Phong p, out string loi)
        {
            p.MaPhong = p.MaPhong == null ? "" : p.MaPhong.Trim();
            p.TenPhong = p.TenPhong == null ? "" : p.TenPhong.Trim();
            loi = Kiem(p, true);
            if (loi != null) return false;
            try
            {
                MyConnection.BeginTransaction();
                dao.Insert(p);
                dao.CapNhatLoaiPhong(p.MaLoai);      // tổng số phòng và số phòng trống của loại tăng lên
                MyConnection.Commit();
                TaiDuLieu();
                return true;
            }
            catch (Exception ex)
            {
                MyConnection.Rollback();
                loi = MyConnection.ThongBaoLoi(ex);
                return false;
            }
        }

        public bool Sua(Phong p, out string loi)
        {
            p.TenPhong = p.TenPhong == null ? "" : p.TenPhong.Trim();
            loi = Kiem(p, false);
            if (loi != null) return false;
            Phong cu = TimTheoMa(p.MaPhong);
            if (cu == null) { loi = "Không tìm thấy phòng cần sửa."; return false; }
            if (cu.TrangThai == "Đã đủ" && cu.MaLoai != p.MaLoai)
            {
                loi = "Phòng đang có khách, không thể đổi loại phòng.";
                return false;
            }
            try
            {
                MyConnection.BeginTransaction();
                dao.Update(p);
                dao.CapNhatLoaiPhong(cu.MaLoai);
                if (cu.MaLoai != p.MaLoai) dao.CapNhatLoaiPhong(p.MaLoai);
                MyConnection.Commit();
                TaiDuLieu();
                return true;
            }
            catch (Exception ex)
            {
                MyConnection.Rollback();
                loi = MyConnection.ThongBaoLoi(ex);
                return false;
            }
        }

        public bool Xoa(string ma, out string loi)
        {
            loi = null;
            Phong cu = TimTheoMa(ma);
            if (cu == null) { loi = "Không tìm thấy phòng cần xóa."; return false; }
            if (cu.TrangThai == "Đã đủ")
            {
                loi = "Phòng đang có khách, không thể xóa.";
                return false;
            }
            try
            {
                MyConnection.BeginTransaction();
                dao.Delete(ma);
                dao.CapNhatLoaiPhong(cu.MaLoai);
                MyConnection.Commit();
                TaiDuLieu();
                return true;
            }
            catch (Exception ex)
            {
                MyConnection.Rollback();
                loi = MyConnection.ThongBaoLoi(ex);
                return false;
            }
        }
    }
}
