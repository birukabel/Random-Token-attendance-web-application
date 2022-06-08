using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECX.Attendance.BLL;
using ECX.Attendance.BE;
using System.Configuration;
using System.Text;
using System.Data;

namespace ECX.Attendance.UI.Reports
{
    public partial class NotWorkingWorkStationsReport : System.Web.UI.Page
    {
        float[] rights = null;
        float[] rights2 = null;

        static string tradingCenter = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInfo user = new UserInfo();
                user = (UserInfo)Session["LoggedUser"];

                ECXSecurityAccess.ECXSecurityAccess Sec = new ECXSecurityAccess.ECXSecurityAccess();
                rights = Sec.HasRights(user.UniqueIdentifier, new string[] { 
                        "AccessAttenanceApp" }, "");

                if (rights[0] == 1 || rights[0] == 3)
                {
                    if (this.Session["LoggedUser"] != null)
                    {
                        if (!ConfigurationManager.AppSettings.AllKeys.Contains(Session["TradingCenter"].ToString()))
                        {
                            tradingCenter = "Addis Ababa";
                        }
                        else
                        {
                            tradingCenter = ConfigurationManager.AppSettings[Session["TradingCenter"].ToString()].ToString();
                        }

                        ECXSecurityAccess.ECXSecurityAccess Sec2 = new ECXSecurityAccess.ECXSecurityAccess();
                        rights2 = Sec2.HasRights(user.UniqueIdentifier, new string[] { 
                        "ViewAttenanceReports" }, "");

                        if (rights2[0] == 1 || rights2[0] == 3)
                        {
                            tradingCenter = "All";
                        }
                    }
                }
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            if (txtStartDate.Text != "" && txtEndDate.Text != "")
            {
                var ds = new NotWorkingWorkStationBLL().GetWorkStationsByDateAsDataSet( Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text));
                DataTable dt = ds.Tables[0];

                if (!tradingCenter.Equals("All") && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0].AsEnumerable().Where(r => ((string)r["TradingCenter"]) == tradingCenter).CopyToDataTable();
                }
                ExportToExcel(dt);
            }
        }

        void ExportToExcel(DataTable dt)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=NotWorkingWorkStations.xls");

                HttpContext.Current.Response.Charset = "utf-8";
                HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding("windows-1250");
                HttpContext.Current.Response.Write("<BR><BR><BR>");

                HttpContext.Current.Response.Write("<Table border='1' bgColor='#ffffff' " +
                  "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
                  "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR bgcolor='seagreen'>");

                int columnscount = dt.Columns.Count;

                for (int j = 0; j < columnscount; j++)
                {
                    HttpContext.Current.Response.Write("<Td>");
                    HttpContext.Current.Response.Write("<B>");
                    HttpContext.Current.Response.Write(dt.Columns[j].ColumnName);
                    HttpContext.Current.Response.Write("</B>");
                    HttpContext.Current.Response.Write("</Td>");
                }
                HttpContext.Current.Response.Write("</TR>");
                foreach (DataRow row in dt.Rows)
                {
                    HttpContext.Current.Response.Write("<TR>");
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        HttpContext.Current.Response.Write("<Td>");
                        HttpContext.Current.Response.Write(row[i].ToString());
                        HttpContext.Current.Response.Write("</Td>");
                    }

                    HttpContext.Current.Response.Write("</TR>");
                }
                HttpContext.Current.Response.Write("</Table>");
                HttpContext.Current.Response.Write("</font>");
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }
    }
}