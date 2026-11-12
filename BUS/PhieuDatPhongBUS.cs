using System;
using System.Collections;
using System.Collections.Generic;
using QLKS.Common;
using QLKS.DAO;
using QLKS.DTO;

namespace QLKS.BUS
{
    public class PhieuDatPhongBUS
    {
        public static readonly string[] TrangThaiCocChoPhep = { "Chưa cọc", "Đã cọc" };

        private readonly PhieuDatPhongDAO dao = new PhieuDatPhongDAO();
        private readonly ChiTietDatPhongDAO ctDao = new ChiTietDatPhongDAO();
        private readonly PhongDAO phongDao = new PhongDAO();
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

        public PhieuDatPhong TimTheoMa(string ma)
        {
            foreach (PhieuDatPhong p in DanhSach) if (p.MaPhieu == ma) return p;
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

        /// <summary>Chỉ nhân viên có chức vụ "Lễ tân" mới được lập phiếu đặt phòng: danh sách cho nút "..." chọn nhân viên.</summary>
        public ArrayList NhanVienLeTan()
        {
            var nvBus = new NhanVienBUS();
            var kq = new ArrayList();
            foreach (NhanVien n in nvBus.DanhSach)
                if (n.ChucVu != null && string.Equals(n.ChucVu.Trim(), "Lễ tân", StringComparison.CurrentCultureIgnoreCase))
                    kq.Add(n);
            return kq;
        }

        // ---------- Giờ chuẩn lấy từ bảng tham số (không viết cứng trong code) ----------
        public static TimeSpan GioNhanPhong()
        {
            return DocGio(ThamSoBUS.Doc("gio_nhan_phong", "14:00"), new TimeSpan(14, 0, 0));
        }

        public static TimeSpan GioTraPhong()
        {
            return DocGio(ThamSoBUS.Doc("gio_tra_phong", "12:00"), new TimeSpan(12, 0, 0));
        }

        private static TimeSpan DocGio(string s, TimeSpan macDinh)
        {
            TimeSpan t;
            return TimeSpan.TryParse(s, out t) ? t : macDinh;
        }

        /// <summary>
        /// Check-out dự kiến. Theo giờ: check-in + số giờ nghỉ. Theo ngày: (ngày check-in + số ngày) lúc giờ trả phòng chuẩn.
        /// </summary>
        public static DateTime TinhCheckout(DateTime checkin, bool theoGio, int soNgay, int soGio)
        {
            if (theoGio) return checkin.AddHours(soGio);
            return checkin.Date.AddDays(soNgay) + GioTraPhong();
        }

        // ---------- Kiểm tra dữ liệu ----------
        private static void ChuanHoa(PhieuDatPhong p)
        {
            p.MaPhieu = p.MaPhieu == null ? "" : p.MaPhieu.Trim();
            if (p.OTheoGio == 1) p.SoNgayODuKien = 0;        // theo giờ: số ngày không dùng
            else p.SoGioNghi = 0;                            // theo ngày: số giờ không dùng
            p.NgayCheckout = TinhCheckout(p.NgayCheckin, p.OTheoGio == 1, p.SoNgayODuKien, p.SoGioNghi);
        }

        // Trả về null nếu hợp lệ, ngược lại trả về câu báo lỗi. tong = tổng tiền dự kiến (để xét tiền cọc)
        private string Kiem(PhieuDatPhong p, bool them, decimal tong)
        {
            if (Validator.Rong(p.MaKH)) return "Vui lòng chọn khách hàng (nút ...).";
            if (Validator.Rong(p.MaNV)) return "Vui lòng chọn nhân viên lập phiếu (nút ...).";
            if (p.OTheoGio != 1 && p.OTheoGio != 2) return "Ở theo giờ chỉ được chọn 1 (Có) hoặc 2 (Không).";
            if (p.OTheoGio == 2 && p.SoNgayODuKien < 1) return "Số ngày ở dự kiến phải từ 1 trở lên.";
            if (p.OTheoGio == 1 && p.SoGioNghi < 1) return "Số giờ nghỉ phải từ 1 trở lên.";
            if (them && p.NgayCheckin.Date < DateTime.Today) return "Ngày check-in không được ở quá khứ.";
            if (them && TimTheoMa(p.MaPhieu) != null) return "Mã phiếu đã tồn tại.";

            if (Array.IndexOf(TrangThaiCocChoPhep, p.TrangThaiCoc) < 0) return "Trạng thái cọc chỉ được chọn \"Chưa cọc\" hoặc \"Đã cọc\".";
            if (p.TienCoc < 0) return "Tiền cọc phải là số không âm.";
            if (p.TrangThaiCoc == "Chưa cọc" && p.TienCoc != 0) return "Trạng thái \"Chưa cọc\" thì tiền cọc phải bằng 0.";
            if (p.TrangThaiCoc == "Đã cọc")
            {
                if (p.TienCoc <= 0) return "Trạng thái \"Đã cọc\" thì tiền cọc phải lớn hơn 0.";
                if (tong > 0)
                {
                    decimal tyLe = ThamSoBUS.DocDecimal("ty_le_coc_toi_thieu", 30);
                    decimal toiThieu = Math.Ceiling(tong * tyLe / 100m);
                    if (p.TienCoc < toiThieu)
                        return "Tiền cọc tối thiểu là " + toiThieu.ToString("N0") + " (" + tyLe.ToString("0.##") + "% tổng tiền dự kiến).";
                    if (p.TienCoc > tong) return "Tiền cọc không được lớn hơn tổng tiền dự kiến (" + tong.ToString("N0") + ").";
                }
            }
            return null;
        }

        public bool Them(PhieuDatPhong p, out string loi)
        {
            ChuanHoa(p);
            loi = Kiem(p, true, 0);
            if (loi != null) return false;
            p.NgayTao = DateTime.Now;
            p.TongTienDuKien = 0;
            try
            {
                MyConnection.BeginTransaction();
                dao.Insert(p);
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

        public bool Sua(PhieuDatPhong p, out string loi)
        {
            ChuanHoa(p);
            PhieuDatPhong cu = TimTheoMa(p.MaPhieu);
            if (cu == null) { loi = "Không tìm thấy phiếu cần sửa."; return false; }
            if (cu.TinhTrang == PhieuDatPhong.DaThanhToan)
            {
                loi = "Phiếu đã có hóa đơn, không thể sửa.";
                return false;
            }

            // Đổi hình thức ở (ngày/giờ) hoặc số ngày/giờ thì giá và thành tiền của từng phòng tính lại
            var cts = new ArrayList();
            decimal tongMoi = 0;
            foreach (ChiTietDatPhong ct in ctDao.GetAll())
            {
                if (ct.MaPhieu != p.MaPhieu || ct.TrangThai != ChiTietDatPhong.DangO) continue;
                ChiTietDatPhongBUS.ApDungGia(ct, p);
                tongMoi += ct.ThanhTien;
                cts.Add(ct);
            }

            loi = Kiem(p, false, tongMoi);
            if (loi != null) return false;
            p.NgayTao = cu.NgayTao;
            try
            {
                MyConnection.BeginTransaction();
                dao.Update(p);
                foreach (ChiTietDatPhong ct in cts) ctDao.Update(ct);
                dao.CapNhatTongTien(p.MaPhieu);
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
            PhieuDatPhong cu = TimTheoMa(ma);
            if (cu == null) { loi = "Không tìm thấy phiếu cần xóa."; return false; }
            if (cu.TinhTrang == PhieuDatPhong.DaThanhToan)
            {
                loi = "Phiếu đã có hóa đơn, không thể xóa.";
                return false;
            }
            try
            {
                MyConnection.BeginTransaction();
                // trả các phòng đang giữ của phiếu về "Trống" (loại phòng tự tăng "số lượng đang có")
                foreach (ChiTietDatPhong ct in ctDao.GetAll())
                    if (ct.MaPhieu == ma && ct.TrangThai == ChiTietDatPhong.DangO)
                        phongDao.DoiTrangThai(ct.MaPhong, "Trống");
                dao.Delete(ma);
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
