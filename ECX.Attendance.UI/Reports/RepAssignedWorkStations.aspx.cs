using ECX.Attendance.BE;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ECX.Attendance.UI.Reports
{
    public partial class RepAssignedWorkStations : System.Web.UI.Page
    {
        float[] rights = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                UserInfo user = new UserInfo();
                user = (UserInfo)Session["LoggedUser"];

                ECXSecurityAccess.ECXSecurityAccess Sec = new ECXSecurityAccess.ECXSecurityAccess();
                rights = Sec.HasRights(user.UniqueIdentifier, new string[] { 
                        "ViewAttenanceReports" }, "");

                if (rights[0] == 1 || rights[0] == 3)
                {
                    if (Session["TradingCenter"] != null)
                    {
                        if (Request.QueryString["Id"] != null)
                        {
                            txtRepId.Text=Request.QueryString["Id"].ToString();
                            PrintToken();
                        }
                    }
                }
                else
                {
                    Response.Redirect("ErrorPage.aspx");
                }
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            PrintToken();           
        }

        private void PrintToken()
        {
            DataSet report = new BLL.MemberAllowedTradeBLL().GetRepAssignedWorkstations(txtRepId.Text.Replace(" ", ""));
            ReportViewer1.Reset();
            //path
            ReportViewer1.LocalReport.ReportEmbeddedResource = "ECX.Attendance.UI.Reports.rptAssignedWorkStation.rdlc";

            UserInfo user = new UserInfo();

            if (Session["LoggedUser"] != null)
            {
                user = (UserInfo)Session["LoggedUser"];
            }
            if (report != null)
            {
                if (report.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrWhiteSpace(txtRepId.Text))
                    {
                        ReportParameter[] parms =
                        {
                            new ReportParameter("MemberName", report.Tables[0].Rows[0]["MemberName"].ToString()),
                            new ReportParameter("RepName", report.Tables[0].Rows[0]["RepName"].ToString()),
                            new ReportParameter("TradingCenter", report.Tables[0].Rows[0]["TradingCenter"].ToString()),
                            new ReportParameter("MemberId", report.Tables[0].Rows[0]["MemberIdNo"].ToString()),
                            new ReportParameter("RepId", report.Tables[0].Rows[0]["RepIdNo"].ToString()),
                            new ReportParameter("Timestamp", report.Tables[0].Rows[0]["TimeStamp"].ToString()),
                            new ReportParameter("IssuedBy", user.UserName.Replace("."," ")),
                        };
                        ReportViewer1.LocalReport.SetParameters(parms);
                    }
                    else
                    {
                        ReportParameter[] parms =
                        {
                            new ReportParameter("MemberName", "All Member"),
                            new ReportParameter("RepName", "All Representative"),
                            new ReportParameter("TradingCenter", "All Trading Center"),
                            new ReportParameter("MemberId", ""),
                            new ReportParameter("RepId", ""),
                            new ReportParameter("Timestamp", report.Tables[0].Rows[0]["TimeStamp"].ToString()),
                            new ReportParameter("IssuedBy", user.UserName.Replace("."," ")),
                        };
                        ReportViewer1.LocalReport.SetParameters(parms);
                    }


                    var rds = new ReportDataSource("DataSet1", report.Tables[0]);
                    ReportViewer1.LocalReport.DataSources.Add(rds);
                    ReportViewer1.LocalReport.DisplayName = "Assigned Token Report";
                    ReportViewer1.LocalReport.Refresh();
                }
            }
        }
    }
}