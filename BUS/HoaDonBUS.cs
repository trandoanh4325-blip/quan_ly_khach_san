using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using QLKS.Common;
using QLKS.DAO;
using QLKS.DTO;

namespace QLKS.BUS
{
    public class HoaDonBUS
    {
        private readonly HoaDonDAO dao = new HoaDonDAO();
        private ArrayList ds;
        public ArrayList DanhSach { get { if (ds == null) TaiDuLieu(); return ds; } }
        public void TaiDuLieu() { ds = dao.GetAll(); }
        public ArrayList TimNhanh(string tuKhoa) { return TimKiemBUS.TimNhanh(DanhSach, tuKhoa); }
        public ArrayList TimNangCao(List<TieuChi> dk) { return TimKiemBUS.Loc(DanhSach, dk); }
        public void DocChiTiet(HoaDon h) { dao.DocChiTiet(h); }
        public ArrayList PhuThu() { return dao.PhuThu(); }
        public ArrayList KhuyenMai() { return dao.KhuyenMai(); }
        public ArrayList PhieuChuaThanhToan()
        {
            var kq = new ArrayList();
            foreach (PhieuDatPhong p in new PhieuDatPhongDAO().GetAll())
                if (p.TinhTrang == PhieuDatPhong.ChuaThanhToan) kq.Add(p);
            return kq;
        }

        // Theo giờ: làm tròn lên khi phần lẻ vượt số phút miễn; tối thiểu một giờ.
        // Theo ngày: đến giờ trả phòng chuẩn, quá giờ + phút miễn tính thêm một ngày.
        public static void TinhTienPhong(ChiTietDatPhong ct, PhieuDatPhong p, DateTime checkout, int phutMien, TimeSpan gioTra)
        {
            if (checkout < p.NgayCheckin) throw new InvalidOperationException("Giờ trả phòng phải từ giờ nhận phòng trở đi.");
            phutMien = Math.Clamp(phutMien, 0, 59);
            ct.SoNgayO = 0; ct.SoGioO = 0;
            if (p.OTheoGio == 1)
            {
                double phut = (checkout - p.NgayCheckin).TotalMinutes;
                int gio = (int)Math.Floor(phut / 60);
                if (phut - gio * 60 > phutMien) gio++;
                ct.SoGioO = Math.Max(1, gio);
                ct.ThanhTien = ct.GiaPhong * ct.SoGioO;
            }
            else
            {
                int ngay = (checkout.Date - p.NgayCheckin.Date).Days;
                if (checkout > checkout.Date + gioTra + TimeSpan.FromMinutes(phutMien)) ngay++;
                ct.SoNgayO = Math.Max(1, ngay);
                ct.ThanhTien = ct.GiaPhong * ct.SoNgayO;
            }
            ct.NgayGioCheckout = checkout;
        }

        public static decimal TinhGiam(KhuyenMaiThanhToan km, DateTime ngay, decimal tongThu, int soLuot, decimal tong)
        {
            if (km == null) return 0;
            if (ngay.Date < km.NgayBatDau.Date || ngay.Date > km.NgayKetThuc.Date)
                throw new InvalidOperationException("Khuyến mãi không có hiệu lực tại ngày thanh toán.");
            if (tongThu < km.TongThuToiThieu || soLuot < km.SoLuotToiToiThieu)
                throw new InvalidOperationException("Khách hàng chưa đủ điều kiện áp dụng khuyến mãi.");
            if (km.GiaTriGiam < 0 || (km.LoaiGiam == "Phần trăm" && km.GiaTriGiam > 100))
                throw new InvalidOperationException("Giá trị khuyến mãi không hợp lệ.");
            if (km.LoaiGiam != "Phần trăm" && km.LoaiGiam != "Số tiền")
                throw new InvalidOperationException("Loại khuyến mãi không hợp lệ.");
            return Math.Min(tong, Math.Round(km.LoaiGiam == "Phần trăm" ? tong * km.GiaTriGiam / 100m : km.GiaTriGiam, 0, MidpointRounding.AwayFromZero));
        }

        private HoaDon TinhLai(HoaDon nhap)
        {
            if (nhap == null) throw new InvalidOperationException("Vui lòng chọn phiếu đặt phòng.");
            PhieuDatPhong p = null;
            foreach (PhieuDatPhong item in new PhieuDatPhongDAO().GetAll()) if (item.MaPhieu == nhap.MaPhieu) p = item;
            if (p == null) throw new InvalidOperationException("Phiếu đặt phòng không còn tồn tại.");
            if (p.TinhTrang == PhieuDatPhong.DaThanhToan) throw new InvalidOperationException("Phiếu đã được thanh toán, không thể lập thêm hóa đơn.");
            if (nhap.NgayThanhToan < p.NgayCheckin || nhap.NgayThanhToan > DateTime.Now.AddMinutes(1))
                throw new InvalidOperationException("Giờ thanh toán phải từ giờ nhận phòng đến hiện tại.");
            bool coNV = false;
            foreach (NhanVien n in new NhanVienDAO().GetAll()) if (n.MaNV == nhap.MaNV) coNV = true;
            if (!coNV) throw new InvalidOperationException("Vui lòng chọn nhân viên thanh toán hợp lệ.");
            var h = new HoaDon { MaPhieu = p.MaPhieu, MaKH = p.MaKH, TenKH = p.TenKH, MaNV = nhap.MaNV,
                NgayThanhToan = nhap.NgayThanhToan, MaKM = string.IsNullOrWhiteSpace(nhap.MaKM) ? null : nhap.MaKM,
                TienDaCoc = p.TrangThaiCoc == "Đã cọc" ? p.TienCoc : 0 };
            var phong = new HashSet<string>();
            foreach (ChiTietDatPhong ct in new ChiTietDatPhongDAO().GetAll())
            {
                if (ct.MaPhieu != p.MaPhieu) continue;
                if (ct.TrangThai != ChiTietDatPhong.DangO) throw new InvalidOperationException("Phiếu có phòng đã checkout. Vui lòng kiểm tra lại dữ liệu.");
                TinhTienPhong(ct, p, h.NgayThanhToan, ThamSoBUS.DocInt("so_phut_le_mien", 15), PhieuDatPhongBUS.GioTraPhong());
                h.Phong.Add(ct); phong.Add(ct.MaPhong); h.TongTienPhong += ct.ThanhTien;
            }
            if (phong.Count == 0) throw new InvalidOperationException("Phiếu chưa có phòng để thanh toán.");
            var dvBus = new DichVuBUS();
            var trung = new HashSet<(string, string)>();
            foreach (var d in nhap.DichVu)
            {
                var dv = dvBus.TimTheoMa(d.MaDV);
                if (dv == null || dv.GiaTien <= 0 || !phong.Contains(d.MaPhong) || d.SoLuongSuDung <= 0 || !trung.Add((d.MaPhong, d.MaDV)))
                    throw new InvalidOperationException("Chi tiết dịch vụ không hợp lệ hoặc bị trùng phòng/dịch vụ.");
                var ct = new ChiTietSuDungDichVu { MaPhong = d.MaPhong, MaDV = dv.MaDV, TenDV = dv.TenDV,
                    SoLuongSuDung = d.SoLuongSuDung, DonGiaSuDung = dv.GiaTien, ThanhTien = dv.GiaTien * d.SoLuongSuDung };
                h.DichVu.Add(ct); h.TongTienDichVu += ct.ThanhTien;
            }
            trung.Clear();
            var phuThu = dao.PhuThu();
            foreach (var d in nhap.PhuThu)
            {
                LuaChonPhuThu pt = null;
                foreach (LuaChonPhuThu item in phuThu) if (item.MaPhuThu == d.MaPhuThu) pt = item;
                if (pt == null || pt.DonGia < 0 || !phong.Contains(d.MaPhong) || d.SoLuong <= 0 || (d.GhiChu ?? "").Length > 255 || !trung.Add((d.MaPhong, d.MaPhuThu)))
                    throw new InvalidOperationException("Chi tiết phụ thu không hợp lệ hoặc bị trùng phòng/phụ thu.");
                var ct = new ChiTietPhuThu { MaPhong = d.MaPhong, MaPhuThu = pt.MaPhuThu, TenPhuThu = pt.TenPhuThu,
                    SoLuong = d.SoLuong, DonGia = pt.DonGia, ThanhTien = pt.DonGia * d.SoLuong, GhiChu = d.GhiChu };
                h.PhuThu.Add(ct); h.TongTienPhuThu += ct.ThanhTien;
            }
            decimal tong = h.TongTienPhong + h.TongTienDichVu + h.TongTienPhuThu;
            KhuyenMaiThanhToan km = null;
            if (h.MaKM != null)
            {
                foreach (KhuyenMaiThanhToan item in dao.KhuyenMai()) if (item.MaKM == h.MaKM) km = item;
                if (km == null) throw new InvalidOperationException("Khuyến mãi không còn tồn tại.");
            }
            DataRow kh = dao.KhachHang(h.MaKH);
            h.TienGiam = TinhGiam(km, h.NgayThanhToan, Convert.ToDecimal(kh["tong_thu"]), Convert.ToInt32(kh["so_luot_toi"]), tong);
            h.TongTienHoaDon = tong - h.TienGiam;
            h.TienCanThanhToan = h.TongTienHoaDon - h.TienDaCoc;
            foreach (decimal tien in new[] { h.TongTienPhong, h.TongTienDichVu, h.TongTienPhuThu, h.TongTienHoaDon })
                if (tien < 0 || tien > 999999999999999m) throw new InvalidOperationException("Số tiền vượt giới hạn lưu trữ (15 chữ số).");
            return h;
        }

        public bool XemTruoc(HoaDon nhap, out HoaDon kq, out string loi)
        {
            kq = null; loi = null;
            try { kq = TinhLai(nhap); return true; }
            catch (Exception ex) { loi = MyConnection.ThongBaoLoi(ex); return false; }
        }

        public bool ThanhToan(HoaDon daXem, out HoaDon kq, out string loi)
        {
            kq = null; loi = null;
            if (daXem == null) { loi = "Vui lòng xem trước hóa đơn trước khi thanh toán."; return false; }
            try
            {
                MyConnection.BeginTransaction();
                dao.KhoaPhieu(daXem.MaPhieu);
                var h = TinhLai(daXem);
                // Nếu giá/cọc/khuyến mãi thay đổi sau khi xem trước, yêu cầu xem lại trước khi thu tiền.
                if (h.MaKH != daXem.MaKH || !CungChiTiet(h, daXem) || h.TongTienPhong != daXem.TongTienPhong || h.TongTienDichVu != daXem.TongTienDichVu ||
                    h.TongTienPhuThu != daXem.TongTienPhuThu || h.TienDaCoc != daXem.TienDaCoc || h.TienGiam != daXem.TienGiam)
                    throw new InvalidOperationException("Chi tiết hoặc số tiền đã thay đổi. Vui lòng tính lại hóa đơn trước khi thanh toán.");
                dao.Insert(h);
                dao.Checkout(h);
                MyConnection.Commit();
                kq = h;
                ds = null;
                return true;
            }
            catch (Exception ex) { MyConnection.Rollback(); loi = MyConnection.ThongBaoLoi(ex); return false; }
        }

        private static bool CungChiTiet(HoaDon moi, HoaDon cu)
        {
            if (moi.Phong.Count != cu.Phong.Count || moi.DichVu.Count != cu.DichVu.Count || moi.PhuThu.Count != cu.PhuThu.Count) return false;
            foreach (var p in moi.Phong)
                if (!cu.Phong.Exists(x => x.MaPhong == p.MaPhong && x.GiaPhong == p.GiaPhong && x.SoNgayO == p.SoNgayO && x.SoGioO == p.SoGioO)) return false;
            foreach (var d in moi.DichVu)
                if (!cu.DichVu.Exists(x => x.MaPhong == d.MaPhong && x.MaDV == d.MaDV && x.SoLuongSuDung == d.SoLuongSuDung && x.DonGiaSuDung == d.DonGiaSuDung)) return false;
            foreach (var p in moi.PhuThu)
                if (!cu.PhuThu.Exists(x => x.MaPhong == p.MaPhong && x.MaPhuThu == p.MaPhuThu && x.SoLuong == p.SoLuong && x.DonGia == p.DonGia)) return false;
            return true;
        }
    }
}
