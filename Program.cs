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
                
                KetNoiBUS.KhoiTao();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Không kết nối được CSDL.\n\n" +
                    "- Kiểm tra XAMPP.\n" +
                    "- Kiểm tra Database/qlks.sql.\n" +
                    "- Kiểm tra kết nối DAO/MyConnection.cs.\n\n" + ex.Message,
                    "Lỗi kết nối", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Application.Run(new frmMain());
            KetNoiBUS.Dong();
        }
    }
}
