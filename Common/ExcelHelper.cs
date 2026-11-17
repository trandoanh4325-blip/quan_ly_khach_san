using System;using System.Data;using System.IO;using System.IO.Compression;using System.Linq;using System.Text;using System.Xml.Linq;using System.Collections.Generic;using System.Globalization;
namespace QLKS.Common {
/// <summary>Shared Excel XLSX helper, no third-party NuGet. First worksheet only on import.</summary>
public static class ExcelHelper {
 static readonly XNamespace ns="http://schemas.openxmlformats.org/spreadsheetml/2006/main";
 static void Put(ZipArchive a,string path,string content){var e=a.CreateEntry(path,CompressionLevel.Optimal);using(var w=new StreamWriter(e.Open(),new UTF8Encoding(false)))w.Write(content);}
 static string Escape(string s)=>System.Security.SecurityElement.Escape(s??"")??"";
 static string Col(int i){string s="";for(i++;i>0;i=(i-1)/26)s=(char)('A'+(i-1)%26)+s;return s;}
 public static void Xuat(string path,DataTable data,string sheetName="DuLieu") {
  if(data==null)throw new ArgumentException("Không có dữ liệu để xuất.");if(data.Columns.Count>16384||data.Rows.Count>1048575)throw new ArgumentException("Dữ liệu vượt giới hạn Excel.");
  using(var fs=File.Create(path))using(var a=new ZipArchive(fs,ZipArchiveMode.Create)) {
   Put(a,"[Content_Types].xml",@"<?xml version=""1.0"" encoding=""UTF-8""?><Types xmlns=""http://schemas.openxmlformats.org/package/2006/content-types""><Default Extension=""rels"" ContentType=""application/vnd.openxmlformats-package.relationships+xml""/><Default Extension=""xml"" ContentType=""application/xml""/><Override PartName=""/xl/workbook.xml"" ContentType=""application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml""/><Override PartName=""/xl/worksheets/sheet1.xml"" ContentType=""application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml""/></Types>");
   Put(a,"_rels/.rels",@"<?xml version=""1.0"" encoding=""UTF-8""?><Relationships xmlns=""http://schemas.openxmlformats.org/package/2006/relationships""><Relationship Id=""rId1"" Type=""http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument"" Target=""xl/workbook.xml""/></Relationships>");
   Put(a,"xl/_rels/workbook.xml.rels",@"<?xml version=""1.0"" encoding=""UTF-8""?><Relationships xmlns=""http://schemas.openxmlformats.org/package/2006/relationships""><Relationship Id=""rId1"" Type=""http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet"" Target=""worksheets/sheet1.xml""/></Relationships>");
   Put(a,"xl/workbook.xml","<?xml version=\"1.0\" encoding=\"UTF-8\"?><workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\"><sheets><sheet name=\""+Escape((sheetName??"DuLieu").Substring(0,Math.Min(31,(sheetName??"DuLieu").Length)))+"\" sheetId=\"1\" r:id=\"rId1\"/></sheets></workbook>");
   var e=a.CreateEntry("xl/worksheets/sheet1.xml",CompressionLevel.Optimal);using(var sw=new StreamWriter(e.Open(),new UTF8Encoding(false))){
    sw.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?><worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\"><sheetData>");
    sw.Write("<row r=\"1\">");for(int c=0;c<data.Columns.Count;c++)sw.Write("<c r=\""+Col(c)+"1\" t=\"inlineStr\"><is><t xml:space=\"preserve\">"+Escape(data.Columns[c].ColumnName)+"</t></is></c>");sw.Write("</row>");
    for(int r=0;r<data.Rows.Count;r++){sw.Write("<row r=\""+(r+2)+"\">");for(int c=0;c<data.Columns.Count;c++){var v=data.Rows[r][c];if(v==DBNull.Value)continue;string id=Col(c)+(r+2);if(v is byte||v is short||v is int||v is long||v is float||v is double||v is decimal){sw.Write("<c r=\""+id+"\"><v>"+Convert.ToString(v,CultureInfo.InvariantCulture)+"</v></c>");}else{string t=v is DateTime d?d.ToString("yyyy-MM-dd HH:mm:ss"):Convert.ToString(v);sw.Write("<c r=\""+id+"\" t=\"inlineStr\"><is><t xml:space=\"preserve\">"+Escape(t)+"</t></is></c>");}}sw.Write("</row>");}
    sw.Write("</sheetData></worksheet>");}
  }
 }
 static int Index(string refText){int v=0;foreach(char c in refText){if(c<'A'||c>'Z')break;v=v*26+c-'A'+1;}return v-1;}
 public static DataTable Nhap(string path){
  using(var fs=File.OpenRead(path))using(var a=new ZipArchive(fs,ZipArchiveMode.Read)){
   var sheet=a.GetEntry("xl/worksheets/sheet1.xml")??throw new InvalidDataException("Không tìm thấy sheet1.xml. Hãy dùng Excel workbook đơn giản.");
   var strings=new List<string>();var shared=a.GetEntry("xl/sharedStrings.xml");if(shared!=null)using(var st=shared.Open()){var sdoc=XDocument.Load(st);foreach(var si in sdoc.Descendants(ns+"si"))strings.Add(string.Concat(si.Descendants(ns+"t").Select(t=>t.Value)));}
   var dt=new DataTable();using(var stream=sheet.Open()){
    var doc=XDocument.Load(stream);var rows=doc.Descendants(ns+"sheetData").Elements(ns+"row").ToList();bool first=true;
    foreach(var row in rows){var cells=row.Elements(ns+"c").ToList();int max=cells.Count==0?0:cells.Max(c=>Index((string)c.Attribute("r")??"A"))+1;
     if(first){for(int c=0;c<max;c++)dt.Columns.Add("Column"+(c+1));first=false;}
     else{while(dt.Columns.Count<max)dt.Columns.Add("Column"+(dt.Columns.Count+1));}
     var vals=new string[dt.Columns.Count];foreach(var c in cells){int index=Index((string)c.Attribute("r")??"A");if(index<0||index>=vals.Length)continue;string type=(string)c.Attribute("t");string val=type=="inlineStr"?string.Concat(c.Descendants(ns+"t").Select(t=>t.Value)):(string)c.Element(ns+"v")??"";if(type=="s"&&int.TryParse(val,out int n)&&n>=0&&n<strings.Count)val=strings[n];vals[index]=val;}
     if(row==rows.First()){for(int i=0;i<dt.Columns.Count;i++){string name=string.IsNullOrWhiteSpace(vals[i])?"Column"+(i+1):vals[i];if(dt.Columns.Cast<DataColumn>().Any(c=>c.Ordinal<i&&c.ColumnName==name))name+="_"+i;dt.Columns[i].ColumnName=name;}}else dt.Rows.Add(vals.Select(v=>(object)(v??"")).ToArray());
    }
   }
   return dt;
  }
 }
}
}
