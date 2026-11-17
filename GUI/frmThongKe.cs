using System;using System.Data;using System.Drawing;using System.Windows.Forms;using QLKS.BUS;using QLKS.Common;
namespace QLKS.GUI {
public class frmThongKe:Form {
 readonly ThongKeBUS bus=new ThongKeBUS();readonly ComboBox loai=new ComboBox(),doiTuong=new ComboBox(),kieu=new ComboBox();readonly DateTimePicker tu=new DateTimePicker(),den=new DateTimePicker();readonly NumericUpDown nam=new NumericUpDown();readonly DataGridView bang=new DataGridView();DataTable hienTai;
 public frmThongKe(){Text="THỐNG KÊ KHO VÀ DOANH THU";Font=new Font("Segoe UI",10);BackColor=Color.White;
 var title=new Label{Text=Text,Dock=DockStyle.Top,Height=48,TextAlign=ContentAlignment.MiddleCenter,Font=new Font("Segoe UI",17,FontStyle.Bold)};
 var filter=new Panel{Dock=DockStyle.Top,Height=150};int x=15;
 void Field(string label,Control c,int width,int y){filter.Controls.Add(new Label{Text=label,Left=x,Top=y,Width=width});c.SetBounds(x,y+25,width,30);filter.Controls.Add(c);x+=width+13;}
 loai.DropDownStyle=doiTuong.DropDownStyle=kieu.DropDownStyle=ComboBoxStyle.DropDownList;
 loai.Items.AddRange(new object[]{"10a - Thu/Chi/Lợi nhuận","10a - Bảng chéo quý","10b - Theo thời gian","10b - Hai khóa","10b - Ba khóa"});doiTuong.Items.AddRange(new object[]{"Nhân viên","Khách hàng","Sản phẩm"});kieu.Items.AddRange(new object[]{"Ngày","Tháng","Quý","Năm","Khoảng ngày"});
 Field("Loại báo cáo",loai,225,3);Field("Đối tượng",doiTuong,155,3);Field("Kiểu thời gian",kieu,150,3);Field("Từ ngày",tu,135,3);Field("Đến ngày",den,135,3);
 x=15;nam.Minimum=2000;nam.Maximum=2100;nam.Value=DateTime.Today.Year;Field("Năm bảng quý",nam,140,75);var btn=new Button{Text="XEM THỐNG KÊ",Left=171,Top=100,Width=150,Height=32};btn.Click+=(s,e)=>Tai();filter.Controls.Add(btn);
 var export=new Button{Text="XUẤT EXCEL",Left=334,Top=100,Width=135,Height=32};export.Click+=(s,e)=>Xuat();filter.Controls.Add(export);
 bang.Dock=DockStyle.Fill;bang.ReadOnly=true;bang.AllowUserToAddRows=false;bang.AutoSizeColumnsMode=DataGridViewAutoSizeColumnsMode.Fill;Controls.Add(bang);Controls.Add(filter);Controls.Add(title);
 loai.SelectedIndex=0;doiTuong.SelectedIndex=0;kieu.SelectedIndex=1;tu.Value=new DateTime(DateTime.Today.Year,1,1);den.Value=DateTime.Today;
 }
 void Tai(){try{hienTai=bus.BaoCao(loai.Text,doiTuong.Text,kieu.Text,tu.Value,den.Value,(int)nam.Value);bang.DataSource=hienTai;}catch(Exception e){MessageBox.Show(e.Message,"Thống kê");}}
 void Xuat(){if(hienTai==null){MessageBox.Show("Hãy xem thống kê trước khi xuất.");return;}using(var dlg=new SaveFileDialog{Filter="Excel Workbook (*.xlsx)|*.xlsx",FileName="ThongKe.xlsx"})if(dlg.ShowDialog()==DialogResult.OK)try{ExcelHelper.Xuat(dlg.FileName,hienTai,"ThongKe");MessageBox.Show("Đã xuất Excel.");}catch(Exception e){MessageBox.Show(e.Message);}}
}}
