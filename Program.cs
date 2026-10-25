using System;
using System.Windows.Forms;
using QLKS.BUS;
using QLKS.GUI;

namespace QLKS
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                // Kết nối CSDL ĐÚNG 1 LẦN khi khởi động (MyConnection dùng lại suốt chương trình)
                KetNoiBUS.KhoiTao();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không kết nối được CSDL.\n\n" +
                    "- Kiểm tra XAMPP đã bật MySQL chưa.\n" +
                    "- Kiểm tra đã import Database/qlks.sql chưa.\n" +
                    "- Kiểm tra chuỗi kết nối trong DAO/MyConnection.cs.\n\n" + ex.Message,
                    "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.Run(new frmMain());
            KetNoiBUS.Dong();
        }
    }
}
