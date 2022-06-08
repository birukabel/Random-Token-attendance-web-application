using ECX.Attendance.BE;
using ECX.Attendance.BLL;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ECX.Attendance.UI
{
    public partial class RandomAttendance : System.Web.UI.Page
    {
        TradeBLL tradeBLL = new TradeBLL();
        MembershipBLL membershipBLL = new MembershipBLL();
        static ConcurrentDictionary<Guid, string> dicAssigenedToken = new ConcurrentDictionary<Guid, string>();
        float[] rights = null;
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
                    if (Session["TradingCenter"] != null)
                    {
                        string TradingCenetr = "";
                        if (ConfigurationManager.AppSettings.AllKeys.Contains(Session["TradingCenter"].ToString()))
                        {
                            TradingCenetr = ConfigurationManager.AppSettings[Session["TradingCenter"].ToString()].ToString();
                        }
                        else
                        {
                            Response.Redirect("~/ErrorPage.aspx");
                        }

                        lblNoWorkstationsNTradingCenter.Text = "";

                        lblNoWorkstationsNTradingCenter.Text = "Trading Center: " +TradingCenetr + ", No of Workstation: " + ConfigurationManager.AppSettings[TradingCenetr].ToString();

                        InitBLL.InitAllCache();
                        dicAssigenedToken.Clear();
                        AssigenTextToNotificationLabel("", Color.Black);
                    }
                }
                else
                {
                    Response.Redirect("~/ErrorPage.aspx");
                }
            }
        }
        void AssigenTextToNotificationLabel(string strMessage, Color foreColor)
        {
            lblNotification.Text = "";
            lblNotification.Text = strMessage;
            lblNotification.ForeColor = foreColor;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtMemberId.Text.Trim()))
            {
                if (FindAssignedRTC())
                {
                    AssigenTextToNotificationLabel("", Color.Red);
                    dicAssigenedToken.Clear();
                    PopulateRepByMember();
                }
                else
                {
                    lblNotification.ForeColor = Color.Red;
                    lblNotification.Text = "Selected NMDT can't be Assigned to Addis Ababa Trading Center";
                }
            }
            else
            {
                lblNotification.ForeColor = Color.Red;
                lblNotification.Text = "Insert Member Old Id";
            }
        }

        private bool FindAssignedRTC()
        {
            DataTable dt = new BLL.TradeBLL().GetAssignedRTc(txtMemberId.Text.Trim());
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["RegionalRTC"].ToString() != "Addis Ababa" && ConfigurationManager.AppSettings[Session["TradingCenter"].ToString()].ToString() != "Addis Ababa")
                {
                    return true;
                }
                else
                {
                    return false;
                }
                //if (ConfigurationManager.AppSettings[Session["TradingCenter"].ToString()].ToString() == "Addis Ababa" && dt.Rows[0]["RegionalRTC"] != "Addis Ababa")
                //{
                //    return false;

                //}
            }
            else
                return true;
            
            
            return false;
        }

        protected void chkToken_CheckedChanged(object sender, EventArgs e)
        {
            //AssigenTextToNotificationLabel("", Color.Red);
            //if (ddlRep.SelectedIndex > 0)
            //{
            //    List<MemberAllowedTrade> lstMemberAllowedToTrade = new List<MemberAllowedTrade>();
            //    Guid memberId = InitBLL.GetAllMemberData().Where(x => x.ECXOldId == Convert.ToInt32(txtMemberId.Text)).FirstOrDefault().Id;
            //    int noOfWorkstations = Convert.ToInt32(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings[Session["TradingCenter"].ToString()].ToString()].ToString());
            //    string tradingCenter = ConfigurationManager.AppSettings[Session["TradingCenter"].ToString()].ToString();
            //    UserInfo user = new UserInfo();

            //    if (Session["LoggedUser"] != null)
            //    {
            //        user = (UserInfo)Session["LoggedUser"];

            //        if (ddlRep.SelectedValue != null)
            //        {
            //            GridViewRow gvRow = (GridViewRow)(sender as Control).Parent.Parent;
            //            int index = gvRow.RowIndex;

            //            Label sessionName = ((Label)gvMemberAllowedToTrade.Rows[index].Cells[0].FindControl("lblSessionName"));
            //            Guid sId = new Guid(gvMemberAllowedToTrade.DataKeys[index].Value.ToString());

            //            Guid commdoityId = InitBLL.GetAllSession().Where(x => x.Id == sId).FirstOrDefault().CommodityGuid;
            //            if (((CheckBox)gvMemberAllowedToTrade.Rows[index].Cells[0].FindControl("chkSelect")).Checked)
            //            { 
            //                int enteredAs = membershipBLL.GetEnteredAs(new Guid(ddlRep.SelectedValue), sId, memberId);
            //                if (enteredAs == -1)
            //                {
            //                    AssigenTextToNotificationLabel("Member does not have seller or buyer position. Token Assignment has been aborted", Color.Red);
            //                    lstMemberAllowedToTrade.Clear();
                                
            //                }
            //                else
            //                {
                                
            //                    int result =Convert.ToInt32(new MemberAllowedTradeBLL().BulkInsertAttendanceTVPNew(memberId, new Guid(ddlRep.SelectedValue), sId, enteredAs, user.UniqueIdentifier, tradingCenter, noOfWorkstations, 1, commdoityId));
            //                    if(result==1)
            //                        AssigenTextToNotificationLabel("Data Saved Successfully", Color.Green);
            //                    else if(result==2)
            //                        AssigenTextToNotificationLabel("Maximum number of tokens are assigned which is " + noOfWorkstations.ToString() + ". Token Assignment has been aborted", Color.Red);
            //                    else if(result==3)
            //                        AssigenTextToNotificationLabel("Member: " + txtMemberId.Text + " Has Exceeded maximum number of reps for "
            //                            + sessionName.Text + " session. Token Assignment has been aborted.", Color.Red);
            //                    else if(result==4)
            //                        AssigenTextToNotificationLabel("Rep: " + ddlRep.SelectedItem.Text + "  is already assigned for " + sessionName.Text + " session. Token Assignment has been aborted.", Color.Red);
            //                    else
            //                        AssigenTextToNotificationLabel("Error occured while Saving Record.", Color.Red);
            //                    BindToGrid();
            //                }
                                
            //            }
                        
            //        }
            //    }
            //}
        }

        private void PopulateRepByMember()
        {
            if (!string.IsNullOrWhiteSpace(txtMemberId.Text))
            {
                ddlRep.ClearSelection();
                ddlRep.DataSource = new MembershipBLL().GetRepByMemberByIdNo(txtMemberId.Text);
                ddlRep.DataTextField = "RepFullName";
                ddlRep.DataValueField = "Id";
                ddlRep.DataBind();
                ddlRep.Items.Insert(0, "Select Rep");
            }
        }

        protected void ddlRep_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssigenTextToNotificationLabel("", Color.Red);
            if (!string.IsNullOrWhiteSpace(txtMemberId.Text))
            {
                if (ddlRep.SelectedIndex > 0)
                {
                    //DataTable attendanceData = new MemberAllowedTradeBLL().GetMemberAllowedToTradeForMember(txtMemberId.Text);

                    //List<Guid> sessionId = attendanceData.AsEnumerable().Select(x => new Guid(x["SessionId"].ToString())).Distinct().ToList<Guid>();
                    //List<Guid> memberAllowed = InitBLL.GetEligibleMemeber(txtMemberId.Text, sessionId);
                    BindToGrid();

                }
            }

        }

        private void BindToGrid()//DataTable attendance, List<Guid> memberAllowed)
        {
            Guid memberId = InitBLL.GetAllMemberData().Where(x => x.ECXOldId == Convert.ToInt32(txtMemberId.Text)).FirstOrDefault().Id;
            DataTable attendanceData = new MemberAllowedTradeBLL().GetMemberAllowedToTradeForMember(memberId, Convert.ToInt32(rdCommodity.SelectedValue));

            List<Guid> sessionId = attendanceData.AsEnumerable().Select(x => new Guid(x["SessionId"].ToString())).Distinct().ToList<Guid>();
            //List<Guid> memberAllowed = InitBLL.GetEligibleMemeber(txtMemberId.Text, sessionId);

            DataTable repList = new MembershipBLL().GetRepByMemberByIdNo(txtMemberId.Text);
            DataTable dt = new DataTable();
            dt.Columns.Add("SessionId");
            dt.Columns.Add("SessionName");
            dt.Columns.Add("AssignedReps");
            dt.Columns.Add("AssignedTokens");
            dt.Columns.Add("Status");
            dt.Columns.Add("RepIdNo");
            dt.Columns.Add("Token");

            dt.Columns.Add("CreatedByName");
            dt.Columns.Add("CreatedDate");
            dt.Columns.Add("UpdatedByName");
            dt.Columns.Add("UpdatedDate");


            foreach (Guid sId in sessionId)
            {
                DataTable data = attendanceData.AsEnumerable().Where(x => new Guid(x["SessionId"].ToString()) == sId).CopyToDataTable();
                if (data.Rows.Count > 0)
                {
                    DataRow row = dt.NewRow();
                    row["SessionId"] = data.Rows[0]["SessionId"];
                    row["SessionName"] = data.Rows[0]["SessionName"];
                    row["CreatedByName"] = data.Rows[0]["CreatedByName"];
                    row["CreatedDate"] = data.Rows[0]["CreatedDate"];
                    row["UpdatedByName"] = data.Rows[0]["UpdatedByName"];
                    row["UpdatedDate"] = data.Rows[0]["UpdatedDate"];
                    row["RepIdNo"] = ddlRep.SelectedItem.Text;

                    /*if (data.Rows[0]["Status"] != DBNull.Value)
                    {
                        if (data.Rows[0]["RepId"] != DBNull.Value)
                        {
                            if (new Guid(data.Rows[0]["RepId"].ToString()) == new Guid(ddlRep.SelectedValue.ToString()))
                                row["Status"] = Convert.ToBoolean(data.Rows[0]["Status"]) ? "Active" : "In Active";
                        }
                    }
                    else
                    {
                        row["Status"] = "Not Assigned";
                    }*/

                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        if (data.Rows[i]["RepId"] != DBNull.Value && data.Rows[i]["TradingCenter"] != DBNull.Value)
                        {
                            if (data.Rows[i]["TradingCenter"].ToString().Trim() == ConfigurationManager.AppSettings[Session["TradingCenter"].ToString()].ToString())
                            {
                                if (i != 0)
                                {
                                    row["AssignedReps"] = row["AssignedReps"] + ", " + repList.AsEnumerable().Where(x=>new Guid(x["Id"].ToString())==new Guid(data.Rows[i]["RepId"].ToString())).Select(y=>y["IdNo"]).FirstOrDefault();
                                    row["AssignedTokens"] = row["AssignedTokens"] + ", " + data.Rows[i]["Token"];
                                }
                                else
                                {
                                    row["AssignedReps"] = repList.AsEnumerable().Where(x => new Guid(x["Id"].ToString()) == new Guid(data.Rows[i]["RepId"].ToString())).Select(y => y["IdNo"]).FirstOrDefault();
                                    row["AssignedTokens"] = data.Rows[i]["Token"];
                                }
                            }
                        }
                    }
                    dt.Rows.Add(row);
                }
                gvMemberAllowedToTrade.DataSource = dt;
                gvMemberAllowedToTrade.DataBind();
            }

        }

        protected void btnAssigenToken_Click(object sender, EventArgs e)
        {
            if (ddlRep.SelectedIndex > 0)
            {
                List<MemberAllowedTrade> lstMemberAllowedToTrade = new List<MemberAllowedTrade>();
                Guid memberId = InitBLL.GetAllMemberData().Where(x => x.ECXOldId == Convert.ToInt32(txtMemberId.Text)).FirstOrDefault().Id;
                int noOfWorkstations = Convert.ToInt32(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings[Session["TradingCenter"].ToString()].ToString()].ToString());
                string tradingCenter = ConfigurationManager.AppSettings[Session["TradingCenter"].ToString()].ToString();
                UserInfo user = new UserInfo();

                if (Session["LoggedUser"] != null)
                {
                    user = (UserInfo)Session["LoggedUser"];

                    if (ddlRep.SelectedValue != null)
                    {
                        foreach (GridViewRow gvr in this.gvMemberAllowedToTrade.Rows)
                        {

                            if (((CheckBox)gvr.FindControl("chkSelect")).Checked)
                            {
                                Label sessionName = ((Label)gvMemberAllowedToTrade.Rows[gvr.RowIndex].Cells[0].FindControl("lblSessionName"));
                                Guid sId = new Guid(gvMemberAllowedToTrade.DataKeys[gvr.RowIndex].Value.ToString());

                                Guid commdoityId = InitBLL.GetAllSession().Where(x => x.Id == sId).FirstOrDefault().CommodityGuid;

                                int noOfReps = 0; //new LookUpBLL().GetMaximumAllowedReps().AsEnumerable().Where(x => x["TradingCenter"].ToString().Trim() == tradingCenter.Trim() &&
                                  //new Guid(x["CommodityId"].ToString()) == commdoityId).Select(y => Convert.ToInt32(y["NumberOfReps"])).FirstOrDefault();

                                if (commdoityId.Equals(new Guid("71604275-df23-4449-9dae-36501b14cc3b")))
                                {
                                    noOfReps=Convert.ToInt32(ConfigurationManager.AppSettings["NoOfRepsPerMeberForCoffee"]);
                                }
                                else
                                {
                                    noOfReps = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfRepsPerMeberForNonCoffee"]);
                                }
                              

                                //int assignedReps = new MemberAllowedTradeBLL().GetAssignedRepsForMember(memberId, sId, tradingCenter);

                                int assignedReps = new MemberAllowedTradeBLL().GetAssignedRepsForMemberWithOutRTC(memberId, sId);

                                if ((new MemberAllowedTradeBLL().CheckRepIsAssigned(new Guid(ddlRep.SelectedValue), memberId, sId)))
                                {
                                    AssigenTextToNotificationLabel("Rep: " + ddlRep.SelectedItem.Text + "  is already assigned for " + sessionName.Text + " session. Token Assignment has been aborted.", Color.Red);
                                    lstMemberAllowedToTrade.Clear();
                                    break;
                                }
                                if (assignedReps >= noOfReps)
                                {
                                    AssigenTextToNotificationLabel("Member: " + txtMemberId.Text + " Has Exceeded maximum number of reps allowed which is " + noOfReps + " for "
                                        + sessionName.Text + " session. Token Assignment has been aborted.", Color.Red);
                                    lstMemberAllowedToTrade.Clear();
                                    break;
                                }


                                if (new MemberAllowedTradeBLL().CheckIfAllTokensAreAssigned(noOfWorkstations, tradingCenter, sId))
                                {
                                    AssigenTextToNotificationLabel("Maximum number of tokens are assigned which is " + noOfWorkstations.ToString() + ". Token Assignment has been aborted", Color.Red);
                                    lstMemberAllowedToTrade.Clear();
                                    break;
                                }
                                MemberAllowedTrade memberAllowedToTrade = new MemberAllowedTrade();

                                memberAllowedToTrade.EnteredAs = membershipBLL.GetEnteredAs(new Guid(ddlRep.SelectedValue), sId, memberId);
                                if (memberAllowedToTrade.EnteredAs == -1)
                                {
                                    AssigenTextToNotificationLabel("Member does not have seller or buyer position. Token Assignment has been aborted", Color.Red);
                                    lstMemberAllowedToTrade.Clear();
                                    break;
                                }
                                
                                memberAllowedToTrade.Id = Guid.NewGuid();
                                memberAllowedToTrade.TradeDate = DateTime.Now;
                                memberAllowedToTrade.MemberId = memberId;
                                memberAllowedToTrade.RepId = new Guid(ddlRep.SelectedValue);
                                memberAllowedToTrade.SessionId = sId;

                                memberAllowedToTrade.Token = new MemberAllowedTradeBLL().GenerateRandomToken(noOfWorkstations, sId, tradingCenter).ToString();



                                memberAllowedToTrade.IsOnline = true;
                                memberAllowedToTrade.Status = true;
                                memberAllowedToTrade.CreatedBy = user.UniqueIdentifier;
                                memberAllowedToTrade.CreatedDate = DateTime.Now;
                                memberAllowedToTrade.UpdatedBy = user.UniqueIdentifier;
                                memberAllowedToTrade.UpdatedDate = DateTime.Now;
                                memberAllowedToTrade.Additional = false;
                                memberAllowedToTrade.TradingCenter = tradingCenter;

                                lstMemberAllowedToTrade.Add(memberAllowedToTrade);
                            }
                        }
                    }
                    if (lstMemberAllowedToTrade.Count > 0)
                    {

                        if (new MemberAllowedTradeBLL().BulkInsertAttendanceTVP(lstMemberAllowedToTrade))
                        {
                            AssigenTextToNotificationLabel("Attendance record saved successfully", Color.Green);
                            BindToGrid();
                        }
                        else
                        {
                            AssigenTextToNotificationLabel("Error while saving attendance record", Color.Red);

                        }
                    }
                }
            }
            else
            {
                AssigenTextToNotificationLabel("Please Select Rep ", Color.Red);
            }
        }
         
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //if (ddlRep.SelectedIndex > 0)
            //{
            //    foreach (GridViewRow gvr in this.gvMemberAllowedToTrade.Rows)
            //    {
            //        if (((CheckBox)gvr.FindControl("chkSelect")).Checked)
            //        {
            //            Guid sId = new Guid(gvMemberAllowedToTrade.DataKeys[gvr.RowIndex].Value.ToString());
            //            if (new MemberAllowedTradeBLL().DeleteAttendance(new Guid(ddlRep.SelectedValue), sId, ConfigurationManager.AppSettings[Session["TradingCenter"].ToString()].ToString()))
            //            {
            //                BindToGrid();
            //                AssigenTextToNotificationLabel("Record(s) deleted Successfully", Color.Green);
            //            }
            //            else
            //            {
            //                AssigenTextToNotificationLabel("Error while deliting record(s)", Color.Red);
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    AssigenTextToNotificationLabel("Please Select Rep ", Color.Red);
            //}
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            if (ddlRep.SelectedIndex > 0)
            {
                string strRepId = ddlRep.SelectedItem.Text;
                int strLen=strRepId.Length-strRepId.IndexOf('-');
                string strCut=strRepId.Substring(strRepId.IndexOf('-'),strLen);
                strRepId = strRepId.Replace(strCut, "");
                Response.Redirect("~/Reports/RepAssignedWorkStations.aspx?Id=" + strRepId);
            }
        }

        protected void gvMemberAllowedToTrade_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Assign")
            {
                AssigenTextToNotificationLabel("", Color.Red);
                if (ddlRep.SelectedIndex > 0)
                {
                    List<MemberAllowedTrade> lstMemberAllowedToTrade = new List<MemberAllowedTrade>();
                    Guid memberId = InitBLL.GetAllMemberData().Where(x => x.ECXOldId == Convert.ToInt32(txtMemberId.Text)).FirstOrDefault().Id;
                    int noOfWorkstations = Convert.ToInt32(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings[Session["TradingCenter"].ToString()].ToString()].ToString());
                    string tradingCenter = ConfigurationManager.AppSettings[Session["TradingCenter"].ToString()].ToString();
                    UserInfo user = new UserInfo();

                    if (Session["LoggedUser"] != null)
                    {
                        user = (UserInfo)Session["LoggedUser"];

                        if (ddlRep.SelectedValue != null)
                        {
                            LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button
                            GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row
                            GridView myGrid = (GridView)sender; // the gridview
                            Guid sId = new Guid(myGrid.DataKeys[myRow.RowIndex].Value.ToString());

                            Label sessionName = ((Label)gvMemberAllowedToTrade.Rows[myRow.RowIndex].Cells[0].FindControl("lblSessionName"));
                            //Guid sId = new Guid(gvMemberAllowedToTrade.DataKeys[index].Value.ToString());

                            Guid commdoityId = InitBLL.GetAllSession().Where(x => x.Id == sId).FirstOrDefault().CommodityGuid;
                            int enteredAs = membershipBLL.GetEnteredAs(new Guid(ddlRep.SelectedValue), sId, memberId);
                            if (enteredAs == -1)
                            {
                                AssigenTextToNotificationLabel("Member does not have seller or buyer position. Token Assignment has been aborted", Color.Red);
                                lstMemberAllowedToTrade.Clear();

                            }
                            else
                            {

                                int result = Convert.ToInt32(new MemberAllowedTradeBLL().BulkInsertAttendanceTVPNew(memberId, new Guid(ddlRep.SelectedValue), sId, enteredAs, user.UniqueIdentifier, tradingCenter, noOfWorkstations, 1, commdoityId));
                                if (result == 1)
                                    AssigenTextToNotificationLabel("Data Saved Successfully", Color.Green);
                                else if (result == 2)
                                    AssigenTextToNotificationLabel("Maximum number of tokens are assigned which is " + noOfWorkstations.ToString() + ". Token Assignment has been aborted", Color.Red);
                                else if (result == 3)
                                    AssigenTextToNotificationLabel("Member: " + txtMemberId.Text + " Has Exceeded maximum number of reps for "
                                        + sessionName.Text + " session. Token Assignment has been aborted.", Color.Red);
                                else if (result == 4)
                                    AssigenTextToNotificationLabel("Rep: " + ddlRep.SelectedItem.Text + "  is already assigned for " + sessionName.Text + " session. Token Assignment has been aborted.", Color.Red);
                                else
                                    AssigenTextToNotificationLabel("Error occured while Saving Record.", Color.Red);
                                BindToGrid();
                            }



                        }
                    }
                }
            }
            else
            {
                if (ddlRep.SelectedIndex > 0)
                {
                    LinkButton lnkBtn = (LinkButton)e.CommandSource;    // the button
                    GridViewRow myRow = (GridViewRow)lnkBtn.Parent.Parent;  // the row
                    GridView myGrid = (GridView)sender; // the gridview
                    Guid sId = new Guid(myGrid.DataKeys[myRow.RowIndex].Value.ToString());
                    UserInfo user = new UserInfo();

                    if (Session["LoggedUser"] != null)
                    {
                        user = (UserInfo)Session["LoggedUser"];
                    }
                    if (new MemberAllowedTradeBLL().DeleteAttendance(new Guid(ddlRep.SelectedValue), sId, ConfigurationManager.AppSettings[Session["TradingCenter"].ToString()].ToString(), user.UniqueIdentifier))
                    {
                        AssigenTextToNotificationLabel("Record(s) deleted Successfully", Color.Green);
                        BindToGrid();
                    }
                    else
                    {
                        AssigenTextToNotificationLabel("Error while deliting record(s)", Color.Red);
                    }
                }


                else
                {
                    AssigenTextToNotificationLabel("Please Select Rep ", Color.Red);
                }
            }
        }

        protected void rdCommodity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!txtMemberId.Text.Equals("") && ddlRep.SelectedIndex>0)
            {
                BindToGrid();
            }
        }

        protected void gvMemberAllowedToTrade_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}