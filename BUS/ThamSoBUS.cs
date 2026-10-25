using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using QLKS.Common;
using QLKS.DAO;
using QLKS.DTO;

namespace QLKS.BUS
{
    public class ThamSoBUS
    {
        // ================== PHẦN STATIC: các module khác dùng để đọc tham số ==================
        // Ví dụ:  int phutLe = ThamSoBUS.DocInt("so_phut_le_mien", 15);
        private static ArrayList boNho;

        public static string Doc(string ten, string macDinh = "")
        {
            try
            {
                if (boNho == null) boNho = new ThamSoDAO().GetAll();
                foreach (ThamSo t in boNho)
                    if (t.TenThamSo == ten) return t.GiaTri;
            }
            catch (Exception) { }
            return macDinh;
        }

        public static int DocInt(string ten, int macDinh = 0)
        {
            int v;
            return int.TryParse(Doc(ten, ""), NumberStyles.Integer, CultureInfo.InvariantCulture, out v) ? v : macDinh;
        }

        public static decimal DocDecimal(string ten, decimal macDinh = 0)
        {
            decimal v;
            return decimal.TryParse(Doc(ten, ""), NumberStyles.Number, CultureInfo.InvariantCulture, out v) ? v : macDinh;
        }

        public static void LamMoi()
        {
            boNho = null;
        }

        // ================== PHẦN INSTANCE: form CRUD tham số ==================
        private readonly ThamSoDAO dao = new ThamSoDAO();
        private ArrayList ds;

        public ArrayList DanhSach
        {
            get { if (ds == null) TaiDuLieu(); return ds; }
        }

        public void TaiDuLieu()
        {
            ds = dao.GetAll();
        }

        public ThamSo TimTheoTen(string ten)
        {
            foreach (ThamSo t in DanhSach) if (t.TenThamSo == ten) return t;
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

        private string Kiem(ThamSo t, bool them)
        {
            if (!Validator.TenThamSoHopLe(t.TenThamSo)) return "Tên tham số chỉ gồm chữ thường không dấu, số và dấu gạch dưới (ví dụ: gio_tra_phong).";
            if (Validator.Rong(t.GiaTri)) return "Giá trị không được để trống.";
            if (them && TimTheoTen(t.TenThamSo) != null) return "Tên tham số đã tồn tại.";
            return null;
        }

        public bool Them(ThamSo t, out string loi)
        {
            loi = Kiem(t, true);
            if (loi != null) return false;
            try { dao.Insert(t); TaiDuLieu(); LamMoi(); return true; }
            catch (Exception ex) { loi = MyConnection.ThongBaoLoi(ex); return false; }
        }

        public bool Sua(ThamSo t, out string loi)
        {
            loi = Kiem(t, false);
            if (loi != null) return false;
            try { dao.Update(t); TaiDuLieu(); LamMoi(); return true; }
            catch (Exception ex) { loi = MyConnection.ThongBaoLoi(ex); return false; }
        }

        public bool Xoa(string ten, out string loi)
        {
            loi = null;
            try { dao.Delete(ten); TaiDuLieu(); LamMoi(); return true; }
            catch (Exception ex) { loi = MyConnection.ThongBaoLoi(ex); return false; }
        }
    }
}
