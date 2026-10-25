using System;
using System.Collections;
using System.Collections.Generic;
using QLKS.Common;
using QLKS.DAO;
using QLKS.DTO;

namespace QLKS.BUS
{
    public class KhachHangBUS
    {
        private readonly KhachHangDAO dao = new KhachHangDAO();
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

        public KhachHang TimTheoMa(string ma)
        {
            foreach (KhachHang k in DanhSach) if (k.MaKH == ma) return k;
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

        private string Kiem(KhachHang kh, bool them)
        {
            if (Validator.Rong(kh.HoTen)) return "Họ tên không được để trống.";
            if (!Validator.CCCDHopLe(kh.CCCD)) return "CCCD phải gồm đúng 12 chữ số.";
            if (Validator.Rong(kh.GioiTinh)) return "Vui lòng chọn giới tính.";
            if (them && TimTheoMa(kh.MaKH) != null) return "Mã khách hàng đã tồn tại.";
            foreach (KhachHang k in DanhSach)                     // tính duy nhất của CCCD
                if (k.CCCD == kh.CCCD && k.MaKH != kh.MaKH) return "CCCD này đã thuộc về khách hàng " + k.MaKH + " - " + k.HoTen + ".";
            return null;
        }

        public bool Them(KhachHang kh, out string loi)
        {
            kh.HoTen = kh.HoTen == null ? "" : kh.HoTen.Trim();
            kh.CCCD = kh.CCCD == null ? "" : kh.CCCD.Trim();
            loi = Kiem(kh, true);
            if (loi != null) return false;
            try { dao.Insert(kh); TaiDuLieu(); return true; }
            catch (Exception ex) { loi = MyConnection.ThongBaoLoi(ex); return false; }
        }

        public bool Sua(KhachHang kh, out string loi)
        {
            kh.HoTen = kh.HoTen == null ? "" : kh.HoTen.Trim();
            kh.CCCD = kh.CCCD == null ? "" : kh.CCCD.Trim();
            loi = Kiem(kh, false);
            if (loi != null) return false;
            try { dao.Update(kh); TaiDuLieu(); return true; }
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
