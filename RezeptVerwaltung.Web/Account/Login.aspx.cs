using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RezeptVerwaltung.Web.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterHyperLink.NavigateUrl = "Register.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.QueryString["ReturnUrl"]);
        }

        protected void myLogin_LoginError(object sender, EventArgs e)
        {
            // Determine why the user could not login...
             LoginUser.FailureText = "Your login attempt was not successful. Please try again.";
        }
    }
}
