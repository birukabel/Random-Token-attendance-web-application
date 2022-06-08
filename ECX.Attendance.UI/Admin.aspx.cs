using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECX.Attendance.BLL;
using ECX.Attendance.BE;
using System.Configuration;
using System.Data;

namespace ECX.Attendance.UI
{
    public partial class Admin : System.Web.UI.Page
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
                        "AccessAttenanceApp" }, "");

                if (rights[0] == 1 || rights[0] == 3)
                {
                    AssigenTextToLabelMessage("", System.Drawing.Color.Green);
                    BindToGrid();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DoSave();

        }

        private void DoSave()
        {
            //NotWorkingWorkStationBLL objSave = new NotWorkingWorkStationBLL();
            UserInfo ui = new UserInfo();
            NotWorkingWorkStation objSave = new NotWorkingWorkStation();
            objSave.TradingCenter = ConfigurationManager.AppSettings[Session["TradingCenter"].ToString()].ToString();
            int ws = Int16.Parse(ConfigurationManager.AppSettings[ConfigurationManager.AppSettings[Session["TradingCenter"].ToString()].ToString()].ToString());

            if (Convert.ToInt32(txtWS.Text) >= 1 && Convert.ToInt32(txtWS.Text) <= ws)
            {
                objSave.TradeDate = DateTime.Now;
                objSave.WorkinStationNumber = txtWS.Text;
                objSave.Status = true;
                ui = (UserInfo)Session["LoggedUser"];
                objSave.CreatedBy = ui.UniqueIdentifier;
                objSave.CreatedDate = DateTime.Now;

                if (new NotWorkingWorkStationBLL().SaveNotWorkingWorkStation(objSave.TradingCenter, objSave.TradeDate, objSave.WorkinStationNumber,
                    objSave.Status, objSave.CreatedBy))
                {
                    BindToGrid();
                    AssigenTextToLabelMessage("Record saved successfuly!", System.Drawing.Color.Green);
                }
                else
                {
                    AssigenTextToLabelMessage("Error while saving record!", System.Drawing.Color.Red);
                }
            }
            else
            {
                AssigenTextToLabelMessage("The number " + txtWS.Text +" is beyond the maximum (" +ws+ ") limit. " , System.Drawing.Color.Red);
            }

        }

        private void BindToGrid()
        {
            gvWS.DataSource = new NotWorkingWorkStationBLL().GetWorkStationsByTradingCennterNDate(ConfigurationManager.AppSettings[Session["TradingCenter"].ToString()], DateTime.Now, DateTime.Now);
            gvWS.DataBind();
        }

        void AssigenTextToLabelMessage(string strMsg, System.Drawing.Color color)
        {
            this.lblMsg.Text = strMsg;
            lblMsg.ForeColor = color;
        }

        protected void gvWS_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Guid id =new Guid( gvWS.DataKeys[e.RowIndex].Value.ToString());
            DoDelete(id);
        }

        private void DoDelete(Guid id)
        {
            bool isDelete = new NotWorkingWorkStationBLL().DeleteNotWorkingStations(id);
            if(isDelete){
                BindToGrid();
                AssigenTextToLabelMessage("Record deleted successfully", System.Drawing.Color.Green);
            }
            else{
                AssigenTextToLabelMessage("Error while deleting record ", System.Drawing.Color.Red);
            }
        }
    }
}