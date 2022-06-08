using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace ECX.Attendance.UI
{
    public partial class mTop : System.Web.UI.MasterPage
    {
        protected string BannerPath = VirtualPathUtility.ToAbsolute("~/Images/TopBack.gif");
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
