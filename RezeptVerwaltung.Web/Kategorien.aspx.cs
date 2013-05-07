using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace RezeptVerwaltung.Web
{
    public partial class Kategorien : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

		protected void Button_Einfügen_Click(object sender, EventArgs e)
		{
			SqlDataSourceRezeptVerwaltung.Insert();
			GridView_Kategorien.DataBind();

			TextBox_Bezeichnung.Text = string.Empty;
		}
    }
}
