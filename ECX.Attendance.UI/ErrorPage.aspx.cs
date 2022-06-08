using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ECX.Attendance.UI
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Request.QueryString["ErrorID"]))
            {
                var h1 = new HtmlGenericControl("h1") { InnerText = "Error Detail" };
               pnlError.Controls.Add(h1);

               var p = new HtmlGenericControl("p") { InnerText = Request.QueryString["ErrorID"] };
                pnlError.Controls.Add(p);
            }
        }
    }
}