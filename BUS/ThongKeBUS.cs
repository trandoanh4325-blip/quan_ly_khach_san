using System;using System.Data;using QLKS.DAO;
namespace QLKS.BUS { public class ThongKeBUS {readonly ThongKeDAO dao=new ThongKeDAO();
 public DataTable BaoCao(string loai,string doiTuong,string thoiGian,DateTime tu,DateTime den,int nam) {
 if(tu.Date>den.Date)throw new ArgumentException("Ngày bắt đầu phải trước hoặc bằng ngày kết thúc.");
 DateTime from=tu.Date,to=den.Date.AddDays(1);
 return loai switch {
 "10a - Thu/Chi/Lợi nhuận"=>dao.ThuChi(from,to),
 "10a - Bảng chéo quý"=>dao.TheoQuy(doiTuong,nam),
 "10b - Theo thời gian"=>dao.TheoThoiGian(doiTuong,thoiGian,from,to),
 "10b - Hai khóa"=>dao.HaiKhoa(from,to),
 "10b - Ba khóa"=>dao.BaKhoa(from,to),
 _=>throw new ArgumentException("Chọn loại thống kê hợp lệ.")
 }; }
 } }
