using System;
using ECX.Attendance.BLL;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace ECX.Attendance.UI.Reports
{
    public partial class SellerBuyerReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Populatesession();
            }
        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            if (!txtStartDate.Text.Equals("") && !txtEndDate.Text.Equals(""))
            { 
                string sessionName="";
                if (ddlSession.SelectedIndex != 0)
                {
                    sessionName = ddlSession.SelectedValue.ToString();
                }
                DataTable dt = new TradeBLL().GetBuyerSellerPerSession(Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text), sessionName);
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
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=SellerBuyerReportPerSession.xls");

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

        private void Populatesession()
        {
            TradeBLL trd = new TradeBLL();
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
    }
}