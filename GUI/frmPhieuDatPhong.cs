using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using QLKS.BUS;
using QLKS.Common;
using QLKS.DTO;

namespace QLKS.GUI
{
    /// <summary>
    /// Phiếu đặt phòng (main) - Chi tiết đặt phòng (sub, ListView thứ 2) - Chi tiết khách ở (lưới nhập khách).
    /// Giao diện dựng bằng code (giống frmChon) nên không có file .Designer.
    /// </summary>
    public class frmPhieuDatPhong : Form
    {
        private const string TuSinh = "(tự sinh)";

        private readonly PhieuDatPhongBUS bus = new PhieuDatPhongBUS();
        private readonly ChiTietDatPhongBUS ctBus = new ChiTietDatPhongBUS();
        private readonly KhachHangBUS khBus = new KhachHangBUS();
        private readonly NhanVienBUS nvBus = new NhanVienBUS();

        private PhieuDatPhong phieuChon;     // phiếu đang chọn ở ListView chính
        private ChiTietDatPhong ctChon;      // dòng chi tiết đang chọn (null = đang nhập phòng mới)
        private KhachHang khChon;            // khách hàng chọn trong form phiếu (nút ...)
        private NhanVien nvChon;             // nhân viên chọn trong form phiếu (nút ...)
        private KhachHang khachDat;          // khách đứng tên phiếu đang chọn (để tự điền người thứ nhất)
        private Phong phongChon;             // phòng chọn bằng nút ... khi thêm phòng mới
        private bool dangThemPhieu, dangNhapPhieu, dangNap, row0LaKhachDat;

        // ---- control ----
        private Panel host, pnlThan, pnlChiTiet;
        private ucTimKiemNangCao ucTim;
        private ListView lvPhieu, lvChiTiet;
        private TextBox txtTim, txtMaPhieu, txtKhach, txtNhanVien, txtCheckout, txtTienCoc, txtTong;
        private TextBox txtPhong, txtToiDa, txtGia, txtThanhTien, txtTrangThaiCT;
        private Button btnNangCao, btnChonKH, btnChonNV, btnThemPhieu, btnSuaPhieu, btnXoaPhieu, btnLuuPhieu, btnHuyPhieu;
        private Button btnChonPhong, btnThemPhong, btnSuaPhong, btnXoaPhong, btnPhongMoi;
        private DateTimePicker dtpNgayTao, dtpCheckin;
        private ComboBox cboTheoGio, cboCoc;
        private NumericUpDown nudNgay, nudGio, nudSoKhach;
        private CheckBox chkKhachDat;
        private DataGridView dgv;
        private Label lblTinhTrang;

        private readonly CotHienThi[] cotPhieu =
        {
            new CotHienThi("Mã phiếu", "MaPhieu", 80),
            new CotHienThi("Khách hàng", "TenKH", 150),
            new CotHienThi("Nhân viên", "TenNV", 130),
            new CotHienThi("Ngày tạo", "NgayTao", 120),
            new CotHienThi("Check-in", "NgayCheckin", 120),
            new CotHienThi("Check-out", "NgayCheckout", 120),
            new CotHienThi("Hình thức", "HinhThuc", 80),
            new CotHienThi("Tiền cọc", "TienCoc", 90),
            new CotHienThi("Trạng thái cọc", "TrangThaiCoc", 100),
            new CotHienThi("Tổng dự kiến", "TongTienDuKien", 110),
            new CotHienThi("Tình trạng", "TinhTrang", 110)
        };

        private readonly CotHienThi[] cotCT =
        {
            new CotHienThi("Mã phòng", "MaPhong", 90),
            new CotHienThi("Tên phòng", "TenPhong", 130),
            new CotHienThi("Số khách", "SoKhach", 80),
            new CotHienThi("Giá phòng", "GiaPhong", 110),
            new CotHienThi("Số ngày ở", "SoNgayO", 90),
            new CotHienThi("Số giờ ở", "SoGioO", 90),
            new CotHienThi("Thành tiền", "ThanhTien", 120),
            new CotHienThi("Trạng thái", "TrangThai", 100),
            new CotHienThi("Ngày/giờ checkout", "NgayGioCheckout", 150)
        };

        private readonly CotHienThi[] cotKhach =
        {
            new CotHienThi("Mã KH", "MaKH", 80),
            new CotHienThi("Họ và tên", "HoTen", 220),
            new CotHienThi("CCCD", "CCCD", 140),
            new CotHienThi("Giới tính", "GioiTinh", 90),
            new CotHienThi("Tổng thu", "TongThu", 120),
            new CotHienThi("Số lượt tới", "SoLuotToi", 100)
        };

        private readonly CotHienThi[] cotNV =
        {
            new CotHienThi("Mã NV", "MaNV", 90),
            new CotHienThi("Họ và tên", "HoTen", 240),
            new CotHienThi("Chức vụ", "ChucVu", 160),
            new CotHienThi("Số điện thoại", "SoDienThoai", 150)
        };

        private readonly CotHienThi[] cotPhong =
        {
            new CotHienThi("Mã phòng", "MaPhong", 100),
            new CotHienThi("Tên phòng", "TenPhong", 180),
            new CotHienThi("Loại phòng", "TenLoai", 160),
            new CotHienThi("Số người tối đa", "SoNguoiToiDa", 130),
            new CotHienThi("Trạng thái", "TrangThai", 110)
        };

        public frmPhieuDatPhong()
        {
            Font = new Font("Segoe UI", 10F);
            Text = "frmPhieuDatPhong";
            ClientSize = new Size(1058, 514);
            XayDung();
            Load += frmPhieuDatPhong_Load;
        }

        // =====================================================================
        //  DỰNG GIAO DIỆN
        // =====================================================================
        private static Label Nhan(Control cha, string text, int x, int y)
        {
            var l = new Label { Text = text, Left = x, Top = y + 4, AutoSize = true };
            cha.Controls.Add(l);
            return l;
        }

        private static TextBox Txt(Control cha, int x, int y, int w, bool chiDoc)
        {
            var t = new TextBox { Left = x, Top = y, Width = w, ReadOnly = chiDoc };
            cha.Controls.Add(t);
            return t;
        }

        private static Button Nut(Control cha, string text, int x, int y, int w, int h)
        {
            var b = new Button { Text = text, Left = x, Top = y, Width = w, Height = h, UseVisualStyleBackColor = true };
            cha.Controls.Add(b);
            return b;
        }

        private static ListView TaoLv(int x, int y, int w, int h)
        {
            return new ListView
            {
                Left = x, Top = y, Width = w, Height = h, View = View.Details, FullRowSelect = true,
                GridLines = true, MultiSelect = false, HideSelection = false
            };
        }

        private static DateTimePicker TaoDtp(Control cha, int x, int y, int w)
        {
            var d = new DateTimePicker { Left = x, Top = y, Width = w, Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm" };
            cha.Controls.Add(d);
            return d;
        }

        private void XayDung()
        {
            host = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
            Controls.Add(host);

            // ---- hàng tiêu đề + tìm kiếm ----
            host.Controls.Add(new Label
            {
                Text = "Quản Lý Phiếu Đặt Phòng", Left = 10, Top = 8, AutoSize = true,
                Font = new Font("Segoe UI", 13.8F, FontStyle.Bold)
            });
            Nhan(host, "Tìm Kiếm", 520, 10);
            txtTim = Txt(host, 590, 10, 300, false);
            btnNangCao = Nut(host, "Tìm kiếm Nâng Cao", 895, 8, 140, 30);

            ucTim = new ucTimKiemNangCao { Left = 10, Top = 48, Width = 1020, Visible = false };
            host.Controls.Add(ucTim);

            pnlThan = new Panel { Left = 0, Top = 48, Width = 1040, Height = 745 };
            host.Controls.Add(pnlThan);
            host.AutoScrollMinSize = new Size(1040, 48 + 745 + 10);

            // ---- phiếu đặt phòng (ListView chính) ----
            lvPhieu = TaoLv(10, 0, 1020, 150);
            pnlThan.Controls.Add(lvPhieu);

            Nhan(pnlThan, "Mã phiếu", 10, 160);
            txtMaPhieu = Txt(pnlThan, 100, 160, 110, true);
            Nhan(pnlThan, "Khách hàng", 225, 160);
            txtKhach = Txt(pnlThan, 320, 160, 185, true);
            btnChonKH = Nut(pnlThan, "...", 510, 159, 34, 28);
            Nhan(pnlThan, "Nhân viên", 560, 160);
            txtNhanVien = Txt(pnlThan, 640, 160, 185, true);
            btnChonNV = Nut(pnlThan, "...", 830, 159, 34, 28);

            Nhan(pnlThan, "Ngày tạo", 10, 196);
            dtpNgayTao = TaoDtp(pnlThan, 90, 196, 150);
            dtpNgayTao.Enabled = false;
            Nhan(pnlThan, "Check-in", 260, 196);
            dtpCheckin = TaoDtp(pnlThan, 335, 196, 160);
            Nhan(pnlThan, "Ở theo giờ", 515, 196);
            cboTheoGio = new ComboBox { Left = 595, Top = 196, Width = 170, DropDownStyle = ComboBoxStyle.DropDownList };
            cboTheoGio.Items.AddRange(new object[] { "1 - Có (theo giờ)", "2 - Không (theo ngày)" });
            pnlThan.Controls.Add(cboTheoGio);
            Nhan(pnlThan, "Check-out", 775, 196);
            txtCheckout = Txt(pnlThan, 850, 196, 150, true);

            Nhan(pnlThan, "Số ngày dự kiến", 10, 232);
            nudNgay = new NumericUpDown { Left = 125, Top = 232, Width = 60, Minimum = 0, Maximum = 365 };
            pnlThan.Controls.Add(nudNgay);
            Nhan(pnlThan, "Số giờ nghỉ", 205, 232);
            nudGio = new NumericUpDown { Left = 290, Top = 232, Width = 60, Minimum = 0, Maximum = 720 };
            pnlThan.Controls.Add(nudGio);
            Nhan(pnlThan, "Trạng thái cọc", 370, 232);
            cboCoc = new ComboBox { Left = 475, Top = 232, Width = 100, DropDownStyle = ComboBoxStyle.DropDownList };
            cboCoc.Items.AddRange(PhieuDatPhongBUS.TrangThaiCocChoPhep);
            pnlThan.Controls.Add(cboCoc);
            Nhan(pnlThan, "Tiền cọc", 595, 232);
            txtTienCoc = Txt(pnlThan, 665, 232, 120, false);
            Nhan(pnlThan, "Tổng dự kiến", 800, 232);
            txtTong = Txt(pnlThan, 895, 232, 125, true);

            btnThemPhieu = Nut(pnlThan, "Thêm", 10, 272, 95, 30);
            btnSuaPhieu = Nut(pnlThan, "Sửa", 115, 272, 95, 30);
            btnXoaPhieu = Nut(pnlThan, "Xóa", 220, 272, 95, 30);
            btnLuuPhieu = Nut(pnlThan, "Lưu", 325, 272, 95, 30);
            btnHuyPhieu = Nut(pnlThan, "Hủy", 430, 272, 95, 30);
            lblTinhTrang = new Label { Left = 545, Top = 278, AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold) };
            pnlThan.Controls.Add(lblTinhTrang);

            // ---- chi tiết đặt phòng (ListView phụ) ----
            pnlThan.Controls.Add(new Label
            {
                Text = "Chi tiết đặt phòng của phiếu đang chọn (bấm 1 dòng phiếu ở bảng trên để xem)",
                Left = 10, Top = 314, AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            });
            lvChiTiet = TaoLv(10, 338, 1020, 130);
            pnlThan.Controls.Add(lvChiTiet);

            pnlChiTiet = new Panel { Left = 0, Top = 474, Width = 1040, Height = 262 };
            pnlThan.Controls.Add(pnlChiTiet);

            Nhan(pnlChiTiet, "Phòng", 10, 0);
            txtPhong = Txt(pnlChiTiet, 70, 0, 200, true);
            btnChonPhong = Nut(pnlChiTiet, "...", 275, 0, 34, 28);
            Nhan(pnlChiTiet, "Số người tối đa", 330, 0);
            txtToiDa = Txt(pnlChiTiet, 445, 0, 50, true);
            Nhan(pnlChiTiet, "Số khách", 515, 0);
            nudSoKhach = new NumericUpDown { Left = 590, Top = 0, Width = 60, Minimum = 1, Maximum = 8, Value = 1 };
            pnlChiTiet.Controls.Add(nudSoKhach);
            chkKhachDat = new CheckBox { Text = "Khách đặt phiếu ở phòng này", Left = 670, Top = 3, Width = 300, Checked = false };
            pnlChiTiet.Controls.Add(chkKhachDat);

            Nhan(pnlChiTiet, "Giá phòng", 10, 36);
            txtGia = Txt(pnlChiTiet, 90, 36, 110, true);
            Nhan(pnlChiTiet, "Thành tiền", 220, 36);
            txtThanhTien = Txt(pnlChiTiet, 305, 36, 120, true);
            Nhan(pnlChiTiet, "Trạng thái", 445, 36);
            txtTrangThaiCT = Txt(pnlChiTiet, 525, 36, 110, true);

            pnlChiTiet.Controls.Add(new Label
            {
                Text = "Danh sách khách ở (nhập họ tên và CCCD 12 số, số dòng = số khách)",
                Left = 10, Top = 76, AutoSize = true, Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            });
            dgv = new DataGridView
            {
                Left = 10, Top = 102, Width = 700, Height = 150, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                RowHeadersVisible = false, BackgroundColor = Color.White, SelectionMode = DataGridViewSelectionMode.CellSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            };
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Mã khách ở", Width = 120, ReadOnly = true, SortMode = DataGridViewColumnSortMode.NotSortable });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Họ và tên", Width = 300, SortMode = DataGridViewColumnSortMode.NotSortable });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "CCCD", Width = 200, SortMode = DataGridViewColumnSortMode.NotSortable });
            pnlChiTiet.Controls.Add(dgv);

            btnThemPhong = Nut(pnlChiTiet, "Thêm phòng vào phiếu", 730, 102, 190, 32);
            btnSuaPhong = Nut(pnlChiTiet, "Lưu sửa phòng / khách", 730, 140, 190, 32);
            btnXoaPhong = Nut(pnlChiTiet, "Xóa phòng khỏi phiếu", 730, 178, 190, 32);
            btnPhongMoi = Nut(pnlChiTiet, "Nhập phòng mới", 730, 216, 190, 32);

            // ---- sự kiện ----
            txtTim.TextChanged += (s, e) => HienThi(bus.TimNhanh(txtTim.Text));
            btnNangCao.Click += (s, e) => { ucTim.Visible = !ucTim.Visible; SapXep(); };
            lvPhieu.SelectedIndexChanged += (s, e) => ChonPhieu();
            lvChiTiet.SelectedIndexChanged += (s, e) => ChonChiTiet();

            btnChonKH.Click += (s, e) => ChonKhachHang();
            btnChonNV.Click += (s, e) => ChonNhanVien();
            btnThemPhieu.Click += (s, e) => ThemPhieu();
            btnSuaPhieu.Click += (s, e) => SuaPhieu();
            btnXoaPhieu.Click += (s, e) => XoaPhieu();
            btnLuuPhieu.Click += (s, e) => LuuPhieu();
            btnHuyPhieu.Click += (s, e) => HuyPhieu();

            cboTheoGio.SelectedIndexChanged += (s, e) => { if (dangNap || !dangNhapPhieu) return; ApDungKieuO(true); CapNhatCheckout(); };
            dtpCheckin.ValueChanged += (s, e) => { if (!dangNap) CapNhatCheckout(); };
            nudNgay.ValueChanged += (s, e) => { if (!dangNap) CapNhatCheckout(); };
            nudGio.ValueChanged += (s, e) => { if (!dangNap) CapNhatCheckout(); };

            btnChonPhong.Click += (s, e) => ChonPhong();
            nudSoKhach.ValueChanged += (s, e) => { if (!dangNap) { CapNhatXemTruoc(); DungLaiLuoi(); } };
            chkKhachDat.CheckedChanged += (s, e) => DoiKhachDat();
            btnThemPhong.Click += (s, e) => ThemPhong();
            btnSuaPhong.Click += (s, e) => SuaPhong();
            btnXoaPhong.Click += (s, e) => XoaPhong();
            btnPhongMoi.Click += (s, e) => NhapPhongMoi();
        }

        private void frmPhieuDatPhong_Load(object sender, EventArgs e)
        {
            LvHelper.TaoCot(lvPhieu, cotPhieu);
            LvHelper.TaoCot(lvChiTiet, cotCT);

            ucTim.DatTruong(cotPhieu);
            ucTim.Dong += () => { ucTim.Visible = false; SapXep(); };
            ucTim.SizeChanged += (s2, e2) => { if (ucTim.Visible) SapXep(); };
            ucTim.TimKiem += dk => HienThi(bus.TimNangCao(dk));     // BUS, trên ArrayList
            ucTim.XoaLoc += () => HienThi(bus.DanhSach);

            bus.TaiDuLieu();
            ctBus.TaiDuLieu();
            khBus.TaiDuLieu();
            nvBus.TaiDuLieu();
            HienThi(bus.DanhSach);

            LamTrongPhieu();
            DatCheDoPhieu(false);
            NhapPhongMoi();
            CapNhatNut();
        }

        private void SapXep()
        {
            int h = ucTim.Visible ? ucTim.Height + 6 : 0;
            pnlThan.Top = 48 + h;
            host.AutoScrollMinSize = new Size(1040, pnlThan.Top + pnlThan.Height + 10);
        }

        private void HienThi(ArrayList ds)
        {
            LvHelper.Do(lvPhieu, ds, cotPhieu);
        }

        private static DateTime BoGiay(DateTime d)
        {
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0);
        }

        private static string Cell(DataGridViewRow r, int i)
        {
            object v = r.Cells[i].Value;
            return v == null ? "" : Convert.ToString(v);
        }

        // =====================================================================
        //  PHIẾU ĐẶT PHÒNG
        // =====================================================================
        private void ChonPhieu()
        {
            if (dangNap || lvPhieu.SelectedItems.Count == 0) return;
            phieuChon = (PhieuDatPhong)lvPhieu.SelectedItems[0].Tag;
            khachDat = khBus.TimTheoMa(phieuChon.MaKH);
            NapFormPhieu(phieuChon);
            LvHelper.Do(lvChiTiet, ctBus.TheoPhieu(phieuChon.MaPhieu), cotCT);   // click phiếu -> n dòng chi tiết, không cần nút xác nhận
            NhapPhongMoi();
            CapNhatNut();
        }

        private void NapFormPhieu(PhieuDatPhong p)
        {
            dangNap = true;
            try
            {
                txtMaPhieu.Text = p.MaPhieu;
                khChon = khBus.TimTheoMa(p.MaKH);
                nvChon = nvBus.TimTheoMa(p.MaNV);
                txtKhach.Text = p.TenKH;
                txtNhanVien.Text = p.TenNV;
                dtpNgayTao.Value = p.NgayTao;
                dtpCheckin.Value = p.NgayCheckin;
                cboTheoGio.SelectedIndex = p.OTheoGio == 1 ? 0 : 1;
                nudNgay.Value = Math.Min(nudNgay.Maximum, Math.Max(nudNgay.Minimum, p.SoNgayODuKien));
                nudGio.Value = Math.Min(nudGio.Maximum, Math.Max(nudGio.Minimum, p.SoGioNghi));
                cboCoc.SelectedItem = p.TrangThaiCoc;
                txtTienCoc.Text = p.TienCoc.ToString("0");
                txtTong.Text = p.TongTienDuKien.ToString("N0");
                txtCheckout.Text = p.NgayCheckout.HasValue ? p.NgayCheckout.Value.ToString("dd/MM/yyyy HH:mm") : "";
                lblTinhTrang.Text = "Tình trạng: " + p.TinhTrang;
            }
            finally
            {
                dangNap = false;
            }
            ApDungKieuO(false);
        }

        private void LamTrongPhieu()
        {
            dangNap = true;
            try
            {
                txtMaPhieu.Text = bus.SinhMa();
                khChon = null;
                nvChon = null;
                txtKhach.Clear();
                txtNhanVien.Clear();
                dtpNgayTao.Value = DateTime.Now;
                dtpCheckin.Value = BoGiay(DateTime.Today + PhieuDatPhongBUS.GioNhanPhong());
                cboTheoGio.SelectedIndex = 1;       // mặc định: không ở theo giờ
                nudNgay.Value = 1;
                nudGio.Value = 0;
                cboCoc.SelectedIndex = 0;
                txtTienCoc.Text = "0";
                txtTong.Text = "0";
                lblTinhTrang.Text = "";
            }
            finally
            {
                dangNap = false;
            }
            ApDungKieuO(false);
            CapNhatCheckout();
        }

        // Ở theo giờ: chỉ nhập số giờ nghỉ. Ở theo ngày: chỉ nhập số ngày dự kiến.
        private void ApDungKieuO(bool chinhGiaTri)
        {
            bool theoGio = cboTheoGio.SelectedIndex == 0;
            nudNgay.Enabled = dangNhapPhieu && !theoGio;
            nudGio.Enabled = dangNhapPhieu && theoGio;
            if (!chinhGiaTri) return;
            if (theoGio)
            {
                nudNgay.Value = 0;
                if (nudGio.Value < 1) nudGio.Value = 1;
            }
            else
            {
                nudGio.Value = 0;
                if (nudNgay.Value < 1) nudNgay.Value = 1;
            }
        }

        private void CapNhatCheckout()
        {
            if (cboTheoGio.SelectedIndex < 0) return;
            bool theoGio = cboTheoGio.SelectedIndex == 0;
            DateTime co = PhieuDatPhongBUS.TinhCheckout(BoGiay(dtpCheckin.Value), theoGio, (int)nudNgay.Value, (int)nudGio.Value);
            txtCheckout.Text = co.ToString("dd/MM/yyyy HH:mm");
        }

        private void DatCheDoPhieu(bool nhap)
        {
            dangNhapPhieu = nhap;
            btnChonKH.Enabled = nhap;
            btnChonNV.Enabled = nhap;
            dtpCheckin.Enabled = nhap;
            cboTheoGio.Enabled = nhap;
            cboCoc.Enabled = nhap;
            txtTienCoc.Enabled = nhap;
            ApDungKieuO(false);
            lvPhieu.Enabled = !nhap;
            txtTim.Enabled = !nhap;
            btnNangCao.Enabled = !nhap;
            CapNhatNut();
        }

        private void CapNhatNut()
        {
            bool coPhieu = phieuChon != null;
            bool chuaTT = coPhieu && phieuChon.TinhTrang != PhieuDatPhong.DaThanhToan;

            btnThemPhieu.Enabled = !dangNhapPhieu;
            btnSuaPhieu.Enabled = !dangNhapPhieu && chuaTT;
            btnXoaPhieu.Enabled = !dangNhapPhieu && chuaTT;
            btnLuuPhieu.Enabled = dangNhapPhieu;
            btnHuyPhieu.Enabled = dangNhapPhieu;

            pnlChiTiet.Enabled = !dangNhapPhieu && coPhieu;
            lvChiTiet.Enabled = !dangNhapPhieu;

            bool dangO = ctChon != null && ctChon.TrangThai == ChiTietDatPhong.DangO;
            bool choSua = chuaTT && (ctChon == null || dangO);       // phòng đã checkout chỉ được xem
            btnChonPhong.Enabled = chuaTT && ctChon == null;
            btnThemPhong.Enabled = chuaTT && ctChon == null;
            btnSuaPhong.Enabled = chuaTT && dangO;
            btnXoaPhong.Enabled = chuaTT && dangO;
            nudSoKhach.Enabled = choSua;
            chkKhachDat.Enabled = choSua;
            dgv.ReadOnly = !choSua;
        }

        private void ChonKhachHang()
        {
            khBus.TaiDuLieu();
            KhachHang k = frmChon.Chon<KhachHang>("Chọn khách hàng", khBus.DanhSach, cotKhach);
            if (k == null) return;
            khChon = k;
            txtKhach.Text = k.HoTen;
        }

        private void ChonNhanVien()
        {
            nvBus.TaiDuLieu();
            ArrayList ds = bus.NhanVienLeTan();      // chỉ nhân viên chức vụ "Lễ tân"
            if (ds.Count == 0)
            {
                MessageBox.Show("Chưa có nhân viên nào có chức vụ \"Lễ tân\". Hãy thêm ở form Nhân viên.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            NhanVien n = frmChon.Chon<NhanVien>("Chọn nhân viên lập phiếu (lễ tân)", ds, cotNV);
            if (n == null) return;
            nvChon = n;
            txtNhanVien.Text = n.HoTen;
        }

        private void ThemPhieu()
        {
            dangThemPhieu = true;
            LamTrongPhieu();
            DatCheDoPhieu(true);
            ApDungKieuO(false);
        }

        private void SuaPhieu()
        {
            if (phieuChon == null)
            {
                MessageBox.Show("Vui lòng chọn phiếu cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            dangThemPhieu = false;
            DatCheDoPhieu(true);
        }

        private void LuuPhieu()
        {
            decimal coc;
            if (!LoaiPhongBUS.DocTien(txtTienCoc.Text, out coc)) coc = -1;     // BUS sẽ báo lỗi

            var p = new PhieuDatPhong
            {
                MaPhieu = txtMaPhieu.Text,
                MaKH = khChon == null ? "" : khChon.MaKH,
                MaNV = nvChon == null ? "" : nvChon.MaNV,
                NgayCheckin = BoGiay(dtpCheckin.Value),
                OTheoGio = cboTheoGio.SelectedIndex == 0 ? 1 : 2,
                SoNgayODuKien = (int)nudNgay.Value,
                SoGioNghi = (int)nudGio.Value,
                TrangThaiCoc = cboCoc.Text,
                TienCoc = coc
            };

            string loi;
            bool ok = dangThemPhieu ? bus.Them(p, out loi) : bus.Sua(p, out loi);
            if (!ok)
            {
                MessageBox.Show(loi, "Không thể lưu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ctBus.TaiDuLieu();                  // sửa phiếu có thể làm đổi giá/thành tiền các phòng
            DatCheDoPhieu(false);
            NapLaiDanhSach(p.MaPhieu);
            if (dangThemPhieu)
                MessageBox.Show("Đã lưu phiếu. Hãy chọn phòng ở phần \"Chi tiết đặt phòng\" bên dưới để thêm vào phiếu.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Lưu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void HuyPhieu()
        {
            DatCheDoPhieu(false);
            if (phieuChon != null) NapFormPhieu(phieuChon);
            else LamTrongPhieu();
        }

        private void XoaPhieu()
        {
            if (phieuChon == null)
            {
                MessageBox.Show("Vui lòng chọn phiếu cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Xóa phiếu " + phieuChon.MaPhieu + "? Các phòng trong phiếu sẽ được trả về trạng thái trống.",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            string loi;
            if (!bus.Xoa(phieuChon.MaPhieu, out loi))
            {
                MessageBox.Show(loi, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ctBus.TaiDuLieu();
            phieuChon = null;
            khachDat = null;
            txtTim.Text = "";
            HienThi(bus.DanhSach);
            lvChiTiet.Items.Clear();
            LamTrongPhieu();
            NhapPhongMoi();
            CapNhatNut();
        }

        // Nạp lại ListView chính rồi chọn lại phiếu theo mã (kéo theo nạp lại chi tiết)
        private void NapLaiDanhSach(string maPhieu)
        {
            txtTim.Text = "";
            HienThi(bus.DanhSach);
            foreach (ListViewItem it in lvPhieu.Items)
            {
                if (((PhieuDatPhong)it.Tag).MaPhieu == maPhieu)
                {
                    it.Selected = true;
                    it.EnsureVisible();
                    return;
                }
            }
        }

        // =====================================================================
        //  CHI TIẾT ĐẶT PHÒNG + KHÁCH Ở
        // =====================================================================
        private void ChonChiTiet()
        {
            if (dangNap) return;
            if (lvChiTiet.SelectedItems.Count == 0)
            {
                ctChon = null;
                CapNhatNut();
                return;
            }
            ctChon = (ChiTietDatPhong)lvChiTiet.SelectedItems[0].Tag;
            phongChon = null;

            dangNap = true;
            try
            {
                txtPhong.Text = ctChon.TenPhong;
                txtToiDa.Text = ctChon.SoNguoiToiDa.ToString();
                txtGia.Text = ctChon.GiaPhong.ToString("N0");
                txtThanhTien.Text = ctChon.ThanhTien.ToString("N0");
                txtTrangThaiCT.Text = ctChon.TrangThai;
                nudSoKhach.Maximum = Math.Max(1, ctChon.SoNguoiToiDa);
                nudSoKhach.Value = Math.Min(nudSoKhach.Maximum, Math.Max(1, ctChon.SoKhach));

                // khách ở của phòng; khách đặt phiếu (nếu có ở phòng này) xếp lên dòng đầu
                var ds = new List<ChiTietKhachO>();
                bool coKhachDat = false;
                foreach (ChiTietKhachO k in ctBus.Khach.Theo(ctChon.MaPhieu, ctChon.MaPhong))
                {
                    if (khachDat != null && !coKhachDat && k.CCCD == khachDat.CCCD)
                    {
                        ds.Insert(0, k);
                        coKhachDat = true;
                    }
                    else ds.Add(k);
                }
                dgv.Rows.Clear();
                foreach (ChiTietKhachO k in ds) dgv.Rows.Add(k.MaKhachO, k.HoTen, k.CCCD);
                while (dgv.Rows.Count < (int)nudSoKhach.Value) dgv.Rows.Add(TuSinh, "", "");
                chkKhachDat.Checked = coKhachDat;
                row0LaKhachDat = coKhachDat;
                KhoaDongDau(coKhachDat);
            }
            finally
            {
                dangNap = false;
            }
            CapNhatNut();
        }

        // Bắt đầu nhập 1 phòng mới (bỏ chọn dòng chi tiết, xóa ô nhập)
        private void NhapPhongMoi()
        {
            dangNap = true;
            try
            {
                lvChiTiet.SelectedItems.Clear();
                ctChon = null;
                phongChon = null;
                txtPhong.Clear();
                txtToiDa.Clear();
                txtGia.Clear();
                txtThanhTien.Clear();
                txtTrangThaiCT.Text = ChiTietDatPhong.DangO;
                nudSoKhach.Maximum = 8;
                nudSoKhach.Value = 1;
                chkKhachDat.Checked = false;
                row0LaKhachDat = false;
                dgv.Rows.Clear();
            }
            finally
            {
                dangNap = false;
            }
            CapNhatNut();
        }

        // Nút "...": bảng chọn phòng trống -> quay lại, điền thông tin phòng (biến static SelectionContext trong frmChon)
        private void ChonPhong()
        {
            if (phieuChon == null) return;
            ArrayList ds = ctBus.PhongCoTheDat(phieuChon);
            if (ds.Count == 0)
            {
                MessageBox.Show("Hiện không còn phòng trống để thêm vào phiếu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Phong ph = frmChon.Chon<Phong>("Chọn phòng trống", ds, cotPhong);
            if (ph == null) return;

            phongChon = ph;
            dangNap = true;
            try
            {
                txtPhong.Text = ph.TenPhong + " (" + ph.TenLoai + ")";
                txtToiDa.Text = ph.SoNguoiToiDa.ToString();
                nudSoKhach.Maximum = ph.SoNguoiToiDa;
                nudSoKhach.Value = 1;
            }
            finally
            {
                dangNap = false;
            }
            CapNhatXemTruoc();
            DungLaiLuoi();
        }

        // Hiện giá phòng và thành tiền dự kiến ngay khi chọn phòng
        private void CapNhatXemTruoc()
        {
            if (phieuChon == null || phongChon == null || ctChon != null) return;
            ChiTietDatPhong xem = ctBus.TaoMoi(phieuChon, phongChon, (int)nudSoKhach.Value);
            if (xem == null) return;
            txtGia.Text = xem.GiaPhong.ToString("N0");
            txtThanhTien.Text = xem.ThanhTien.ToString("N0");
        }

        // Số dòng của lưới = số khách. Giữ lại những gì đã gõ ở các dòng còn lại.
        private void DungLaiLuoi()
        {
            if (dangNap) return;
            dgv.EndEdit();
            int n = (int)nudSoKhach.Value;
            var cu = new List<string[]>();
            foreach (DataGridViewRow r in dgv.Rows) cu.Add(new[] { Cell(r, 0), Cell(r, 1), Cell(r, 2) });

            dgv.Rows.Clear();
            for (int i = 0; i < n; i++)
            {
                string[] v = i < cu.Count ? cu[i] : new[] { TuSinh, "", "" };
                if (v[0] == "") v[0] = TuSinh;
                dgv.Rows.Add(v[0], v[1], v[2]);
            }
            ApDungKhachDat();
        }

        private void DoiKhachDat()
        {
            if (dangNap) return;
            if (chkKhachDat.Checked && khachDat == null)
            {
                chkKhachDat.Checked = false;
                MessageBox.Show("Vui lòng chọn phiếu có thông tin khách đặt.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dgv.Rows.Count == 0) return;
            ApDungKhachDat();
        }

        // Tick: người thứ nhất là khách đặt phiếu (tự điền, không sửa). Bỏ tick: nhập đủ n dòng.
        private void ApDungKhachDat()
        {
            if (dgv.Rows.Count == 0) return;
            DataGridViewRow r0 = dgv.Rows[0];
            if (chkKhachDat.Checked && khachDat != null)
            {
                r0.Cells[1].Value = khachDat.HoTen;
                r0.Cells[2].Value = khachDat.CCCD;
                row0LaKhachDat = true;
                KhoaDongDau(true);
            }
            else
            {
                if (row0LaKhachDat)
                {
                    r0.Cells[1].Value = "";
                    r0.Cells[2].Value = "";
                }
                row0LaKhachDat = false;
                KhoaDongDau(false);
            }
        }

        private void KhoaDongDau(bool khoa)
        {
            if (dgv.Rows.Count == 0) return;
            dgv.Rows[0].Cells[1].ReadOnly = khoa;
            dgv.Rows[0].Cells[2].ReadOnly = khoa;
        }

        private List<ChiTietKhachO> LayKhach()
        {
            dgv.EndEdit();
            var ds = new List<ChiTietKhachO>();
            foreach (DataGridViewRow r in dgv.Rows)
            {
                string ma = Cell(r, 0);
                ds.Add(new ChiTietKhachO
                {
                    MaKhachO = ma == TuSinh ? "" : ma,
                    HoTen = Cell(r, 1),
                    CCCD = Cell(r, 2)
                });
            }
            return ds;
        }

        // Sau khi thêm/sửa/xóa chi tiết: nạp lại phiếu (tổng tiền dự kiến đổi) và chọn lại phiếu
        private void SauKhiDoiChiTiet()
        {
            string ma = phieuChon.MaPhieu;
            bus.TaiDuLieu();
            NapLaiDanhSach(ma);
        }

        private void ThemPhong()
        {
            if (phieuChon == null)
            {
                MessageBox.Show("Vui lòng chọn phiếu đặt phòng ở bảng phía trên.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (phongChon == null)
            {
                MessageBox.Show("Vui lòng chọn phòng (nút ...).", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string loi;
            if (!ctBus.Them(phieuChon, phongChon, (int)nudSoKhach.Value, LayKhach(), out loi))
            {
                MessageBox.Show(loi, "Không thể thêm phòng", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SauKhiDoiChiTiet();
            MessageBox.Show("Đã thêm phòng vào phiếu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SuaPhong()
        {
            if (ctChon == null)
            {
                MessageBox.Show("Vui lòng chọn dòng chi tiết cần sửa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string loi;
            if (!ctBus.Sua(phieuChon, ctChon, (int)nudSoKhach.Value, LayKhach(), out loi))
            {
                MessageBox.Show(loi, "Không thể sửa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SauKhiDoiChiTiet();
            MessageBox.Show("Lưu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void XoaPhong()
        {
            if (ctChon == null)
            {
                MessageBox.Show("Vui lòng chọn dòng chi tiết cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Xóa " + ctChon.TenPhong + " khỏi phiếu? Phòng sẽ trở về trạng thái trống.",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            string loi;
            if (!ctBus.Xoa(phieuChon, ctChon, out loi))
            {
                MessageBox.Show(loi, "Không thể xóa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SauKhiDoiChiTiet();
        }
    }
}
