using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace QLKS.DAO
{
    /// <summary>
    /// Mô hình 3 lớp ver 2: toàn bộ ứng dụng dùng CHUNG 1 kết nối.
    /// Open() được gọi 1 lần ở Program.cs (KetNoiBUS.KhoiTao). Các DAO chỉ gọi ExecuteXxx.
    /// </summary>
    public static class MyConnection
    {
        // CHUỖI KẾT NỐI - chỉ sửa ở đây (đổi Pwd nếu MySQL của bạn có mật khẩu).
        // Lưu ý tiếng Việt: GV yêu cầu "?useUnicode=yes&characterEncoding=UTF-8" (cú pháp JDBC/Java).
        // Với C# + MySql.Data, tùy chọn tương đương là CharSet=utf8mb4 (JDBC option sẽ gây lỗi).
        public const string ConnectionString =
            "Server=localhost;Port=3306;Database=qlks;Uid=root;Pwd=;CharSet=utf8mb4;";

        private static MySqlConnection _conn;
        private static MySqlTransaction _tran;

        public static void Open()
        {
            if (_conn == null) _conn = new MySqlConnection(ConnectionString);
            if (_conn.State != ConnectionState.Open) _conn.Open();
        }

        public static void Close()
        {
            if (_conn != null && _conn.State == ConnectionState.Open) _conn.Close();
        }

        private static MySqlCommand TaoLenh(string sql, MySqlParameter[] ps)
        {
            Open(); // an toàn: nếu đã mở thì không làm gì
            var cmd = new MySqlCommand(sql, _conn, _tran);
            if (ps != null && ps.Length > 0) cmd.Parameters.AddRange(ps);
            return cmd;
        }

        public static DataTable ExecuteQuery(string sql, params MySqlParameter[] ps)
        {
            var dt = new DataTable();
            using (var cmd = TaoLenh(sql, ps))
            using (var da = new MySqlDataAdapter(cmd))
            {
                da.Fill(dt);
            }
            return dt;
        }

        public static int ExecuteNonQuery(string sql, params MySqlParameter[] ps)
        {
            using (var cmd = TaoLenh(sql, ps))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        public static object ExecuteScalar(string sql, params MySqlParameter[] ps)
        {
            using (var cmd = TaoLenh(sql, ps))
            {
                return cmd.ExecuteScalar();
            }
        }

        // ---- Transaction: dùng khi 1 nghiệp vụ cập nhật nhiều bảng (vd thanh toán hóa đơn) ----
        public static void BeginTransaction()
        {
            Open();
            if (_tran == null) _tran = _conn.BeginTransaction();
        }

        public static void Commit()
        {
            if (_tran == null) return;
            _tran.Commit();
            _tran.Dispose();
            _tran = null;
        }

        public static void Rollback()
        {
            if (_tran == null) return;
            _tran.Rollback();
            _tran.Dispose();
            _tran = null;
        }

        /// <summary>Tạo tham số, null tự đổi thành NULL của SQL.</summary>
        public static MySqlParameter P(string ten, object giaTri)
        {
            return new MySqlParameter(ten, giaTri ?? DBNull.Value);
        }

        /// <summary>Đổi lỗi MySQL sang câu thông báo dễ hiểu.</summary>
        public static string ThongBaoLoi(Exception ex)
        {
            var my = ex as MySqlException;
            if (my != null)
            {
                switch (my.Number)
                {
                    case 1062: return "Dữ liệu bị trùng (mã hoặc giá trị duy nhất đã tồn tại).";
                    case 1451: return "Không thể xóa vì dữ liệu đang được sử dụng ở bảng khác.";
                    case 1452: return "Dữ liệu tham chiếu không tồn tại (sai khóa ngoại).";
                    case 3819: return "Dữ liệu vi phạm ràng buộc kiểm tra (CHECK) của CSDL.";
                }
            }
            return ex.Message;
        }
    }
}
