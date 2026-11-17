
namespace QLKS.DTO
{
    public class SanPham
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public string MaLoaiSP { get; set; }
        public string DonViTinh { get; set; }
        public decimal GiaBan { get; set; }

        // Tồn kho tự cập nhật qua nghiệp vụ,
        // không cho nhập trực tiếp trên Form
        public int SoLuongTon { get; set; }
    }
}
