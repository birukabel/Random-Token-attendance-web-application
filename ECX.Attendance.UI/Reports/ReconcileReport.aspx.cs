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
    public partial class ReconcileReport : System.Web.UI.Page
    {
        #region memberVariables

        float[] rightsAll, rightsAA, rightsHW, rightsHU, rightsAD, rightsGN, rightsNK = null;       

        static string tradingCenter = "";

        #endregion

        #region MembeEvents
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
                }
            }
        } 

        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnRun_Click(object sender, EventArgs e)
        {
            var sessionNewName = "All";
            if (txtStartDate.Text != "" && txtEndDate.Text != "")
            {
                if (ddlSession.SelectedIndex != 0)
                {
                    sessionNewName = ddlSession.SelectedValue;
                }

                DataTable dtOrderNoAttendance = new TradeBLL().GetOrdersWithNoAttendance(Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text));
                DataTable dtAuctionSellOrderNoAttendance = new TradeBLL().GetAuctionSellOrdersWithNoAttendance(Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text));
                DataTable dtAuctionBuyOrderNoAttendance = new TradeBLL().GetAuctionBuyOrdersWithNoAttendance(Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text));

                DataSet dstTrade = new TradeBLL().GetTradesByDateRange(Convert.ToDateTime(txtStartDate.Text), Convert.ToDateTime(txtEndDate.Text));

                DataTable dtReturn = ConstructDataTable(dtOrderNoAttendance, dtAuctionSellOrderNoAttendance, dtAuctionBuyOrderNoAttendance, dstTrade);


                if (dtReturn!= null && dtReturn.Rows.Count > 0 && (sessionNewName != "All"))
                {
                    dtReturn = dtReturn.AsEnumerable().Where(r => ((string)r["Session"]) == sessionNewName).CopyToDataTable();
                }

                ExportToExcel(dtReturn);
            }
            
        }

        DataTable ConstructDataTable(DataTable dtOrdNoAtten,DataTable dtAuSelOrNoAtten,DataTable dtAuBuyOrNoAtten,DataSet dsT)
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("TradeDate",typeof(DateTime)));
            dt.Columns.Add(new DataColumn("Session", typeof(string)));
            dt.Columns.Add(new DataColumn("OrderId", typeof(string)));
            dt.Columns.Add(new DataColumn("MemberId", typeof(string)));
            dt.Columns.Add(new DataColumn("MemberName", typeof(string)));
            dt.Columns.Add(new DataColumn("RepId", typeof(string)));
            dt.Columns.Add(new DataColumn("RepName", typeof(string)));
            dt.Columns.Add(new DataColumn("TradeStatus", typeof(string)));

            if (dtOrdNoAtten.Rows.Count > 0)
            {
                 DataTable dtLocal=new DataTable();
                 foreach (DataRow dr in dtOrdNoAtten.Rows)//Non-Coffee trade
                 {
                     DataRow drNew = dt.NewRow();
                     drNew["TradeDate"] = dr["TradeDate"];
                     drNew["Session"] = dr["Name"];
                     drNew["OrderId"] = dr["OrderId"];
                     drNew["MemberId"] = dr["MemberIDNo"];
                     drNew["MemberName"] = dr["MemberName"];
                     drNew["RepId"] = dr["RepIdNo"];
                     drNew["RepName"] = dr["RepName"];
                     drNew["TradeStatus"] = "No Trade";

                     if (dsT.Tables[0] != null && dsT.Tables[0].Rows.Count > 0)
                     {
                         if (dsT.Tables[0].AsEnumerable().Where(r => ((Guid)r["Id"]) == new Guid(dr["Id"].ToString())).Count() > 0)
                         {
                             if (dsT.Tables[0].AsEnumerable().Where(r => ((Guid)r["Id"]) == new Guid(dr["Id"].ToString()) && (r["TradeStatus"].ToString() == "Accepted" || r["TradeStatus"].ToString() == "AcceptedReconciled" || r["TradeStatus"].ToString() == "Settled")).Count() > 0)
                             {
                                 drNew["TradeStatus"] = "Accepted";//A slong as the status is accepted we don't care about the actual status. i.e., AcceptedReconciled, Settled, Accepted 
                             }
                             else
                             {
                                 drNew["TradeStatus"] = "Rejected";
                             }
                         }
                             
                     }
                     dt.Rows.Add(drNew);
                 }
            }
            if(dtAuSelOrNoAtten.Rows.Count>0 )
            {
                 foreach (DataRow dr in dtAuSelOrNoAtten.Rows)//Coffee Sell trade
                 {
                     DataRow drNew = dt.NewRow();
                     drNew["TradeDate"] = dr["TradeDate"];
                     drNew["Session"] = dr["Name"];
                     drNew["OrderId"] = dr["OrderId"];
                     drNew["MemberId"] = dr["MemberIDNo"];
                     drNew["MemberName"] = dr["MemberName"];
                     drNew["RepId"] = dr["RepIdNo"];
                     drNew["RepName"] = dr["RepName"];
                     drNew["TradeStatus"] = "No Trade";

                     if (dsT.Tables[0] != null && dsT.Tables[0].Rows.Count > 0)
                     {
                         if (dsT.Tables[0].AsEnumerable().Where(r => ((Guid)r["Id"]) == new Guid(dr["Id"].ToString())).Count() > 0)
                         {
                             if (dsT.Tables[0].AsEnumerable().Where(r => ((Guid)r["Id"]) == new Guid(dr["Id"].ToString()) && (r["TradeStatus"].ToString() == "Accepted" || r["TradeStatus"].ToString() == "AcceptedReconciled" || r["TradeStatus"].ToString() == "Settled")).Count() > 0)
                             {
                                 drNew["TradeStatus"] = "Accepted";//A slong as the status is accepted we don't care about the actual status. i.e., AcceptedReconciled, Settled, Accepted 
                             }
                             else
                             {
                                 drNew["TradeStatus"] = "Rejected";
                             }
                         }

                     }
                     dt.Rows.Add(drNew);
                 }
            }
            if (dtAuBuyOrNoAtten.Rows.Count > 0)
            {
                foreach (DataRow dr in dtAuBuyOrNoAtten.Rows)//Coffee Buy trade
                {
                    DataRow drNew = dt.NewRow();
                    drNew["TradeDate"] = dr["TradeDate"];
                    drNew["Session"] = dr["Name"];
                    drNew["OrderId"] = dr["OrderId"];
                    drNew["MemberId"] = dr["MemberIDNo"];
                    drNew["MemberName"] = dr["MemberName"];
                    drNew["RepId"] = dr["RepIdNo"];
                    drNew["RepName"] = dr["RepName"];
                    drNew["TradeStatus"] = "No Trade";

                    if (dsT.Tables[0] != null && dsT.Tables[0].Rows.Count > 0)
                    {
                        if (dsT.Tables[0].AsEnumerable().Where(r => ((Guid)r["Id"]) == new Guid(dr["Id"].ToString())).Count() > 0)
                        {
                            if (dsT.Tables[0].AsEnumerable().Where(r => ((Guid)r["Id"]) == new Guid(dr["Id"].ToString()) && (r["TradeStatus"].ToString() == "Accepted" || r["TradeStatus"].ToString() == "AcceptedReconciled" || r["TradeStatus"].ToString() == "Settled")).Count() > 0)
                            {
                                drNew["TradeStatus"] = "Accepted";//A slong as the status is accepted we don't care about the actual status. i.e., AcceptedReconciled, Settled, Accepted 
                            }
                            else
                            {
                                drNew["TradeStatus"] = "Rejected";
                            }
                        }

                    }
                    dt.Rows.Add(drNew);
                }
            }
            return dt;
        }

        protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {

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
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=AttendanceReconcilationReport.xls");

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

        #region memberMethods

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

        #endregion 
    }
}