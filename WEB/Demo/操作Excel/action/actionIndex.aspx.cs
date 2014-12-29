using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class action_actionIndex : System.Web.UI.Page
{
    int res = 0;
    string desc = "提交失败！";

    protected void Page_Load(object sender, EventArgs e)
    {
        string action = Request["action"] ?? "";
        switch (action)
        {
            case "ajaxuploadfile":

                if (Request.Files.Count > 0)
                {

                    HttpPostedFile ajaxFile = Request.Files[0];//runat="server"
                    if (ajaxFile.ContentLength < 10485760) //判断文件是否小于10Mb  
                    {
                        BaoxianResult result = ImportDataSetFromExcel(ajaxFile.InputStream);
                    }
                }
                break;
            default:
                break;
        }
        string json = "{\"res\":" + res + ",\"desc\":\"" + desc + "\"}";
        Response.Clear();
        Response.Write(json);
        Response.End();
    }

    private BaoxianResult ImportDataSetFromExcel(Stream stream)
    {

        BaoxianResult result = new BaoxianResult();
        string message = "";
        string exmessage = "";
        try
        {
            List<BaoxianExc> list = new List<BaoxianExc>();
            HSSFWorkbook workbook = new HSSFWorkbook(stream);
            ISheet sheet = workbook.GetSheet("团单上传标准格式");
            int lastRowNum = sheet.LastRowNum;
            if (lastRowNum > 55)
                lastRowNum = 55;
            for (int i = 5; i <= lastRowNum; i++)
            {
                try
                {
                    BaoxianExc baoxianE = new BaoxianExc();
                    IRow rows = sheet.GetRow(i);
                    //short lastCellNum = rows.LastCellNum; 
                    if (!string.IsNullOrWhiteSpace(rows.Cells[1].StringCellValue) && !string.IsNullOrWhiteSpace(rows.Cells[2].StringCellValue) && !string.IsNullOrWhiteSpace(rows.Cells[3].StringCellValue) && rows.Cells[4].DateCellValue != null && !string.IsNullOrWhiteSpace(rows.Cells[5].StringCellValue) && !string.IsNullOrWhiteSpace(rows.Cells[6].StringCellValue))
                    {
                        baoxianE.ID = Convert.ToInt32(rows.Cells[0].NumericCellValue);
                        baoxianE.Name = rows.Cells[1].StringCellValue;
                        baoxianE.CardType = Convert.ToInt32(rows.Cells[2].StringCellValue);
                        baoxianE.Card = rows.Cells[3].StringCellValue;
                        baoxianE.Birthday = rows.Cells[4].DateCellValue;
                        baoxianE.Sex = rows.Cells[5].StringCellValue == "男" ? 1 : 0;
                        baoxianE.Phone = rows.Cells[6].StringCellValue;
                        list.Add(baoxianE);
                    }
                }
                catch (Exception ex)
                {
                    message += (i - 4) + "行,";
                    exmessage += ex.Message + "\n\r";

                }
            }
            if (!string.IsNullOrWhiteSpace(message))
                message += "->数据有问题";
            result.ExMessage = exmessage;
            result.MessageLog = message;
            result.list = list;
        }
        catch (Exception ex)
        {
            message = "加载Excel模板失败！";
            exmessage = ex.Message + "\n\r";
        }

        return result;
    }
}
public class BaoxianResult
{
    public string MessageLog { get; set; }
    public string ExMessage { get; set; }
    public List<BaoxianExc> list { get; set; }
}
public class BaoxianExc
{
    public int ID { get; set; }
    public string Name { get; set; }
    public int CardType { get; set; }
    public string Card { get; set; }
    public DateTime Birthday { get; set; }
    public int Sex { get; set; }//0 女 1 男
    public string Phone { get; set; }
}