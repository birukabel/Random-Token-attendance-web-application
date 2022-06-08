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
using System.Web.Caching;

namespace ECX.Attendance.UI
{
    public partial class mMenu : System.Web.UI.MasterPage
    {
        protected string MenuBackPath = VirtualPathUtility.ToAbsolute("~/Images/yellowbg1.gif");
        protected void Page_Load(object sender, EventArgs e)
        {
//            this.Session["LoggedUser"] = new Guid("19204415-e4be-4213-a102-723b9de412c6");
        }
      
    }
}
