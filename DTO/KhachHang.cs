namespace QLKS.DTO
{
    public class KhachHang
    {
        public string MaKH { get; set; }
        public string HoTen { get; set; }
        public string CCCD { get; set; }
        public string GioiTinh { get; set; }
        public decimal TongThu { get; set; }   // read-only trên form, tự cộng sau thanh toán
        public int SoLuotToi { get; set; }     // read-only trên form, tự +1 sau thanh toán
    }
}
