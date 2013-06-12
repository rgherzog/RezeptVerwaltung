using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RezeptVerwaltung.DataAccess.Models;
using System.Text;

namespace RezeptVerwaltung.Web
{
    public partial class RezeptSuche : System.Web.UI.Page
    {
        private const string SUCHSTRING = "";

        protected void Page_Load(object sender, EventArgs e)
        {
			//set focus to search box
			this.TextBox_Suche.Focus();

            if (!Page.IsPostBack && Session[SUCHSTRING] != null)
            {
                TextBox_Suche.Text = (string)Session[SUCHSTRING];
                PrintSuchErgebnis(TextBox_Suche.Text);
            }
        }

        protected void ButtonSuche_Click(object sender, EventArgs e)
        {
            PrintSuchErgebnis(TextBox_Suche.Text);
            Session.Add(SUCHSTRING, TextBox_Suche.Text);
        }

        private void PrintSuchErgebnis(string seachString)
        {
            //bestehenden Text löschen
            //Table_Suchergebnis.Rows.Clear();

			using (var db = new rherzog_70515_rzvwContext())
            {
                //Rezeptnamen und Zutaten durchsuchen
                var rezepte = db.Rezepts.Where(d => d.RezeptZutats.Where(i => i.Zutat.Name.Contains(seachString)).Count() != 0 
                    || d.Name.Contains(seachString))
                    .ToList();

                //GUI Darstellung

                //nichts gefunden
                if (rezepte.Count == 0)
                {
                    Literal lit = new Literal {Text = Helper.DivPrependAppend("nichts gefunden...")};
                    Panel_Suchergebnis.Controls.Add(lit);
                    return;
                }

                //darstellung ergebnis
                foreach (var rezept in rezepte)
                {
                    //rezept Name
                    LinkButton linkButton = new LinkButton();
                    linkButton.PostBackUrl = "RezeptDetail.aspx?RezeptID=" + rezept.ID;
                    linkButton.CssClass = "RezeptSuche_Rezept";
                    linkButton.Text = rezept.Name;
                    Panel_Suchergebnis.Controls.Add(linkButton);
                    Panel_Suchergebnis.Controls.Add(new Literal { Text = "&nbsp;&nbsp;&nbsp;" });

                    //Zutaten Liste
                    var sb = new StringBuilder();

                    for (int i = 0; i < rezept.RezeptZutats.Count; i++)
                    {
                        var zutats = (List<RezeptZutat>)rezept.RezeptZutats;
                        sb.Append(zutats[i].Zutat.Name);

                        if (i != rezept.RezeptZutats.Count() - 1)
                            sb.Append(", ");
                    }
                    Panel_Suchergebnis.Controls.Add(new Literal { Text = sb.ToString() });

                    //Zeilenumbruch                    
                    Panel_Suchergebnis.Controls.Add(new Literal { Text = "<br />" });
                }
            }
        }
    }
}
