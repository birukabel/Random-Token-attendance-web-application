using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECX.Attendance.BLL;
using ECX.Attendance.BE;
using System.Drawing;
using System.Data;

namespace ECX.Attendance.UI
{
    public partial class Home : System.Web.UI.Page
    {
        #region memberVariables

        TradeBLL tradeBLL = new TradeBLL();
        MembershipBLL membershipBLL = new MembershipBLL();
        static ConcurrentDictionary<Guid, string> dicAssigenedToken = new ConcurrentDictionary<Guid, string>();
        float[] rights = null;

        #endregion

        #region memberMethods

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
                        if (!ConfigurationManager.AppSettings.AllKeys.Contains(Session["TradingCenter"].ToString()))
                        {
                            TradingCenetr = "Addis Ababa";
                        }
                        else
                        {
                            TradingCenetr = ConfigurationManager.AppSettings[Session["TradingCenter"].ToString()].ToString();
                        }

                        PopulateSession();
                        lblNoWorkstationsNTradingCenter.Text = "";

                        lblNoWorkstationsNTradingCenter.Text = "Trading Center: " + TradingCenetr + ", No of Workstation: " + ConfigurationManager.AppSettings[TradingCenetr].ToString();

                        InitBLL.InitAllCache();
                        dicAssigenedToken.Clear();
                        AssigenTextToNotificationLabel("", Color.Black);
                    }
                }
                else
                {
                    Response.Redirect("ErrorPage.aspx");
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            dicAssigenedToken.Clear();
            BindToGrid();
        }

        private void BindToGrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RepId");
            dt.Columns.Add("ECXNewId");
            dt.Columns.Add("RepName");
            dt.Columns.Add("Status");
            dt.Columns.Add("Token");

            dt.Columns.Add("CreatedByName");
            dt.Columns.Add("CreatedDate");
            dt.Columns.Add("UpdatedByName");
            dt.Columns.Add("UpdatedDate");
            //AssigenTextToNotificationLabel("", Color.Black);
            if (!txtMemberId.Text.Equals("") && !(ddlSession.SelectedIndex == 0))
            {
                var member = InitBLL.GetEligibleMemeber(txtMemberId.Text, new Guid(ddlSession.SelectedValue));
                if (member != null)
                {
                    List<Representative> lst = InitBLL.GetAllRepresentativeData().Where(x => x.MemberId == member.Id).ToList();
                    if (lst != null)
                    {
                        List<MemberAllowedTrade> dtbl = new MemberAllowedTradeBLL().LoadMemberAllowedToTrade(member.Id, new Guid(ddlSession.SelectedValue));
                        if (dtbl != null)
                        {
                            foreach (MemberAllowedTrade rep in dtbl)
                            {
                                DataRow r = dt.NewRow();
                                r["RepId"] = rep.RepId;

                                r["ECXNewId"] = lst.Where(x => x.Id == rep.RepId).FirstOrDefault().ECXNewId;
                                r["RepName"] = lst.Where(x => x.Id == rep.RepId).Select(y => y.Name).FirstOrDefault();
                                r["Status"] = (rep.Status == true ? "Active" : "In Active");

                                r["Token"] = rep.Token;
                                r["CreatedByName"] = rep.CreatedByName;
                                r["CreatedDate"] = rep.CreatedDate;
                                r["UpdatedByName"] = rep.UpdatedByName;
                                r["UpdatedDate"] = rep.UpdatedDate;
                                dt.Rows.Add(r);
                            }
                            foreach (Representative rep in lst)
                            {
                                if (!dtbl.Exists(x => x.RepId == rep.Id))
                                {
                                    DataRow r = dt.NewRow();
                                    r["RepId"] = rep.Id;
                                    r["ECXNewId"] = rep.ECXNewId;
                                    r["RepName"] = rep.Name;
                                    r["Status"] = "Not Assigned";
                                    r["Token"] = "";
                                    r["CreatedByName"] = "";
                                    r["CreatedDate"] = "";
                                    r["UpdatedByName"] = "";
                                    r["UpdatedDate"] = "";
                                    dt.Rows.Add(r);
                                }
                            }
                        }
                        else
                        {
                            if (lst.Count > 0)
                            {
                                lst.OrderByDescending(x => x.ECXNewId);
                            }
                            foreach (Representative rep in lst)
                            {
                                DataRow r = dt.NewRow();
                                r["RepId"] = rep.Id;
                                r["ECXNewId"] = rep.ECXNewId;
                                r["RepName"] = rep.Name;
                                r["Status"] = "Not Assigned";
                                r["Token"] = "";
                                r["CreatedBy"] = "";
                                r["CreatedDate"] = "";
                                r["UpdatedBy"] = "";
                                r["UpdatedDate"] = "";
                                dt.Rows.Add(r);
                            }
                        }
                    }
                    dt.DefaultView.Sort = "ECXNewId DESC";
                    gvMemberAllowedToTrade.DataSource = dt;
                    gvMemberAllowedToTrade.DataBind();
                    HighLightAssignedTokenOnGVW();
                }

            }
            else
            {
                AssigenTextToNotificationLabel("Please provide member ID NO and select session", Color.Red);
            }
        }

        void HighLightAssignedTokenOnGVW()
        {
            if (gvMemberAllowedToTrade.Rows.Count > 0)
            {
                foreach (GridViewRow gr in gvMemberAllowedToTrade.Rows)
                {
                    TextBox txt = ((TextBox)gr.Cells[0].FindControl("txtToken"));
                    if (!txt.Text.Equals(string.Empty))
                    {
                        txt.ForeColor = Color.Green;
                    }
                }
            }
        }

        void PopulateSession()
        {
            ddlSession.Items.Clear();
            ddlSession.DataSource = tradeBLL.GetCurrentSession();
            ddlSession.DataTextField = "Name";
            ddlSession.DataValueField = "Id";
            ddlSession.DataBind();
            ddlSession.Items.Insert(0, "Select Session");
        }

        void AssigenTextToNotificationLabel(string strMessage, Color foreColor)
        {
            lblNotification.Text = "";
            lblNotification.Text = strMessage;
            lblNotification.ForeColor = foreColor;
        }

        #endregion

        protected void chkToken_CheckedChanged(object sender, EventArgs e)
        {
            int counter = 0;
            AssigenTextToNotificationLabel("", Color.Green);
            foreach (GridViewRow gvr in this.gvMemberAllowedToTrade.Rows)
            {
                TextBox txt = ((TextBox)gvMemberAllowedToTrade.Rows[gvr.RowIndex].Cells[0].FindControl("txtToken"));
                Label lbl = ((Label)gvMemberAllowedToTrade.Rows[gvr.RowIndex].Cells[0].FindControl("lblStatus"));

                if ((((CheckBox)gvr.FindControl("chkSelect")).Checked == true && !txt.Text.Equals("") && lbl.Text.Equals("Not Assigned")) ||
                    (((CheckBox)gvr.FindControl("chkSelect")).Checked == true && !txt.Text.Equals("") && lbl.Text.Equals("Active")) ||
                    lbl.Text.Equals("Active"))
                {
                    ++counter;
                }
            }
            Guid commdoityId = InitBLL.GetAllSession().Where(x => x.Id == new Guid(ddlSession.SelectedValue.ToString())).FirstOrDefault().CommodityGuid;
            int noOfAllowedReps = -1;
            if (commdoityId.Equals(new Guid(ConfigurationManager.AppSettings["CoffeeGuid"].ToString())))
            {//Selected Session if for coffee
                noOfAllowedReps = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfRepsPerMeberForCoffee"].ToString());
            }
            else
            {
                noOfAllowedReps = Convert.ToInt32(ConfigurationManager.AppSettings["NoOfRepsPerMeberForNonCoffee"].ToString());
            }
            if (counter >= noOfAllowedReps)
            {
                AssigenTextToNotificationLabel("Member: " + txtMemberId.Text + " Has Exceeded maximum number of reps allowed which is " + noOfAllowedReps + " for selected session.", Color.Red);
            }
            else
            {
                foreach (GridViewRow gvr in this.gvMemberAllowedToTrade.Rows)
                {
                    TextBox txt = ((TextBox)gvMemberAllowedToTrade.Rows[gvr.RowIndex].Cells[0].FindControl("txtToken"));
                    Label lbl = ((Label)gvMemberAllowedToTrade.Rows[gvr.RowIndex].Cells[0].FindControl("lblStatus"));
                    if (((CheckBox)gvr.FindControl("chkSelect")).Checked == true && txt.Text.Equals("") && lbl.Text.Equals("Not Assigned"))
                    {
                        Guid memberId = InitBLL.GetAllMemberData().Where(x => x.ECXOldId == Convert.ToInt32(txtMemberId.Text)).FirstOrDefault().Id;

                        List<Guid> reps = InitBLL.GetAllRepresentativeData().Where(x => x.MemberId == memberId).Select(y => y.Id).ToList();

                        TextBox repId = ((TextBox)gvMemberAllowedToTrade.Rows[gvr.RowIndex].Cells[0].FindControl("txtRepId"));
                        Guid RepGuid = InitBLL.GetAllRepresentativeData().Where(x => x.ECXNewId == repId.Text).Select(y => y.Id).FirstOrDefault();
                        if (!(new MemberAllowedTradeBLL().CheckRepIsAssigned(RepGuid, memberId, new Guid(ddlSession.SelectedValue.ToString())) && !dicAssigenedToken.ContainsKey(RepGuid)))
                        {
                            //if (!new MemberAllowedTradeBLL().CheckMemberHasExceededMaxNoOfReps(InitBLL.GetAllMemberData().Where(x => x.ECXOldId == Convert.ToInt32(txtMemberId.Text)).FirstOrDefault().Id, new Guid(ddlSession.SelectedValue.ToString()), noOfAllowedReps))
                            //{

                            //Check if all workstations are assigned here
                            string tradingCenter = ConfigurationManager.AppSettings[Session["TradingCenter"].ToString()].ToString();
                            int noOfWorkstations = Convert.ToInt32(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings[Session["TradingCenter"].ToString()].ToString()].ToString());
                            if (new MemberAllowedTradeBLL().CheckIfAllTokensAreAssigned(noOfWorkstations, tradingCenter, new Guid(ddlSession.SelectedValue.ToString())))
                            {
                                AssigenTextToNotificationLabel("Maximum number of tokens are assigned which is " + noOfWorkstations.ToString(), Color.Red);
                            }
                            else
                            {
                                int tokenNumber = new MemberAllowedTradeBLL().GenerateRandomToken(noOfWorkstations, new Guid(ddlSession.SelectedValue.ToString()), dicAssigenedToken, tradingCenter);

                                txt.Text = tokenNumber.ToString();
                                if (!dicAssigenedToken.ContainsKey(new Guid(gvMemberAllowedToTrade.DataKeys[gvr.RowIndex].Value.ToString())))
                                {
                                    dicAssigenedToken.TryAdd(new Guid(gvMemberAllowedToTrade.DataKeys[gvr.RowIndex].Value.ToString()), tokenNumber.ToString());
                                }
                            }
                            //}
                            /*else
                            {
                                AssigenTextToNotificationLabel("Member: " + txtMemberId.Text + " Has Exceeded maximum number of reps allowed which is " + noOfAllowedReps + " for selected session.", Color.Red);
                            }*/
                        }
                        else
                        {
                            AssigenTextToNotificationLabel("Rep: " + repId.Text + " is already assigned for selected session.", Color.Red);
                        }
                    }
                    else if (((CheckBox)gvr.FindControl("chkSelect")).Checked == false)
                    {
                        txt.Text = string.Empty;
                        if (dicAssigenedToken.ContainsKey(new Guid(gvMemberAllowedToTrade.DataKeys[gvr.RowIndex].Value.ToString())))
                        {
                            string reV="";
                            dicAssigenedToken.TryRemove(new Guid(gvMemberAllowedToTrade.DataKeys[gvr.RowIndex].Value.ToString()), out reV);
                        }
                    }
                }
            }
        }

        protected void gvMemberAllowedToTrade_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    CheckBox ck = (CheckBox)e.Row.FindControl("chkToken");
                    TextBox hd = (TextBox)e.Row.FindControl("txtToken");
                    //ck.Checked = Boolean.Parse(hd.Value.ToString());
                }

            }
        }

        protected void gvMemberAllowedToTrade_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            DoAttendanceDelete(new Guid(gvMemberAllowedToTrade.DataKeys[e.RowIndex].Value.ToString()), new Guid(ddlSession.SelectedValue.ToString()));
        }

        private void DoAttendanceDelete(Guid repId, Guid sessionId)
        {
            UserInfo user = new UserInfo();

            if (Session["LoggedUser"] != null)
            {
                user = (UserInfo)Session["LoggedUser"];
            }
            if (new MemberAllowedTradeBLL().DeleteAttendance(repId, sessionId, ConfigurationManager.AppSettings[Session["TradingCenter"].ToString()].ToString(), user.UniqueIdentifier))
            {
                BindToGrid();
                AssigenTextToNotificationLabel("Record deleted Successfully", Color.Green);
            }
            else
            {
                AssigenTextToNotificationLabel("Error while deliting record", Color.Red);
            }
        }

        protected void btnAssigenToken_Click(object sender, EventArgs e)
        {
            if (!txtMemberId.Text.Equals("") && !(ddlSession.SelectedIndex == 0))
            {
                List<MemberAllowedTrade> lstMemberAllowedToTrade = new List<MemberAllowedTrade>();
                Guid memberId = InitBLL.GetAllMemberData().Where(x => x.ECXOldId == Convert.ToInt32(txtMemberId.Text)).FirstOrDefault().Id;

                UserInfo user = new UserInfo();

                if (Session["LoggedUser"] != null)
                {
                    user = (UserInfo)Session["LoggedUser"];


                    if (dicAssigenedToken.Count > 0)
                    {
                        foreach (Guid repId in dicAssigenedToken.Keys)
                        {
                            AssigenTextToNotificationLabel("", Color.Green);
                            MemberAllowedTrade memberAllowedToTrade = new MemberAllowedTrade();

                            memberAllowedToTrade.Id = Guid.NewGuid();
                            memberAllowedToTrade.TradeDate = DateTime.Now;
                            memberAllowedToTrade.MemberId = memberId;
                            memberAllowedToTrade.RepId = repId;
                            memberAllowedToTrade.SessionId = new Guid(ddlSession.SelectedValue.ToString());
                            memberAllowedToTrade.Token = dicAssigenedToken[repId];

                            memberAllowedToTrade.EnteredAs = membershipBLL.GetEnteredAs(repId, new Guid(ddlSession.SelectedValue.ToString()), memberId);
                            if (memberAllowedToTrade.EnteredAs == -1)
                            {
                                AssigenTextToNotificationLabel("Member does not have seller or buyer position", Color.Red);
                                break;
                            }
                            memberAllowedToTrade.IsOnline = true;
                            memberAllowedToTrade.Status = true;
                            memberAllowedToTrade.CreatedBy = user.UniqueIdentifier;
                            memberAllowedToTrade.CreatedDate = DateTime.Now;
                            memberAllowedToTrade.UpdatedBy = user.UniqueIdentifier;
                            memberAllowedToTrade.UpdatedDate = DateTime.Now;
                            memberAllowedToTrade.Additional = false;
                            memberAllowedToTrade.TradingCenter = ConfigurationManager.AppSettings[Session["TradingCenter"].ToString()].ToString();
                            if (new MemberAllowedTradeBLL().CheckTokenIsAssigned(memberAllowedToTrade.Token, memberAllowedToTrade.SessionId, memberAllowedToTrade.TradingCenter))
                            {
                                AssigenTextToNotificationLabel("Generated Token "+ memberAllowedToTrade.Token.ToString() + " is already assigned", Color.Red);
                                dicAssigenedToken.Clear();
                                break;
                            }
                            else
                            {
                                lstMemberAllowedToTrade.Add(memberAllowedToTrade);
                            }
                        }
                    }
                    if (lstMemberAllowedToTrade.Count > 0)
                    {
                        if (new MemberAllowedTradeBLL().BulkInsertAttendanceTVP(lstMemberAllowedToTrade))
                        {
                            AssigenTextToNotificationLabel("Attendance record saved successfully", Color.Green);
                            dicAssigenedToken.Clear();
                            BindToGrid();
                        }
                        else
                        {
                            AssigenTextToNotificationLabel("Error while saving attendance record", Color.Red);
                            dicAssigenedToken.Clear();
                        }
                    }
                }
                else
                {
                    AssigenTextToNotificationLabel("Logged in user information is not avilable", Color.Red);
                }
            }
            else
            {
                AssigenTextToNotificationLabel("Please provide member ID NO and select session", Color.Red);
            }
        }
    }
}