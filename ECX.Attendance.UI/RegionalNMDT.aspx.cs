using ECX.Attendance.BE;
using ECX.Attendance.BLL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ECX.Attendance.UI
{
    public partial class RegionalNMDT : System.Web.UI.Page
    {
        #region meberVariables

        float[] rights = null;

        #endregion

        #region memberEvents
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Redirect("~/ErrorPage.aspx?ErrorID=" + "test Error");
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

                        AssigenTextToLabel(" The data imported successfully", System.Drawing.Color.Green);
                    }
                }
                ClearControlsText();
                PopulateTradingCenter();
                AssigenTextToLabel("", System.Drawing.Color.Green);
                BindToGrid();
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            ImportFileToExcel();
            BindToGrid();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DoSave();
        }

        protected void gvList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Guid id = gvList.DataKeys[].SelectedRow.Cells[].Text;
        }

        protected void gvList_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            Guid id = new Guid(gvList.DataKeys[e.NewSelectedIndex].Value.ToString());
            string strName = gvList.SelectedRow.Cells[1].Text;
            string strIdNo = gvList.SelectedRow.Cells[2].Text;
            string strRegional = gvList.SelectedRow.Cells[3].Text;
            populateControls(strName, strIdNo, strRegional);
        }


        #endregion

        #region memberMethods

        void BindToGrid()
        {
            gvList.DataSource = new TradeBLL().GetAllNMDTAssigenedToRTC();
            gvList.DataBind();
        }

        private void PopulateTradingCenter()
        {
            TradeBLL trade = new TradeBLL();
            ddlCenter.DataSource = trade.GetRegionalTradingCenter();
            ddlCenter.DataValueField = "ID";
            ddlCenter.DataTextField = "Name";
            ddlCenter.DataBind();
        }

        private void DoSave()
        {
            int returnedValue=new MembershipBLL().ChekNMDTIsValidForRTC(txtMemberId.Text.Trim());
            if (returnedValue==1)
            {
                TradeBLL trade = new TradeBLL();
                UserInfo user = new UserInfo();

                Guid memberId = new Guid(InitBLL.GetAllMemberData().Where(x => x.ECXNewId == txtMemberId.Text.Trim()).Select(y => y.Id).FirstOrDefault().ToString());
                if (Session["LoggedUser"] != null)
                {
                    user = (UserInfo)Session["LoggedUser"];

                    if (trade.SaveNMDTRegionalRTC(memberId, txtMemberId.Text.Trim(), txtMemberName.Text.Trim(), ddlCenter.SelectedItem.Text, user.UniqueIdentifier))
                    {
                        AssigenTextToLabel("Member regional trading center data saved successfully.", System.Drawing.Color.Green);
                        BindToGrid();
                        ClearControlsText();
                    }
                    else
                    {
                        AssigenTextToLabel("Error while saving data.", System.Drawing.Color.Red);
                    }
                }
            }
            else if (returnedValue == 2)//Not NMDT
            {
                AssigenTextToLabel("The member=" + txtMemberId.Text.Trim() + " is not valid for trading center registration.", System.Drawing.Color.Red);
            }
            else if (returnedValue == 3)//NMDT but assigned to RTC
            {
                AssigenTextToLabel("The member=" + txtMemberId.Text.Trim() + " is already assigned for regional trading center.", System.Drawing.Color.Red);
            }
        }
        void populateControls(string sName, string strIdno, string stRegi)
        {
            txtMemberId.Text = strIdno;
            txtMemberName.Text = sName;
            ddlCenter.SelectedItem.Text = stRegi;
        }
        private void ImportFileToExcel()
        {
            string consString = ConfigurationManager.ConnectionStrings["TradeConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(consString);
            SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con);
            Exception exT = null;
            string strEx = "";
            string excelPath = "";
            try
            {
                excelPath = Server.MapPath("~/UploadFile/") + DateTime.Now.Month + "_" + DateTime.Now.Day + "_" + DateTime.Now.Year + Path.GetFileName(fuImport.PostedFile.FileName);
                fuImport.SaveAs(excelPath);
            }
            catch (SqlException ex)
            {
                exT = ex;
                Response.Redirect("~/ErrorPage.apx?ErrorID=" + exT.ToString());
            }
            catch (Exception ex)
            {
                strEx = exT.ToString() + ex.ToString();
                Response.Redirect("~/ErrorPage.apx?ErrorID=" + strEx);
            }
            finally
            {
                if (Session["LoggedUser"] != null && exT != null)
                {
                    UserInfo userInfo = (UserInfo)Session["LoggedUser"];

                    new TradeBLL().SaveExceptionLog(exT.GetType().ToString(), exT.Message.ToString(), exT.ToString(), userInfo.UserName, userInfo.UniqueIdentifier, "Error ocurred while uploading excel file to server", exT.Source);
                }
            }
            string conString = string.Empty;
            string extension = Path.GetExtension(fuImport.PostedFile.FileName);
            switch (extension)
            {
                case ".xls": //Excel 97-03
                    conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                    break;
                case ".xlsx": //Excel 07 or higher
                    conString = ConfigurationManager.ConnectionStrings["Excel07+ConString"].ConnectionString;
                    break;

            }
            conString = string.Format(conString, excelPath);
            using (OleDbConnection excel_con = new OleDbConnection(conString))
            {
                excel_con.Open();
                string sheet1 = excel_con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null).Rows[0]["TABLE_NAME"].ToString();
                DataTable dtExcelData = new DataTable();
                try
                {
                    using (OleDbDataAdapter oda = new OleDbDataAdapter("SELECT * FROM [" + sheet1 + "]", excel_con))
                    {
                        oda.Fill(dtExcelData);
                    }
                    excel_con.Close();
                }
                catch (SqlException ex)
                {
                    exT = ex;
                    Response.Redirect("~/ErrorPage.apx?ErrorID=" + exT.ToString());
                }
                catch (Exception ex)
                {
                    strEx = exT.ToString() + ex.ToString();
                    Response.Redirect("~/ErrorPage.apx?ErrorID=" + strEx);
                }
                finally
                {
                    if (Session["LoggedUser"] != null && exT != null)
                    {
                        UserInfo userInfo = (UserInfo)Session["LoggedUser"];

                        new TradeBLL().SaveExceptionLog(exT.GetType().ToString(), exT.Message.ToString(), exT.ToString(), userInfo.UserName, userInfo.UniqueIdentifier, "Error ocurred while importing excel file data to datatable", exT.Source);
                    }
                    con.Close();
                }

                //using (SqlConnection con = new SqlConnection(consString))
                //{
                //using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                //{
                //Set the database table name
                try
                {
                    sqlBulkCopy.DestinationTableName = "dbo.tblNMDTRegionalRTC";

                    dtExcelData.Columns.Add("MemberIDNo", typeof(string));
                    dtExcelData.Columns.Add("MemberName", typeof(string));
                    dtExcelData.Columns.Add("RegionalRTC", typeof(string));

                    DataTable dtSave = ConstructDataTable(dtExcelData);

                    con.Open();
                    sqlBulkCopy.WriteToServer(dtSave);
                    AssigenTextToLabel("Member regional trading center data imported successfully", System.Drawing.Color.Green);
                }
                catch (SqlException ex)
                {
                    exT = ex;
                    Response.Redirect("~/ErrorPage.apx?ErrorID=" + exT.ToString());
                    //throw ex;
                }
                catch (Exception ex)
                {
                    strEx = exT.ToString() + ex.ToString();
                    Response.Redirect("~/ErrorPage.apx?ErrorID=" + strEx);
                }
                finally
                {
                    if (Session["LoggedUser"] != null && exT != null)
                    {
                        UserInfo userInfo = (UserInfo)Session["LoggedUser"];

                        new TradeBLL().SaveExceptionLog(exT.GetType().ToString(), exT.Message.ToString(), exT.ToString(), userInfo.UserName, userInfo.UniqueIdentifier, "Error ocurred while bulk copying excel file data to database", exT.Source);
                    }
                    //con.Close();
                }
                //}
                //}
            }
        }

        void AssigenTextToLabel(string strMsg, System.Drawing.Color colr)
        {
            lblMsg.Visible = true;
            lblMsg.Text = strMsg;

            lblMsg.ForeColor = colr;
        }

        DataTable ConstructDataTable(DataTable dtExcel)
        {
            UserInfo user = new UserInfo();
            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("ID", typeof(Guid)));
            dt.Columns.Add(new DataColumn("MemberID", typeof(Guid)));
            dt.Columns.Add(new DataColumn("MemberIdNo", typeof(string)));
            dt.Columns.Add(new DataColumn("MemberName", typeof(string)));
            dt.Columns.Add(new DataColumn("RegionalRTC", typeof(string)));
            dt.Columns.Add(new DataColumn("Status", typeof(bool)));
            dt.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("CreatedBy", typeof(Guid)));

            Exception exT = null;

            try
            {
                if (dtExcel.Rows.Count > 0)
                {
                    DataTable dtLocal = new DataTable();
                    for (int i = 0; i < dtExcel.Rows.Count; i++)
                    {
                        var mIdNo = Convert.ToString(dtExcel.Rows[i]["ID"]).Trim();
                        MembershipBLL nmdt = new MembershipBLL();

                        if (nmdt.ChekNMDTIsValidForRTC(mIdNo)==1)
                        {
                            DataRow drNew = dt.NewRow();
                            drNew["ID"] = Guid.NewGuid();
                            var id = InitBLL.GetAllMemberData().Where(x => x.ECXNewId == Convert.ToString(dtExcel.Rows[i]["ID"]).Trim()).Select(y => y.Id).FirstOrDefault();
                            drNew["MemberID"] = id;
                            drNew["MemberIdNo"] = Convert.ToString(dtExcel.Rows[i]["ID"]).Trim();
                            drNew["MemberName"] = Convert.ToString(dtExcel.Rows[i]["Name"]).Trim();
                            drNew["RegionalRTC"] = Convert.ToString(dtExcel.Rows[i]["Main Trading Center "]).Trim();
                            drNew["Status"] = true;
                            drNew["CreatedDate"] = DateTime.Now;
                            if (Session["LoggedUser"] != null)
                            {
                                user = (UserInfo)Session["LoggedUser"];
                            }
                            drNew["CreatedBy"] = user.UniqueIdentifier;
                            dt.Rows.Add(drNew);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                exT = ex;
                //throw ex;
            }
            finally
            {

                if (Session["LoggedUser"] != null && exT != null)
                {
                    UserInfo userInfo = (UserInfo)Session["LoggedUser"];

                    new TradeBLL().SaveExceptionLog(exT.GetType().ToString(), exT.Message.ToString(), exT.Message.ToString(), userInfo.UserName, userInfo.UniqueIdentifier, "Error ocurred while importing excel file", exT.Source);
                }
            }
            return dt;
        }

        #endregion

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearControlsText();
        }

        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvList.PageIndex = e.NewPageIndex;
            BindToGrid();
        }


        void ClearControlsText()
        {
            txtMemberId.Text = "";
            txtMemberName.Text = "";
        }

    }
}