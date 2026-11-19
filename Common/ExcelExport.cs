using System;
using System.Data;
using System.Windows.Forms;
using QLKS.DAO;

namespace QLKS.Common
{
    public static class ExcelExport
    {
        public static void XuatBang(IWin32Window owner, string tableName, string fileName, string sheetName)
        {
            try
            {
                using (var dialog = new SaveFileDialog
                {
                    Filter = "Excel (*.xlsx)|*.xlsx",
                    FileName = fileName + ".xlsx",
                    Title = "Xuất dữ liệu Excel"
                })
                {
                    if (dialog.ShowDialog(owner) != DialogResult.OK)
                        return;

                    DataTable data = MyConnection.ExecuteQuery("SELECT * FROM " + tableName);
                    ExcelHelper.Xuat(dialog.FileName, data, sheetName);
                    MessageBox.Show(owner, "Đã xuất Excel: " + data.Rows.Count + " dòng.", "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(owner, ex.Message, "Xuất Excel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
