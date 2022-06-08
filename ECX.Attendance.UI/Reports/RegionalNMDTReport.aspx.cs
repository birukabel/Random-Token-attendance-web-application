using ECX.Attendance.BE;
using ECX.Attendance.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ECX.Attendance.UI.Reports
{
    public partial class RegionalNMDTReport : System.Web.UI.Page
    {
        #region memberVariables
        float[] rightsAll, rightsAA, rightsHW, rightsHU, rightsAD, rightsGN, rightsNK = null;

        static string tradingCenter = "";
        #endregion

        #region memberEvents
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.Session["LoggedUser"] != null)
                {
                    if (ConfigurationManager.AppSettings.AllKeys.Contains(Session["TradingCenter"].ToString()))
                    {
                        tradingCenter = ConfigurationManager.AppSettings[Session["TradingCenter"].ToString()].ToString();
                    }
                    else
                    {
                        Response.Redirect("~/ErrorPage.aspx");
                    }

                    UserInfo user = new UserInfo();
                    user = (UserInfo)Session["LoggedUser"];

                    ECXSecurityAccess.ECXSecurityAccess Sec = new ECXSecurityAccess.ECXSecurityAccess();
                    rightsAll = Sec.HasRights(user.UniqueIdentifier, new string[] { 
                        "ViewAttenanceReports" }, "");

                    rightsAA = Sec.HasRights(user.UniqueIdentifier, new string[] { 
                        "ViewAttenanceAA" }, "");

                    rightsHW = Sec.HasRights(user.UniqueIdentifier, new string[] { 
                        "ViewAttenanceHW" }, "");

                    rightsHU = Sec.HasRights(user.UniqueIdentifier, new string[] { 
                        "ViewAttenanceHU" }, "");

                    rightsAD = Sec.HasRights(user.UniqueIdentifier, new string[] { 
                        "ViewAttenanceAD" }, "");

                    rightsGN = Sec.HasRights(user.UniqueIdentifier, new string[] { 
                        "ViewAttenanceGN" }, "");

                    rightsNK = Sec.HasRights(user.UniqueIdentifier, new string[] { 
                        "ViewAttenanceNK" }, "");

                    if (rightsAll[0] == 1 || rightsAll[0] == 3)
                    {
                        tradingCenter = "All";
                    }
                    if (rightsAA[0] == 1 || rightsAA[0] == 3)
                    {
                        tradingCenter = "Addis Ababa";
                    }
                    if (rightsHW[0] == 1 || rightsHW[0] == 3)
                    {
                        tradingCenter = "Hawassa";
                    }
                    if (rightsHU[0] == 1 || rightsHU[0] == 3)
                    {
                        tradingCenter = "Hummera";
                    }
                    if (rightsAD[0] == 1 || rightsAD[0] == 3)
                    {
                        tradingCenter = "Adama";
                    }
                    if (rightsGN[0] == 1 || rightsGN[0] == 3)
                    {
                        tradingCenter = "Gondar";
                    }
                    if (rightsNK[0] == 1 || rightsNK[0] == 3)
                    {
                        tradingCenter = "Nekemte";
                    }
                    PopulateTradingCenter();
                }
                
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (ddlCenter.SelectedIndex != 0)
            {
                ExportToExcel(new TradeBLL().GetNMDTRtc(ddlCenter.SelectedItem.Text));
            }
            else
            {
                ExportToExcel(new TradeBLL().GetNMDTRtc(""));
            }
        }

        #endregion


        #region memmberMethods

        private void PopulateTradingCenter()
        {
            TradeBLL trade = new TradeBLL();
            List<string> strItems = new List<string>();
            DataTable dt = trade.GetRegionalTradingCenter();

            ddlCenter.DataSource = dt;
            ddlCenter.DataTextField = "Name";
            ddlCenter.DataValueField = "ID";
            ddlCenter.DataBind();
            ddlCenter.Items.Insert(0, "--Select--");
            ddlCenter.Items.FindByText("--Select--").Value = Guid.Empty.ToString();
            ddlCenter.SelectedIndex = 0;
            //ddlCenter.DataBind();
            //ddlCenter.DataBind();
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
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=RegionalNMDTReport.xls");

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

        #endregion
    }
}