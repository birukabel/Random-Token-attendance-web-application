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
    public partial class AttendanceReport : System.Web.UI.Page
    {
        float[] rightsAll, rightsAA, rightsHW, rightsHU, rightsAD, rightsGN, rightsNK = null;       

        static string tradingCenter = "";
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

                    Populatesession();
                    PopulateMember();
                    ddlRepId.SelectedIndex = -1;
                }
            }
        }

        private void PopulateMember()
        {
            MembershipBLL mb = new MembershipBLL();
            DataTable dt = new DataTable();
            dt = mb.MemberData();
            ddlMemberId.DataSource = dt;
            ddlMemberId.DataTextField = "ECXNewId";
            ddlMemberId.DataValueField = "ECXNewId";
            ddlMemberId.DataBind();
            ddlMemberId.Items.Insert(0, "--Select--");
            ddlMemberId.Items.FindByText("--Select--").Value = "0";
            ddlMemberId.SelectedIndex = 0;
        }

        private void Populatesession()
        {
            TradeBLL trd =new TradeBLL();
            DataTable dt = new DataTable();
            dt = trd.GetTodaySession();
            ddlSession.DataSource = dt;
            ddlSession.DataTextField = "Name";
            ddlSession.DataValueField = "Name";
            ddlSession.DataBind();
            ddlSession.Items.Insert(0, "--Select--");
            ddlSession.Items.FindByText("--Select--").Value = "0";
            ddlSession.SelectedIndex = 0;

        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            if (txtStartDate.Text != "" && txtEndDate.Text != "")
            {
                var sessionNewName = String.Empty;
                var memmberNewName = String.Empty;
                var repNewName = String.Empty;

                DataTable dt = new DataTable();
                TradeBLL trd = new TradeBLL();
                if (ddlSession.SelectedIndex != 0)
                {
                    sessionNewName = ddlSession.SelectedValue;
                }
                if (ddlMemberId.SelectedIndex != 0)
                {
                    memmberNewName = ddlMemberId.SelectedValue;
                }
                if (ddlRepId.SelectedIndex != -1)
                {
                    repNewName = ddlRepId.SelectedValue;
                }

                //ExportToExcel(trd.GetAttendanceReport(Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), sessionNewId, memmberNewId, repNewId, Convert.ToInt16(ddlTradeAttend.SelectedValue)));

                DataSet ds = trd.GetAttendanceReport(Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), Convert.ToInt16(ddlTradeAttend.SelectedValue));
                dt = ds.Tables[0];
                if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0 && (sessionNewName != "" || memmberNewName != "" || repNewName != ""))
                {
                    dt = ds.Tables[0].AsEnumerable().Where(r => ((string)r["Description"]) == sessionNewName || ((string)r["MemberId"]) == memmberNewName || ((string)r["RepId"]) == repNewName).CopyToDataTable();
                }
                
                
                //DataTable dTable = dt;
                if (!tradingCenter.Equals("All") && dt.Rows.Count >0 )
                {

                    dt = ds.Tables[0].AsEnumerable().Where(r => (r["TradingCenter"]!=DBNull.Value) && ((string)r["TradingCenter"].ToString().Trim()) == tradingCenter).CopyToDataTable();
                    //for (int i = 0; i < dt.Rows.Count; ++i)
                    //{
                    //    if (dt.Rows[i]["TradingCenter"] != DBNull.Value)
                    //    {
                    //        if ( dt.Rows[i]["TradingCenter"].ToString().Trim().Equals(tradingCenter.Trim()))
                    //        {
                    //            dTable.Rows.Add(dt.Rows[i]);
                    //        }
                    //    }
                    //}
                        
                    //dt = ds.Tables[0].AsEnumerable().Where(r => (r["TradingCenter"]!=DBNull.Value) && ((string)r["TradingCenter"]) == tradingCenter).CopyToDataTable();
                }
                ExportToExcel(dt);
            }
        }

        void ExportToExcel(DataTable dt)
        {
            if (dt != null && dt.Rows.Count>0)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearContent();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.Buffer = true;
                HttpContext.Current.Response.ContentType = "application/ms-excel";
                HttpContext.Current.Response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=AttendanceReport.xls");

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

    
        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            

        }

        protected void ddlMemberId_SelectedIndexChanged(object sender, EventArgs e)
        {
            MembershipBLL mb = new MembershipBLL();
            DataTable dt = new DataTable();
            dt = mb.ReperesentativeData(new Guid(ddlMemberId.SelectedValue));
            ddlRepId.DataSource = dt;
            ddlRepId.DataTextField = "IdNo";
            ddlRepId.DataValueField = "IdNo";
            ddlRepId.DataBind();
            //ddlRepId.Items.Insert(0, "--Select--");
            //ddlRepId.Items.FindByText("--Select--").Value = "0";
            //ddlRepId.SelectedIndex = 0;

        }
    }
}





