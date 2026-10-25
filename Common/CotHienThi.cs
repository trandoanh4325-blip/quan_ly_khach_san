namespace QLKS.Common
{
    /// <summary>Mô tả 1 cột hiển thị (tiêu đề, tên thuộc tính của DTO, độ rộng). Dùng cho ListView và tìm kiếm nâng cao.</summary>
    public class CotHienThi
    {
        public string TieuDe { get; private set; }
        public string Prop { get; private set; }
        public int Rong { get; private set; }

        public CotHienThi(string tieuDe, string prop, int rong = 120)
        {
            TieuDe = tieuDe;
            Prop = prop;
            Rong = rong;
        }
    }
}
