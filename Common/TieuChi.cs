namespace QLKS.Common
{
    /// <summary>1 điều kiện của tìm kiếm nâng cao.</summary>
    public class TieuChi
    {
        public string Field { get; set; }       // tên thuộc tính của DTO
        public string Op { get; set; }          // =, <>, >, >=, <, <=, chứa
        public string Value { get; set; }
        public string Logic { get; set; }       // AND / OR: nối với điều kiện ĐỨNG TRƯỚC nó
        public bool Not { get; set; }           // đảo kết quả của điều kiện này

        public TieuChi()
        {
            Op = "=";
            Value = "";
            Logic = "AND";
        }
    }
}
