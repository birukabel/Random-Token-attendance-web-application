using ECX.Attendance.BE;
using ECX.Attendance.BLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ECX.Attendance.UI
{
    public partial class NoOfRepSetting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateCommodity();
                BindToGrid();
            }
        }

        

        private void PopulateCommodity()
        {
            drpCommodity.ClearSelection();
            drpCommodity.DataSource = new LookUpBLL().GetCommodity().AsEnumerable().Where(x => Convert.ToBoolean(x["Status"]) == true).CopyToDataTable() ;
            drpCommodity.DataTextField = "Description";
            drpCommodity.DataValueField = "Guid";
            drpCommodity.DataBind();
            drpCommodity.Items.Insert(0, "Select Commodity");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (drpTradingCenter.SelectedIndex > 0 && drpCommodity.SelectedIndex > 0 && !string.IsNullOrWhiteSpace(txtNoRep.Text))
            {
                UserInfo user = new UserInfo();

                if (Session["LoggedUser"] != null)
                {
                    user = (UserInfo)Session["LoggedUser"];

                    new LookUpBLL().SaveSetting(drpTradingCenter.SelectedItem.ToString(), new Guid(drpCommodity.SelectedValue), Convert.ToInt32(txtNoRep.Text), user.UniqueIdentifier);
                    BindToGrid();
                }
            }
        }

        private void BindToGrid()
        {
            gvNoOfRep.DataSource = new LookUpBLL().GetMaximumAllowedReps();
            gvNoOfRep.DataBind();
        }
    }
}