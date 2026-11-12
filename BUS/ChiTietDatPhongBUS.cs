using System;
using System.Collections;
using System.Collections.Generic;
using QLKS.Common;
using QLKS.DAO;
using QLKS.DTO;

namespace QLKS.BUS
{
    /// <summary>
    /// Chi tiết đặt phòng + Chi tiết khách ở (main-sub của Phiếu đặt phòng).
    /// Thêm/sửa/xóa 1 phòng luôn đi cùng danh sách khách ở, phòng, loại phòng và tổng tiền phiếu
    /// nên chạy trong 1 transaction.
    /// </summary>
    public class ChiTietDatPhongBUS
    {
        private readonly ChiTietDatPhongDAO dao = new ChiTietDatPhongDAO();
        private readonly ChiTietKhachODAO khDao = new ChiTietKhachODAO();
        private readonly PhieuDatPhongDAO phieuDao = new PhieuDatPhongDAO();
        private readonly PhongDAO phongDao = new PhongDAO();
        private readonly PhongBUS phongBus = new PhongBUS();
        private readonly LoaiPhongBUS loaiBus = new LoaiPhongBUS();
        private ArrayList ds;

        /// <summary>Danh sách khách ở (đã load sẵn, lọc theo phòng/phiếu trong bộ nhớ).</summary>
        public readonly ChiTietKhachOBUS Khach = new ChiTietKhachOBUS();

        public ArrayList DanhSach
        {
            get { if (ds == null) TaiDuLieu(); return ds; }
        }

        public void TaiDuLieu()
        {
            ds = dao.GetAll();
            Khach.TaiDuLieu();
        }

        /// <summary>Các dòng chi tiết của 1 phiếu (click phiếu thì hiện đúng n dòng này).</summary>
        public ArrayList TheoPhieu(string maPhieu)
        {
            var kq = new ArrayList();
            foreach (ChiTietDatPhong ct in DanhSach)
                if (ct.MaPhieu == maPhieu) kq.Add(ct);
            return kq;
        }

        public ChiTietDatPhong TimChiTiet(string maPhieu, string maPhong)
        {
            foreach (ChiTietDatPhong ct in DanhSach)
                if (ct.MaPhieu == maPhieu && ct.MaPhong == maPhong) return ct;
            return null;
        }

        /// <summary>Phòng đang "Trống" và chưa có trong phiếu: danh sách cho nút "..." chọn phòng.</summary>
        public ArrayList PhongCoTheDat(PhieuDatPhong p)
        {
            phongBus.TaiDuLieu();
            var kq = new ArrayList();
            foreach (Phong ph in phongBus.DanhSach)
                if (ph.TrangThai == "Trống" && TimChiTiet(p.MaPhieu, ph.MaPhong) == null) kq.Add(ph);
            return kq;
        }

        /// <summary>
        /// Giá áp dụng + thành tiền dự kiến của 1 phòng theo phiếu:
        /// ở theo ngày = đơn giá ngày x số ngày dự kiến; ở theo giờ = đơn giá giờ x số giờ nghỉ.
        /// </summary>
        public static void ApDungGia(ChiTietDatPhong ct, PhieuDatPhong p)
        {
            if (p.OTheoGio == 1)
            {
                ct.GiaPhong = ct.DonGiaGio;
                ct.SoNgayO = 0;
                ct.SoGioO = p.SoGioNghi;
                ct.ThanhTien = ct.GiaPhong * ct.SoGioO;
            }
            else
            {
                ct.GiaPhong = ct.DonGiaNgay;
                ct.SoNgayO = p.SoNgayODuKien;
                ct.SoGioO = 0;
                ct.ThanhTien = ct.GiaPhong * ct.SoNgayO;
            }
        }

        /// <summary>Tạo dòng chi tiết (chưa lưu) để form hiện giá/thành tiền ngay khi chọn phòng.</summary>
        public ChiTietDatPhong TaoMoi(PhieuDatPhong p, Phong ph, int soKhach)
        {
            loaiBus.TaiDuLieu();
            LoaiPhong lp = loaiBus.TimTheoMa(ph.MaLoai);
            if (lp == null) return null;
            var ct = new ChiTietDatPhong
            {
                MaPhieu = p.MaPhieu,
                MaPhong = ph.MaPhong,
                TenPhong = ph.TenPhong,
                SoNguoiToiDa = ph.SoNguoiToiDa,
                DonGiaNgay = lp.DonGiaNgay,
                DonGiaGio = lp.DonGiaGio,
                SoKhach = soKhach,
                TrangThai = ChiTietDatPhong.DangO
            };
            ApDungGia(ct, p);
            return ct;
        }

        // Kiểm tra số khách và danh sách khách ở. Trả về null nếu hợp lệ.
        private string KiemKhach(Phong ph, int soKhach, List<ChiTietKhachO> khach, string maPhieu)
        {
            if (soKhach < 1) return "Số khách phải từ 1 trở lên.";
            if (soKhach > ph.SoNguoiToiDa) return ph.TenPhong + " chỉ ở tối đa " + ph.SoNguoiToiDa + " người.";
            if (khach == null || khach.Count != soKhach) return "Cần nhập đủ họ tên và CCCD của " + soKhach + " khách.";

            var daCo = new List<string>();
            foreach (ChiTietKhachO k in khach)
            {
                k.HoTen = k.HoTen == null ? "" : k.HoTen.Trim();
                k.CCCD = k.CCCD == null ? "" : k.CCCD.Trim();
                if (Validator.Rong(k.HoTen)) return "Họ tên khách không được để trống.";
                if (!Validator.CCCDHopLe(k.CCCD)) return "CCCD của khách " + k.HoTen + " phải gồm đúng 12 chữ số.";
                if (daCo.Contains(k.CCCD)) return "CCCD " + k.CCCD + " bị nhập trùng trong cùng phòng.";
                daCo.Add(k.CCCD);
            }

            foreach (ChiTietKhachO k in khach)
            {
                // trong cùng phiếu, CCCD không trùng giữa các phòng
                foreach (ChiTietKhachO khac in Khach.TheoPhieu(maPhieu))
                    if (khac.MaPhong != ph.MaPhong && khac.CCCD == k.CCCD)
                        return "CCCD " + k.CCCD + " đã có ở phòng " + khac.MaPhong + " trong cùng phiếu.";
                // một CCCD không được đang ở hai phòng cùng lúc
                if (khDao.DemDangO(k.CCCD, maPhieu) > 0)
                    return "CCCD " + k.CCCD + " (" + k.HoTen + ") đang ở một phòng khác, phải checkout trước.";
            }
            return null;
        }

        private static string KiemPhieu(PhieuDatPhong p)
        {
            if (p == null) return "Vui lòng chọn phiếu đặt phòng ở bảng phía trên.";
            if (p.TinhTrang == PhieuDatPhong.DaThanhToan) return "Phiếu đã có hóa đơn, không thể thay đổi phòng.";
            return null;
        }

        private void GhiKhach(ChiTietDatPhong ct, List<ChiTietKhachO> khach)
        {
            foreach (ChiTietKhachO k in khach)
            {
                if (Validator.Rong(k.MaKhachO)) k.MaKhachO = khDao.SinhMa();   // mã khách ở tự sinh
                k.MaPhieu = ct.MaPhieu;
                k.MaPhong = ct.MaPhong;
                khDao.Insert(k);
            }
        }

        public bool Them(PhieuDatPhong p, Phong chon, int soKhach, List<ChiTietKhachO> khach, out string loi)
        {
            loi = KiemPhieu(p);
            if (loi != null) return false;
            if (chon == null) { loi = "Vui lòng chọn phòng (nút ...)."; return false; }

            phongBus.TaiDuLieu();                       // trạng thái phòng mới nhất
            Phong ph = phongBus.TimTheoMa(chon.MaPhong);
            if (ph == null) { loi = "Không tìm thấy phòng đã chọn."; return false; }
            if (ph.TrangThai != "Trống") { loi = ph.TenPhong + " đang có khách, vui lòng chọn phòng trống."; return false; }
            if (TimChiTiet(p.MaPhieu, ph.MaPhong) != null) { loi = ph.TenPhong + " đã có trong phiếu này."; return false; }

            loi = KiemKhach(ph, soKhach, khach, p.MaPhieu);
            if (loi != null) return false;

            DateTime den = p.NgayCheckout.HasValue ? p.NgayCheckout.Value : p.NgayCheckin;
            if (dao.DemTrungThoiGian(ph.MaPhong, p.MaPhieu, p.NgayCheckin, den) > 0)
            {
                loi = ph.TenPhong + " đã được đặt trong khoảng thời gian này.";
                return false;
            }

            ChiTietDatPhong ct = TaoMoi(p, ph, soKhach);
            if (ct == null) { loi = "Không tìm thấy loại phòng của phòng đã chọn."; return false; }

            try
            {
                MyConnection.BeginTransaction();
                dao.Insert(ct);
                GhiKhach(ct, khach);
                phongDao.DoiTrangThai(ph.MaPhong, "Đã đủ");     // trống -> đã đủ, loại phòng giảm "số lượng đang có"
                phieuDao.CapNhatTongTien(p.MaPhieu);
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

        /// <summary>Sửa số khách và danh sách khách của 1 phòng đang ở (phòng không đổi được, muốn đổi thì xóa rồi thêm lại).</summary>
        public bool Sua(PhieuDatPhong p, ChiTietDatPhong ct, int soKhach, List<ChiTietKhachO> khach, out string loi)
        {
            loi = KiemPhieu(p);
            if (loi != null) return false;
            if (ct == null) { loi = "Vui lòng chọn dòng chi tiết cần sửa."; return false; }
            if (ct.TrangThai != ChiTietDatPhong.DangO) { loi = "Phòng đã checkout, chỉ được xem."; return false; }

            phongBus.TaiDuLieu();
            Phong ph = phongBus.TimTheoMa(ct.MaPhong);
            if (ph == null) { loi = "Không tìm thấy phòng."; return false; }

            loi = KiemKhach(ph, soKhach, khach, p.MaPhieu);
            if (loi != null) return false;

            try
            {
                MyConnection.BeginTransaction();
                ct.SoKhach = soKhach;
                dao.Update(ct);
                khDao.XoaTheoPhong(ct.MaPhieu, ct.MaPhong);     // ghi lại toàn bộ; khách cũ giữ nguyên mã khách ở
                GhiKhach(ct, khach);
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

        public bool Xoa(PhieuDatPhong p, ChiTietDatPhong ct, out string loi)
        {
            loi = KiemPhieu(p);
            if (loi != null) return false;
            if (ct == null) { loi = "Vui lòng chọn dòng chi tiết cần xóa."; return false; }
            if (ct.TrangThai != ChiTietDatPhong.DangO) { loi = "Phòng đã checkout, không thể xóa."; return false; }

            try
            {
                MyConnection.BeginTransaction();
                dao.Delete(ct.MaPhieu, ct.MaPhong);               // khách ở của phòng bị xóa theo
                phongDao.DoiTrangThai(ct.MaPhong, "Trống");       // đã đủ -> trống, loại phòng tăng "số lượng đang có"
                phieuDao.CapNhatTongTien(ct.MaPhieu);
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
