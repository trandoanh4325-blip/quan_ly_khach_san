using System;using System.Data;
namespace QLKS.DAO {
public class ThongKeDAO {
 public DataTable ThuChi(DateTime from, DateTime to)=>MyConnection.ExecuteQuery(@"
 SELECT 'Tổng thu (hóa đơn)' chi_tieu, COALESCE(SUM(tong_tien_hoa_don),0) so_tien FROM hoa_don WHERE ngay_thanh_toan>=@f AND ngay_thanh_toan<@t
 UNION ALL SELECT 'Tổng chi (phiếu nhập)',COALESCE(SUM(tong_tien_nhap),0) FROM phieu_nhap WHERE ngay_nhap>=@f AND ngay_nhap<@t
 UNION ALL SELECT 'Lợi nhuận',
 (SELECT COALESCE(SUM(tong_tien_hoa_don),0) FROM hoa_don WHERE ngay_thanh_toan>=@f AND ngay_thanh_toan<@t)-
 (SELECT COALESCE(SUM(tong_tien_nhap),0) FROM phieu_nhap WHERE ngay_nhap>=@f AND ngay_nhap<@t)",MyConnection.P("@f",from),MyConnection.P("@t",to));
 // 10a: Pivot theo quý. Với sản phẩm, chỉ cộng chi tiết phụ thu, KHÔNG lấy tổng hóa đơn.
 public DataTable TheoQuy(string key, int year) {
 string from,expr,name;
 if(key=="Nhân viên") {from="hoa_don h JOIN nhan_vien n ON n.ma_nv=h.ma_nv";expr="h.tong_tien_hoa_don";name="CONCAT(n.ma_nv,' - ',n.ho_ten)";}
 else if(key=="Khách hàng") {from="hoa_don h JOIN khach_hang n ON n.ma_kh=h.ma_kh";expr="h.tong_tien_hoa_don";name="CONCAT(n.ma_kh,' - ',n.ho_ten)";}
 else {from="hoa_don h JOIN chi_tiet_phu_thu c ON c.ma_hoa_don=h.ma_hoa_don JOIN phu_thu pt ON pt.ma_phu_thu=c.ma_phu_thu JOIN san_pham s ON s.ma_sp=pt.ma_sp";expr="c.thanh_tien";name="CONCAT(s.ma_sp,' - ',s.ten_sp)";}
 string sql=$@"SELECT {name} doi_tuong,
 COALESCE(SUM(CASE WHEN QUARTER(h.ngay_thanh_toan)=1 THEN {expr} ELSE 0 END),0) Q1,
 COALESCE(SUM(CASE WHEN QUARTER(h.ngay_thanh_toan)=2 THEN {expr} ELSE 0 END),0) Q2,
 COALESCE(SUM(CASE WHEN QUARTER(h.ngay_thanh_toan)=3 THEN {expr} ELSE 0 END),0) Q3,
 COALESCE(SUM(CASE WHEN QUARTER(h.ngay_thanh_toan)=4 THEN {expr} ELSE 0 END),0) Q4,
 COALESCE(SUM({expr}),0) tong_cong FROM {from} WHERE YEAR(h.ngay_thanh_toan)=@nam GROUP BY 1 ORDER BY 1";
 return MyConnection.ExecuteQuery(sql,MyConnection.P("@nam",year));
 }
 public DataTable TheoThoiGian(string key,string kieu,DateTime from,DateTime to) {
 string group=kieu switch {"Ngày"=>"DATE(h.ngay_thanh_toan)","Tháng"=>"DATE_FORMAT(h.ngay_thanh_toan,'%Y-%m')","Quý"=>"CONCAT(YEAR(h.ngay_thanh_toan),'-Q',QUARTER(h.ngay_thanh_toan))","Năm"=>"YEAR(h.ngay_thanh_toan)",_=>"'Trong khoảng'"};
 string source,entity,amount;
 if(key=="Nhân viên") {source="hoa_don h JOIN nhan_vien n ON h.ma_nv=n.ma_nv";entity="CONCAT(n.ma_nv,' - ',n.ho_ten)";amount="h.tong_tien_hoa_don";}
 else if(key=="Khách hàng") {source="hoa_don h JOIN khach_hang n ON h.ma_kh=n.ma_kh";entity="CONCAT(n.ma_kh,' - ',n.ho_ten)";amount="h.tong_tien_hoa_don";}
 else {source="hoa_don h JOIN chi_tiet_phu_thu c ON c.ma_hoa_don=h.ma_hoa_don JOIN phu_thu pt ON pt.ma_phu_thu=c.ma_phu_thu JOIN san_pham n ON n.ma_sp=pt.ma_sp";entity="CONCAT(n.ma_sp,' - ',n.ten_sp)";amount="c.thanh_tien";}
 return MyConnection.ExecuteQuery($"SELECT {group} thoi_gian,{entity} doi_tuong,SUM({amount}) tong_tien FROM {source} WHERE h.ngay_thanh_toan>=@f AND h.ngay_thanh_toan<@t GROUP BY 1,2 ORDER BY 1,2",MyConnection.P("@f",from),MyConnection.P("@t",to));
 }
 public DataTable HaiKhoa(DateTime from,DateTime to) => MyConnection.ExecuteQuery(@"SELECT CONCAT(h.ma_kh,' - ',k.ho_ten) khach_hang, CONCAT(s.ma_sp,' - ',s.ten_sp) san_pham,SUM(c.thanh_tien) tong_tien FROM hoa_don h JOIN khach_hang k ON h.ma_kh=k.ma_kh JOIN chi_tiet_phu_thu c ON c.ma_hoa_don=h.ma_hoa_don JOIN phu_thu pt ON pt.ma_phu_thu=c.ma_phu_thu JOIN san_pham s ON s.ma_sp=pt.ma_sp WHERE h.ngay_thanh_toan>=@f AND h.ngay_thanh_toan<@t GROUP BY 1,2 ORDER BY 1,2",MyConnection.P("@f",from),MyConnection.P("@t",to));
 public DataTable BaKhoa(DateTime from,DateTime to) => MyConnection.ExecuteQuery(@"SELECT CONCAT(h.ma_nv,' - ',n.ho_ten) nhan_vien, CONCAT(s.ma_sp,' - ',s.ten_sp) san_pham,YEAR(h.ngay_thanh_toan) nam,SUM(c.thanh_tien) tong_tien FROM hoa_don h JOIN nhan_vien n ON h.ma_nv=n.ma_nv JOIN chi_tiet_phu_thu c ON c.ma_hoa_don=h.ma_hoa_don JOIN phu_thu pt ON pt.ma_phu_thu=c.ma_phu_thu JOIN san_pham s ON s.ma_sp=pt.ma_sp WHERE h.ngay_thanh_toan>=@f AND h.ngay_thanh_toan<@t GROUP BY 1,2,3 ORDER BY 3,1,2",MyConnection.P("@f",from),MyConnection.P("@t",to));
 }
}
