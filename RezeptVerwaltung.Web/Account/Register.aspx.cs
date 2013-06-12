using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RezeptVerwaltung.Web.Account
{
    public partial class Register : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterUser.ContinueDestinationPageUrl = Request.QueryString["ReturnUrl"];
        }

        protected void RegisterUser_CreatedUser(object sender, EventArgs e)
        {
            //MembershipCreateStatus p = MembershipCreateStatus.Success;
            //Membership.CreateUser(RegisterUser.UserName,
            //RegisterUser.Password, RegisterUser.Email,
            //RegisterUser.Question, RegisterUser.Answer, true, out p);

            //Membership.CreateUser(RegisterUser.UserName, RegisterUser.Password, RegisterUser.Email);

            if (!User.IsInRole("Users"))
            {
                Roles.AddUserToRole(RegisterUser.UserName, "Users");
            }

            FormsAuthentication.SetAuthCookie(RegisterUser.UserName, false /* createPersistentCookie */);

            string continueUrl = RegisterUser.ContinueDestinationPageUrl;
            if (String.IsNullOrEmpty(continueUrl))
            {
                continueUrl = "~/";
            }
            Response.Redirect(continueUrl);
        }

    }
}
