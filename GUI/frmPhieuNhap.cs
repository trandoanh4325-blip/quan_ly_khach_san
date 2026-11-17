using System;using System.Data;using System.Drawing;using System.Linq;using System.Collections.Generic;using System.Windows.Forms;using QLKS.BUS;using QLKS.DTO;using QLKS.Common;using System.Collections;
namespace QLKS.GUI {
public class frmPhieuNhap : Form {
 readonly PhieuNhapBUS bus=new PhieuNhapBUS();
 readonly ListView ds=new ListView(),ct=new ListView(),nhap=new ListView();
 readonly ComboBox ncc=new ComboBox(), nv=new ComboBox(),sp=new ComboBox();
 readonly NumericUpDown sl=new NumericUpDown(),gia=new NumericUpDown();
 readonly TextBox ma=new TextBox();readonly Label tong=new Label();readonly DateTimePicker ngay=new DateTimePicker();
 readonly List<ChiTietPhieuNhap> chiTiet=new List<ChiTietPhieuNhap>();
 public frmPhieuNhap(){Text="PHIẾU NHẬP HÀNG";Font=new Font("Segoe UI",10); BackColor=Color.White;AutoScroll=true;
 var title=new Label{Text=Text,Dock=DockStyle.Top,Height=45,TextAlign=ContentAlignment.MiddleCenter,Font=new Font("Segoe UI",16,FontStyle.Bold)};
 var head=new Panel{Dock=DockStyle.Top,Height=115};int x=12;void Field(string label,Control c,int width){head.Controls.Add(new Label{Text=label,Left=x,Top=8,Width=width});c.SetBounds(x,31,width,28);head.Controls.Add(c);x+=width+15;}
 ma.ReadOnly=true;Field("Mã phiếu",ma,110);ncc.DropDownStyle=ComboBoxStyle.DropDownList;Field("Nhà cung cấp",ncc,220);nv.DropDownStyle=ComboBoxStyle.DropDownList;Field("Nhân viên",nv,205);Field("Ngày nhập",ngay,200);
 var btnNew=new Button{Text="Phiếu mới",Left=12,Top=73,Width=110};btnNew.Click+=(s,e)=>Moi();head.Controls.Add(btnNew);
 var btnSave=new Button{Text="LƯU PHIẾU",Left=130,Top=73,Width=120};btnSave.Click+=(s,e)=>Luu();head.Controls.Add(btnSave);
 tong.SetBounds(280,77,450,30);head.Controls.Add(tong);
 var body=new SplitContainer{Dock=DockStyle.Fill,Orientation=Orientation.Horizontal};
 var main=new SplitContainer{Dock=DockStyle.Fill};ds.Dock=DockStyle.Fill;ct.Dock=DockStyle.Fill;main.Panel1.Controls.Add(ds);main.Panel2.Controls.Add(ct);body.Panel1.Controls.Add(main);
 Setup(ds,new[]{"Mã phiếu","Nhà cung cấp","Nhân viên","Ngày","Tổng tiền"});Setup(ct,new[]{"Mã SP","Tên sản phẩm","Số lượng","Đơn giá","Thành tiền"});ds.SelectedIndexChanged+=(s,e)=>XemChiTiet();
 var entry=new Panel{Dock=DockStyle.Top,Height=64};int xx=10;void Entry(string label,Control c,int width){entry.Controls.Add(new Label{Left=xx,Top=4,Text=label,Width=width});c.SetBounds(xx,28,width,28);entry.Controls.Add(c);xx+=width+12;}
 sp.DropDownStyle=ComboBoxStyle.DropDownList;Entry("Sản phẩm",sp,280);sl.Minimum=1;sl.Maximum=1000000;Entry("Số lượng",sl,100);gia.DecimalPlaces=0;gia.Maximum=999999999999m;gia.ThousandsSeparator=true;Entry("Đơn giá nhập",gia,150);
 var add=new Button{Text="Thêm chi tiết",Left=xx,Top=24,Width=130};add.Click+=(s,e)=>ThemDong();entry.Controls.Add(add);
 var pick=new Button{Text="...",Left=xx+140,Top=24,Width=38};pick.Click+=(s,e)=>ChonSanPham();entry.Controls.Add(pick);
 var remove=new Button{Text="Xóa dòng",Left=xx+186,Top=24,Width=100};remove.Click+=(s,e)=>XoaDong();entry.Controls.Add(remove);
 nhap.Dock=DockStyle.Fill;Setup(nhap,new[]{"Mã SP","Số lượng","Đơn giá","Thành tiền"});nhap.SelectedIndexChanged+=(s,e)=>ChonDongNhap();
 body.Panel2.Controls.Add(nhap);body.Panel2.Controls.Add(entry);
 Controls.Add(body);Controls.Add(head);Controls.Add(title);Load+=(s,e)=>{Tai(); if(body.Height>350)body.SplitterDistance=body.Height/2; if(main.Width>600)main.SplitterDistance=main.Width/2;};Resize+=(s,e)=>{if(body.Height>350)body.SplitterDistance=body.Height/2;if(main.Width>600)main.SplitterDistance=main.Width/2;};
 }
 void Setup(ListView lv,string[] cols){lv.View=View.Details;lv.GridLines=true;lv.FullRowSelect=true;lv.MultiSelect=false;foreach(var c in cols)lv.Columns.Add(c,c=="Tên sản phẩm"||c=="Nhà cung cấp"?200:130);}
 void Bind(ComboBox c,DataTable dt,string display,string value){c.DisplayMember=display;c.ValueMember=value;c.DataSource=dt;}
 void Tai(){try{Bind(ncc,bus.NCC(),"ten_ncc","ma_ncc");Bind(nv,bus.NhanVien(),"ho_ten","ma_nv");Bind(sp,bus.SanPham(),"ten_sp","ma_sp");Moi();HienThi();}catch(Exception ex){Bao(ex.Message);}}
 void Moi(){try{ma.Text=bus.SinhMa();ngay.Value=DateTime.Now;chiTiet.Clear();CapNhatDong();}catch(Exception ex){Bao(ex.Message);}}
 void HienThi(){ds.Items.Clear(); foreach(DataRow r in bus.LayDanhSach().Rows){var item=new ListViewItem(new[]{r["ma_phieu_nhap"].ToString(),r["ten_ncc"].ToString(),r["ho_ten"].ToString(),Convert.ToDateTime(r["ngay_nhap"]).ToString("dd/MM/yyyy HH:mm"),Convert.ToDecimal(r["tong_tien_nhap"]).ToString("N0")});item.Tag=r["ma_phieu_nhap"].ToString();ds.Items.Add(item);}}
 void XemChiTiet(){if(ds.SelectedItems.Count==0)return;try{ct.Items.Clear();foreach(DataRow r in bus.ChiTiet(ds.SelectedItems[0].Tag.ToString()).Rows){ct.Items.Add(new ListViewItem(new[]{r["ma_sp"].ToString(),r["ten_sp"].ToString(),r["so_luong_nhap"].ToString(),Convert.ToDecimal(r["don_gia_nhap"]).ToString("N0"),Convert.ToDecimal(r["thanh_tien"]).ToString("N0")}));}}catch(Exception ex){Bao(ex.Message);}}
 void ChonSanPham(){try{var b=new SanPhamBUS();var o=frmChon.Chon<SanPham>("Chọn sản phẩm",b.DanhSach,new CotHienThi("Mã SP","MaSP",110),new CotHienThi("Tên SP","TenSP",240),new CotHienThi("Tồn kho","SoLuongTon",100));if(o!=null)sp.SelectedValue=o.MaSP;}catch(Exception ex){Bao(ex.Message);}}
 void ThemDong(){if(sp.SelectedValue==null){Bao("Chọn sản phẩm.");return;}string id=sp.SelectedValue.ToString();if(chiTiet.Any(c=>c.MaSP==id)){Bao("Sản phẩm này đã có trong phiếu.");return;}chiTiet.Add(new ChiTietPhieuNhap{MaSP=id,SoLuongNhap=(int)sl.Value,DonGiaNhap=gia.Value});CapNhatDong();}
 void ChonDongNhap(){if(nhap.SelectedItems.Count==0)return;var c=nhap.SelectedItems[0].Tag as ChiTietPhieuNhap;if(c==null)return;sp.SelectedValue=c.MaSP;sl.Value=c.SoLuongNhap;gia.Value=c.DonGiaNhap;}
 void XoaDong(){if(nhap.SelectedItems.Count==0)return;chiTiet.Remove((ChiTietPhieuNhap)nhap.SelectedItems[0].Tag);CapNhatDong();}
 void CapNhatDong(){nhap.Items.Clear();foreach(var c in chiTiet){var it=new ListViewItem(new[]{c.MaSP,c.SoLuongNhap.ToString(),c.DonGiaNhap.ToString("N0"),c.ThanhTien.ToString("N0")});it.Tag=c;nhap.Items.Add(it);}tong.Text="Tổng tiền nhập: "+chiTiet.Sum(c=>c.ThanhTien).ToString("N0")+" đ";}
 void Luu(){if(ncc.SelectedValue==null||nv.SelectedValue==null){Bao("Chọn nhà cung cấp và nhân viên.");return;}var p=new PhieuNhap{MaPhieuNhap=ma.Text,MaNCC=ncc.SelectedValue.ToString(),MaNV=nv.SelectedValue.ToString(),NgayNhap=ngay.Value,ChiTiet=chiTiet.ToList()};string loi;if(!bus.Them(p,out loi)){Bao(loi);return;}MessageBox.Show("Đã lưu phiếu và cập nhật tồn kho.");Tai();}
 void Bao(string msg)=>MessageBox.Show(msg,"Phiếu nhập",MessageBoxButtons.OK,MessageBoxIcon.Warning);
}}
