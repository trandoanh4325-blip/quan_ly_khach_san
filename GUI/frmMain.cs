// =====================================================================
// CODE-BEHIND cho form frmMain (vẽ bằng Designer) - LÀM CUỐI CÙNG, sau khi đã có
// đủ frmNhanVien, frmKhachHang, frmThamSo.
// Control cần có (đặt đúng tên):
//   lblTieuDe, pnlMenu, btnNhanVien, btnKhachHang, btnThamSo, pnlNoiDung, lblChao
// Thêm chức năng mới: vẽ thêm 1 Button vào pnlMenu rồi thêm 1 dòng Click ở hàm khởi tạo.
// =====================================================================
using System;
using System.Drawing;
using System.Windows.Forms;
using QLKS.BUS;

namespace QLKS.GUI
{
    public partial class frmMain : Form
    {
        private Form formCon;
        private Button nutDangChon;

        public frmMain()
        {
            InitializeComponent();
            Load += frmMain_Load;

            // ---- TV1 ----
            btnNhanVien.Click += (s, e) => MoForm(new frmNhanVien(), btnNhanVien);
            btnKhachHang.Click += (s, e) => MoForm(new frmKhachHang(), btnKhachHang);
            btnThamSo.Click += (s, e) => MoForm(new frmThamSo(), btnThamSo);

            // ---- TV2 (bỏ comment khi đã vẽ nút và có form) ----
            // btnLoaiPhong.Click += (s, e) => MoForm(new frmLoaiPhong(), btnLoaiPhong);
            // btnPhong.Click += (s, e) => MoForm(new frmPhong(), btnPhong);
            // btnPhieuDatPhong.Click += (s, e) => MoForm(new frmPhieuDatPhong(), btnPhieuDatPhong);

            // ---- TV3 ----
            // btnDichVu.Click += (s, e) => MoForm(new frmDichVu(), btnDichVu);
            // btnHoaDon.Click += (s, e) => MoForm(new frmHoaDon(), btnHoaDon);

            // ---- TV4 ----
            // btnSanPham.Click += (s, e) => MoForm(new frmSanPham(), btnSanPham);
            // btnThongKe.Click += (s, e) => MoForm(new frmThongKe(), btnThongKe);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            // Tên khách sạn lấy từ bảng tham số
            lblTieuDe.Text = ThamSoBUS.Doc("ten_khach_san", "QUẢN LÝ KHÁCH SẠN").ToUpper();
            
        }

        // Mở 1 form con vào vùng nội dung bên phải
        private void MoForm(Form f, Button nut)
        {
            if (nutDangChon != null) nutDangChon.BackColor = SystemColors.Control;
            nut.BackColor = Color.FromArgb(217, 226, 243);
            nutDangChon = nut;

            if (formCon != null)
            {
                pnlNoiDung.Controls.Remove(formCon);
                formCon.Dispose();
            }
            

            formCon = f;
            f.TopLevel = false;
            f.FormBorderStyle = FormBorderStyle.None;
            f.Dock = DockStyle.Fill;
            pnlNoiDung.Controls.Add(f);
            f.BringToFront();
            f.Show();
        }
    }
}
