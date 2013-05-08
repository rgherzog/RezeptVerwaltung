using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RezeptVerwaltung.Web
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            sb.Append("&copy;");
            sb.Append("&nbsp;");
            sb.Append(DateTime.Now.Year);
            sb.Append("&nbsp;");
            sb.Append("Reinhart Herzog");
            Literal_footer.Text = sb.ToString();
        }
    }
}
