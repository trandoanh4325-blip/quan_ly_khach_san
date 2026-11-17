using System;

namespace QLKS.DTO
{
    public class KhuyenMai
    {
        public string MaKM { get; set; }
        public string TenKM { get; set; }
        public string LoaiGiam { get; set; }
        public decimal GiaTriGiam { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public decimal TongThuToiThieu { get; set; }
        public int SoLuotToiToiThieu { get; set; }
    }
}
