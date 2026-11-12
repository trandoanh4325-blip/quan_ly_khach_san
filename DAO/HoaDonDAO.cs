using System;
using System.Collections;
using System.Data;
using QLKS.DTO;

namespace QLKS.DAO
{
    public class HoaDonDAO
    {
        public ArrayList GetAll()
        {
            var ds = new ArrayList();
            foreach (DataRow r in MyConnection.ExecuteQuery(
                "SELECT h.*,k.ho_ten FROM hoa_don h JOIN khach_hang k ON k.ma_kh=h.ma_kh ORDER BY h.ngay_thanh_toan DESC,h.ma_hoa_don").Rows)
                ds.Add(new HoaDon {
                    MaHoaDon = r["ma_hoa_don"].ToString(), MaPhieu = r["ma_phieu"].ToString(),
                    MaKH = r["ma_kh"].ToString(), TenKH = r["ho_ten"].ToString(), MaNV = r["ma_nv"].ToString(),
                    NgayThanhToan = Convert.ToDateTime(r["ngay_thanh_toan"]), MaKM = r["ma_km"] == DBNull.Value ? null : r["ma_km"].ToString(),
                    TienDaCoc = Convert.ToDecimal(r["tien_da_coc"]), TienGiam = Convert.ToDecimal(r["tien_giam"]),
                    TongTienPhong = Convert.ToDecimal(r["tong_tien_phong"]), TongTienDichVu = Convert.ToDecimal(r["tong_tien_dich_vu"]),
                    TongTienPhuThu = Convert.ToDecimal(r["tong_tien_phu_thu"]), TongTienHoaDon = Convert.ToDecimal(r["tong_tien_hoa_don"]),
                    TienCanThanhToan = Convert.ToDecimal(r["tien_can_thanh_toan"]) });
            return ds;
        }

        public void DocChiTiet(HoaDon h)
        {
            h.Phong.Clear(); h.DichVu.Clear(); h.PhuThu.Clear();
            foreach (ChiTietDatPhong ct in new ChiTietDatPhongDAO().GetAll())
                if (ct.MaPhieu == h.MaPhieu) h.Phong.Add(ct);
            foreach (DataRow r in MyConnection.ExecuteQuery(
                "SELECT c.*,d.ten_dv FROM chi_tiet_su_dung_dich_vu c JOIN dich_vu d ON d.ma_dv=c.ma_dv WHERE ma_hoa_don=@ma",
                MyConnection.P("@ma", h.MaHoaDon)).Rows)
                h.DichVu.Add(new ChiTietSuDungDichVu { MaPhong = r["ma_phong"].ToString(), MaDV = r["ma_dv"].ToString(),
                    TenDV = r["ten_dv"].ToString(), SoLuongSuDung = Convert.ToInt32(r["so_luong_su_dung"]),
                    DonGiaSuDung = Convert.ToDecimal(r["don_gia_su_dung"]), ThanhTien = Convert.ToDecimal(r["thanh_tien"]) });
            foreach (DataRow r in MyConnection.ExecuteQuery(
                "SELECT c.*,p.ten_phu_thu FROM chi_tiet_phu_thu c JOIN phu_thu p ON p.ma_phu_thu=c.ma_phu_thu WHERE ma_hoa_don=@ma",
                MyConnection.P("@ma", h.MaHoaDon)).Rows)
                h.PhuThu.Add(new ChiTietPhuThu { MaPhong = r["ma_phong"].ToString(), MaPhuThu = r["ma_phu_thu"].ToString(),
                    TenPhuThu = r["ten_phu_thu"].ToString(), SoLuong = Convert.ToInt32(r["so_luong"]),
                    DonGia = Convert.ToDecimal(r["don_gia"]), ThanhTien = Convert.ToDecimal(r["thanh_tien"]), GhiChu = r["ghi_chu"].ToString() });
        }

        public ArrayList PhuThu()
        {
            var ds = new ArrayList();
            foreach (DataRow r in MyConnection.ExecuteQuery(
                "SELECT p.ma_phu_thu,p.ten_phu_thu,s.gia_ban FROM phu_thu p JOIN san_pham s ON s.ma_sp=p.ma_sp ORDER BY p.ma_phu_thu").Rows)
                ds.Add(new LuaChonPhuThu { MaPhuThu = r["ma_phu_thu"].ToString(), TenPhuThu = r["ten_phu_thu"].ToString(), DonGia = Convert.ToDecimal(r["gia_ban"]) });
            return ds;
        }

        public ArrayList KhuyenMai()
        {
            var ds = new ArrayList();
            foreach (DataRow r in MyConnection.ExecuteQuery("SELECT * FROM khuyen_mai ORDER BY ma_km").Rows)
                ds.Add(new KhuyenMaiThanhToan { MaKM = r["ma_km"].ToString(), TenKM = r["ten_km"].ToString(), LoaiGiam = r["loai_giam"].ToString(),
                    GiaTriGiam = Convert.ToDecimal(r["gia_tri_giam"]), NgayBatDau = Convert.ToDateTime(r["ngay_bat_dau"]),
                    NgayKetThuc = Convert.ToDateTime(r["ngay_ket_thuc"]), TongThuToiThieu = Convert.ToDecimal(r["tong_thu_toi_thieu"]),
                    SoLuotToiToiThieu = Convert.ToInt32(r["so_luot_toi_toi_thieu"]) });
            return ds;
        }

        // Khóa phiếu trước khi đọc lại dữ liệu và tạo hóa đơn: chỉ một lần thanh toán được commit.
        public void KhoaPhieu(string ma)
        {
            if (MyConnection.ExecuteQuery("SELECT ma_phieu FROM phieu_dat_phong WHERE ma_phieu=@ma FOR UPDATE", MyConnection.P("@ma", ma)).Rows.Count == 0)
                throw new InvalidOperationException("Phiếu đặt phòng không còn tồn tại.");
            MyConnection.ExecuteQuery("SELECT ma_phong FROM chi_tiet_dat_phong WHERE ma_phieu=@ma FOR UPDATE", MyConnection.P("@ma", ma));
        }

        public DataRow KhachHang(string ma)
        {
            var dt = MyConnection.ExecuteQuery("SELECT * FROM khach_hang WHERE ma_kh=@ma FOR UPDATE", MyConnection.P("@ma", ma));
            if (dt.Rows.Count == 0) throw new InvalidOperationException("Khách hàng không còn tồn tại.");
            return dt.Rows[0];
        }

        public void Insert(HoaDon h)
        {
            h.MaHoaDon = MaTuDongDAO.Sinh("HD", "hoa_don", "ma_hoa_don");
            MyConnection.ExecuteNonQuery(
                "INSERT INTO hoa_don (ma_hoa_don,ma_phieu,ma_kh,ma_nv,ngay_thanh_toan,ma_km,tien_da_coc,tien_giam," +
                "tong_tien_phong,tong_tien_dich_vu,tong_tien_phu_thu,tong_tien_hoa_don,tien_can_thanh_toan) " +
                "VALUES (@ma,@pd,@kh,@nv,@ngay,@km,@coc,@giam,@phong,@dv,@pt,@tong,@con)",
                MyConnection.P("@ma", h.MaHoaDon), MyConnection.P("@pd", h.MaPhieu), MyConnection.P("@kh", h.MaKH),
                MyConnection.P("@nv", h.MaNV), MyConnection.P("@ngay", h.NgayThanhToan), MyConnection.P("@km", h.MaKM),
                MyConnection.P("@coc", h.TienDaCoc), MyConnection.P("@giam", h.TienGiam), MyConnection.P("@phong", h.TongTienPhong),
                MyConnection.P("@dv", h.TongTienDichVu), MyConnection.P("@pt", h.TongTienPhuThu),
                MyConnection.P("@tong", h.TongTienHoaDon), MyConnection.P("@con", h.TienCanThanhToan));
            foreach (var d in h.DichVu)
                MyConnection.ExecuteNonQuery("INSERT INTO chi_tiet_su_dung_dich_vu VALUES (@hd,@ph,@dv,@sl,@gia,@tt)",
                    MyConnection.P("@hd", h.MaHoaDon), MyConnection.P("@ph", d.MaPhong), MyConnection.P("@dv", d.MaDV),
                    MyConnection.P("@sl", d.SoLuongSuDung), MyConnection.P("@gia", d.DonGiaSuDung), MyConnection.P("@tt", d.ThanhTien));
            foreach (var p in h.PhuThu)
                MyConnection.ExecuteNonQuery("INSERT INTO chi_tiet_phu_thu VALUES (@hd,@ph,@pt,@sl,@gia,@tt,@gc)",
                    MyConnection.P("@hd", h.MaHoaDon), MyConnection.P("@ph", p.MaPhong), MyConnection.P("@pt", p.MaPhuThu),
                    MyConnection.P("@sl", p.SoLuong), MyConnection.P("@gia", p.DonGia), MyConnection.P("@tt", p.ThanhTien), MyConnection.P("@gc", p.GhiChu));
        }

        public void Checkout(HoaDon h)
        {
            var phongDao = new PhongDAO();
            foreach (var p in h.Phong)
            {
                int soDong = MyConnection.ExecuteNonQuery(
                    "UPDATE chi_tiet_dat_phong SET ngay_gio_checkout=@ngay,trang_thai='Đã checkout',so_ngay_o=@sn,so_gio_o=@sg,thanh_tien=@tt " +
                    "WHERE ma_phieu=@pd AND ma_phong=@ph AND trang_thai='Đang ở'",
                    MyConnection.P("@ngay", h.NgayThanhToan), MyConnection.P("@sn", p.SoNgayO), MyConnection.P("@sg", p.SoGioO),
                    MyConnection.P("@tt", p.ThanhTien), MyConnection.P("@pd", h.MaPhieu), MyConnection.P("@ph", p.MaPhong));
                if (soDong != 1) throw new InvalidOperationException("Trạng thái phòng đã thay đổi. Vui lòng tải lại phiếu.");
                // Không giải phóng phòng còn được một phiếu khác giữ.
                int conGiu = Convert.ToInt32(MyConnection.ExecuteScalar(
                    "SELECT COUNT(*) FROM chi_tiet_dat_phong WHERE ma_phong=@ph AND trang_thai='Đang ở'", MyConnection.P("@ph", p.MaPhong)));
                phongDao.DoiTrangThai(p.MaPhong, conGiu == 0 ? "Trống" : "Đã đủ");
            }
            MyConnection.ExecuteNonQuery("UPDATE khach_hang SET tong_thu=tong_thu+@tt,so_luot_toi=so_luot_toi+1 WHERE ma_kh=@kh",
                MyConnection.P("@tt", h.TongTienHoaDon), MyConnection.P("@kh", h.MaKH));
        }
    }
}
