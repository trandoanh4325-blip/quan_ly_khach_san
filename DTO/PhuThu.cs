namespace QLKS.DTO
{
    public class PhuThu : LuaChonPhuThu
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public string DonViTinh { get; set; }
    }

    public class SanPhamPhuThu
    {
        public string MaSP { get; set; }
        public string TenSP { get; set; }
        public string DonViTinh { get; set; }
        public decimal GiaBan { get; set; }
    }
}
