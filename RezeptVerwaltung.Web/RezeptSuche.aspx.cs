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
            Table_Suchergebnis.Rows.Clear();

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
                    var row = new TableRow();

                    TableCell cell = new TableCell();
                    cell.Text = "nichts gefunden...";
                    row.Cells.Add(cell);
                    Table_Suchergebnis.Rows.Add(row);
                    return;
                }

                //darstellung ergebnis
                foreach (var rezept in rezepte)
                {
                    var row = new TableRow();
                    
                    TableCell cell = null;

                    //rezept Name
                    cell = new TableCell();
                    cell.CssClass = "RezeptSuche_Rezept";
                    HyperLink link = new HyperLink();
                    link.NavigateUrl = "RezeptDetail.aspx?RezeptID="+rezept.ID;
                    link.Text = rezept.Name;
                    cell.Controls.Add(link);
                    row.Cells.Add(cell);

                    //Zutaten Liste
                    cell = new TableCell();
                    cell.CssClass = "RezeptSuche_Zutaten";
                    var strBuilder = new StringBuilder();

                    for (int i = 0; i < rezept.RezeptZutats.Count; i++)
                    {
                        var zutats = (List<RezeptZutat>)rezept.RezeptZutats;
                        strBuilder.Append(zutats[i].Zutat.Name);

                        if (i != rezept.RezeptZutats.Count() - 1)
                            strBuilder.Append(", ");
                    }
                    cell.Text = strBuilder.ToString();
                    row.Cells.Add(cell);

                    Table_Suchergebnis.Rows.Add(row);
                }
            }
        }
    }
}
