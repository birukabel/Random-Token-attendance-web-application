using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ECX.Attendance.BE;
using System.Web.Security;
using System.Configuration;
using System.Net;
using System.DirectoryServices;
using System.Web.Caching;

namespace ECX.Attendance.UI
{
    public partial class LogIn : System.Web.UI.Page
    {
        static Guid userADID;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (IsAuthenticated(System.Configuration.ConfigurationManager.AppSettings["DirPath"], System.Configuration.ConfigurationManager.AppSettings["domain"], HttpUtility.HtmlEncode(txtUserName.Text), txtPassword.Text, System.Configuration.ConfigurationManager.AppSettings["ACDUser"], System.Configuration.ConfigurationManager.AppSettings["ACDPass"]))
            {
                ECXSecurityAccess.AuthenticationStatus AuStatus = new ECXSecurityAccess.ECXSecurityAccess().IsAuthenticated(
                    HttpUtility.HtmlEncode(txtUserName.Text), txtPassword.Text, "", out userADID);

                if (AuStatus == ECXSecurityAccess.AuthenticationStatus.AccessGranted)
                {
                    UserInfo user = new UserInfo();
                    user.UniqueIdentifier = userADID;
                    user.UserName = HttpUtility.HtmlEncode(txtUserName.Text);
                    user.UniqueIdentifier = userADID;
                    this.Session["LoggedUser"] = user;

                    string ipAddress = "10.1.16.10"; //GetClientIPAddress(Request); // ;"10.1.16.10";
                    string vlan = ipAddress.Substring(0, ipAddress.LastIndexOf('.'));
                    Session["TradingCenter"] = vlan;
                    try
                    {
                        if (ConfigurationManager.AppSettings.AllKeys.Contains(vlan))
                        {
                            this.Response.Redirect("~/RandomAttendance.aspx");
                        }
                        else
                        {
                            this.Response.Redirect("~/Reports/AttendanceReport.aspx");
                        }
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }                  

                }
                else
                {
                    lblStatus.Text = "Invalid user name or password";
                }
            }
            else
            {
                lblStatus.Text = "Invalid user name or password";
            }
        }
        private static string GetClientIPAddress(System.Web.HttpRequest httpRequest)
        {
            //string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
           
            // Get the IP  
            //string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();

            string originalIP = string.Empty;
            string remoteIP = string.Empty;
            originalIP = httpRequest.ServerVariables["HTTP_X_FORWARDED_FOR"]; //original IP will be updated by Proxy/Load Balancer.
            remoteIP = httpRequest.ServerVariables["REMOTE_ADDR"]; //Proxy/Load Balancer IP or original IP if no proxy was used
            if (originalIP != null && originalIP.Trim().Length > 0)
            {
                return originalIP + "(" + remoteIP + ")"; //Lets return both the IPs.
            }
            return remoteIP;
            //return myIP;
        }

        public bool IsAuthenticated(string dirPath, string _domain, string userName, string pwd, string _adAdminUser, string _adAdminPass)
        {
            string domain = _domain;//SettingReader.ADDomainNameForEmployees;
            string LDAP_Path = dirPath;//SettingReader.ADPathForEmployees
            //string container = "OU=Trade, DC=Trade, DC=ECX, DC=com";
            string adAdminUser = _adAdminUser;//System.Configuration.ConfigurationManager.AppSettings["ACDUser"];
            string adAdminPass = _adAdminPass;//System.Configuration.ConfigurationManager.AppSettings["ACDPass"];

            if (string.IsNullOrEmpty(domain) || string.IsNullOrEmpty(LDAP_Path))
                return false;
            string domainAndUsername = domain + "\\" + userName;

            try
            {
                #region Authenticate using Directory Search
                //DirectoryEntry entry = new DirectoryEntry(LDAP_Path, userName, pwd, AuthenticationTypes.Secure | AuthenticationTypes.Sealing);
                using (DirectoryEntry entry = new DirectoryEntry(LDAP_Path, userName, pwd, AuthenticationTypes.Secure | AuthenticationTypes.Sealing | AuthenticationTypes.Signing))
                {
                    //Bind to the native AdsObject to force authentication.
                    object obj = entry.NativeObject;
                    DirectorySearcher search = new DirectorySearcher(entry);

                    search.Filter = "(sAMAccountName=" + userName + ")";
                    search.PropertiesToLoad.Add("CN");
                    SearchResultCollection results = search.FindAll();
                    if (results == null || results.Count == 0)
                    {//no AD record found
                        return false;
                    }
                    if (results.Count > 1)
                    {//multiple AD records were found
                        results.Dispose();
                        return false;
                    }
                    SearchResult result = results[0];//take the first AD Record

                    if (result != null)
                    {
                        DirectoryEntry userADEntry = result.GetDirectoryEntry();
                        userADID = userADEntry.Guid;
                        Session["LoggedUser"] = userADID;
                    }
                    else
                    {
                        return false;
                    }
                    entry.Close();
                    return true;
                }
                #endregion
            }
            catch (Exception ex)
            {
                return false;//authentication fails - let the AD handle the # of trials

                //throw new Exception("Error authenticating user. \n" + ex.Message);
            }
        }
    }
}