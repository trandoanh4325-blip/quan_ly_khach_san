using System;using System.Data;using System.Drawing;using System.Windows.Forms;using QLKS.Common;using QLKS.DAO;using QLKS.BUS;using QLKS.DTO;
namespace QLKS.GUI {
public class frmExcelTV4:Form {
 readonly ComboBox table=new ComboBox();readonly DataGridView preview=new DataGridView();DataTable data;
 readonly string[] tables={"loai_san_pham","san_pham","nha_cung_cap","phieu_nhap","chi_tiet_phieu_nhap","hoa_don","chi_tiet_phu_thu","nhan_vien","khach_hang","tham_so","loai_phong","phong","dich_vu","phu_thu","khuyen_mai","phieu_dat_phong","chi_tiet_dat_phong","chi_tiet_khach_o","chi_tiet_su_dung_dich_vu"};
 public frmExcelTV4(){Text="IMPORT / EXPORT EXCEL";Font=new Font("Segoe UI",10);var title=new Label{Text="EXCEL - QUẢN LÝ DỮ LIỆU",Dock=DockStyle.Top,Height=45,Font=new Font("Segoe UI",16,FontStyle.Bold),TextAlign=ContentAlignment.MiddleCenter};
 var panel=new FlowLayoutPanel{Dock=DockStyle.Top,Height=55,Padding=new Padding(9)};table.Width=200;table.DropDownStyle=ComboBoxStyle.DropDownList;table.Items.AddRange(tables);table.SelectedIndexChanged+=(s,e)=>Tai();panel.Controls.Add(table);
 var xuat=new Button{Text="Xuất Excel (.xlsx)",Width=155,Height=32};xuat.Click+=(s,e)=>Xuat();panel.Controls.Add(xuat);
 var nhap=new Button{Text="Nhập Excel (.xlsx)",Width=155,Height=32};nhap.Click+=(s,e)=>Nhap();panel.Controls.Add(nhap);
 var hint=new Label{Text="Nhập chỉ hỗ trợ 3 danh mục (Loại SP, Sản phẩm, NCC); tồn kho không nhập tay.",AutoSize=true};panel.Controls.Add(hint);
 preview.Dock=DockStyle.Fill;preview.ReadOnly=true;preview.AllowUserToAddRows=false;preview.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.DisplayedCells;Controls.Add(preview);Controls.Add(panel);Controls.Add(title);table.SelectedIndex=0;
 }
 void Tai(){try{string t=table.Text;data=MyConnection.ExecuteQuery("SELECT * FROM "+t+" LIMIT 10000");preview.DataSource=data;}catch(Exception e){MessageBox.Show(e.Message);}}
 void Xuat(){try{using(var dlg=new SaveFileDialog{Filter="Excel (*.xlsx)|*.xlsx",FileName=table.Text+".xlsx"})if(dlg.ShowDialog()==DialogResult.OK){DataTable d=MyConnection.ExecuteQuery("SELECT * FROM "+table.Text);ExcelHelper.Xuat(dlg.FileName,d,table.Text);MessageBox.Show("Đã xuất Excel: "+d.Rows.Count+" dòng.");}}catch(Exception e){MessageBox.Show(e.Message);}}
 void Nhap(){if(table.Text!="loai_san_pham"&&table.Text!="san_pham"&&table.Text!="nha_cung_cap"){MessageBox.Show("Chỉ cho nhập 3 danh mục. Phiếu nhập hàng phải tạo bằng form để tăng tồn chính xác.");return;}
  using(var dlg=new OpenFileDialog{Filter="Excel (*.xlsx)|*.xlsx"}){if(dlg.ShowDialog()!=DialogResult.OK)return;try{var d=ExcelHelper.Nhap(dlg.FileName);if(d.Rows.Count==0){MessageBox.Show("File Excel không có dòng dữ liệu.");return;}
   foreach(string col in table.Text=="san_pham"?new[]{"ma_sp","ten_sp","ma_loai_sp","don_vi_tinh","gia_ban"}:table.Text=="loai_san_pham"?new[]{"ma_loai_sp","ten_loai_sp"}:new[]{"ma_ncc","ten_ncc","so_dien_thoai","dia_chi"})if(!d.Columns.Contains(col))throw new ArgumentException("Thiếu cột bắt buộc: "+col);
   if(MessageBox.Show("Nhập "+d.Rows.Count+" dòng? Dữ liệu trùng mã sẽ báo lỗi và bỏ qua.","Xác nhận",MessageBoxButtons.YesNo)!=DialogResult.Yes)return;
   int ok=0;var errs=new System.Collections.Generic.List<string>();var lb=new LoaiSanPhamBUS();var nb=new NhaCungCapBUS();var sb=new SanPhamBUS();
   foreach(DataRow r in d.Rows){string loi="";bool success=false;try{string v(string key)=>Convert.ToString(r[key])??"";
    if(table.Text=="loai_san_pham")success=lb.Luu(new LoaiSanPham{MaLoaiSP=v("ma_loai_sp"),TenLoaiSP=v("ten_loai_sp")},true,out loi);
    else if(table.Text=="nha_cung_cap")success=nb.Luu(new NhaCungCap{MaNCC=v("ma_ncc"),TenNCC=v("ten_ncc"),SoDienThoai=v("so_dien_thoai"),DiaChi=v("dia_chi")},true,out loi);
    else success=sb.Them(new SanPham{MaSP=v("ma_sp"),TenSP=v("ten_sp"),MaLoaiSP=v("ma_loai_sp"),DonViTinh=v("don_vi_tinh"),GiaBan=decimal.Parse(v("gia_ban"),System.Globalization.CultureInfo.InvariantCulture),SoLuongTon=0},out loi);
    if(success)ok++;else errs.Add("Dòng "+(d.Rows.IndexOf(r)+2)+": "+loi);
   }catch(Exception ex){errs.Add("Dòng "+(d.Rows.IndexOf(r)+2)+": "+ex.Message);}}
   Tai();MessageBox.Show("Nhập thành công "+ok+"/"+d.Rows.Count+" dòng. "+(errs.Count>0?"Lỗi đầu tiên:\n"+string.Join("\n",errs.GetRange(0,Math.Min(5,errs.Count))):""));
  }catch(Exception e){MessageBox.Show(e.Message);}}}
 }
}
