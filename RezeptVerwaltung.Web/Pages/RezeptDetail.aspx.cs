using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RezeptVerwaltung.DataAccess.Models;
using System.Text;
using System.IO;

namespace RezeptVerwaltung.Web
{
    public partial class RezeptDetail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SeiteBefüllen();
        }

        private void SeiteBefüllen()
        {
            var rezeptID = Int32.Parse(Request.Params["RezeptID"]);
            SetEditLink(rezeptID);

			using (var db = new rherzog_70515_rzvwContext())
            {
                //DB Abfrage
                var rezept = db.Rezepts.Where(d => d.ID == rezeptID).FirstOrDefault();

                //Rezept und Kategorie
                Literal_Rezept.Text = rezept.Name;
                Literal_Kategorie.Text = rezept.Kategorie.Name;

                //Zutatüberschrift
                var sb = new StringBuilder();
                sb.Append("Zutaten für ");
                sb.Append(rezept.Menge);
                sb.Append(" ");
                sb.Append(rezept.Einheit.Bezeichnung);
                sb.Append(":");
                sb.Append("</br>");
                Literal_Zutatenüberschrift.Text = sb.ToString();

                //Zutaten
                SeiteBefüllenRezeptabteilungen(rezept);

                //Bild
                var bytes = rezept.Bild;
                if (bytes != null)
                {
                    Helper.SetImageFromByteArray(Image_Rezept, bytes);
                }

                //Anleitung
                Literal_Anleitung.Text = rezept.Anleitung;
            }
        }

        private void SeiteBefüllenRezeptabteilungen(Rezept rezept)
        {
            var zutatenNachRezeptAbteilung = rezept.RezeptZutats.GroupBy(d => d.Rezeptabteilung).Where(d => d.Key != null).OrderBy(d => d.Key.Name).ToList();
            var zutatenOhneRezeptAbteilung = rezept.RezeptZutats.Where(d => d.RezeptabteilungID == null).ToList();            

            //Zutaten mit Rezeptabteilung anzeigen
            var zutatenNachRezeptAbteilungList = zutatenNachRezeptAbteilung.ToList();
            for (int i = 1; i <= zutatenNachRezeptAbteilungList.Count(); i++)
            {
                this.DisplaySingleRezeptabteilung(zutatenNachRezeptAbteilungList[i - 1].Key.RezeptZutats);
            }

            //Zutaten ohne Rezeptabteilung anzeigen
            this.DisplaySingleRezeptabteilung(zutatenOhneRezeptAbteilung);
        }

        private void DisplaySingleRezeptabteilung(ICollection<RezeptZutat> rezeptZutatsForSingleRezeptabteilung)
        {
            if (rezeptZutatsForSingleRezeptabteilung.Count == 0)
                return;            

            //Rezeptabteilung
            var rezeptAbteilung = rezeptZutatsForSingleRezeptabteilung.First().Rezeptabteilung;
            if (rezeptAbteilung != null && rezeptAbteilung.Name != String.Empty)
            {
                Literal litRezeptAbteilungName = new Literal();
                this.Panel_Rezeptabteilungen.Controls.Add(litRezeptAbteilungName);
                litRezeptAbteilungName.Text = rezeptAbteilung.Name;
            }

            //Zutaten List
            ListItem item = null;
            BulletedList bulletedListZutaten = new BulletedList();
            foreach (var rZut in rezeptZutatsForSingleRezeptabteilung)
            {                
                item = new ListItem();
                StringBuilder sb = new StringBuilder();
                if (rZut.Menge != null)
                {
                    sb.Append(rZut.Menge);
                }
                sb.Append(" ");
                //Einheit is optional
                if (rZut.Einheit != null)
                    sb.Append(rZut.Einheit.Bezeichnung);
                else
                    sb.Append(string.Empty);

                sb.Append(" ");
                sb.Append(rZut.Zutat.Name);
                item.Text = sb.ToString();

                bulletedListZutaten.Items.Add(item);
            }

            Panel_Rezeptabteilungen.Controls.Add(bulletedListZutaten);
        }

        private void SetEditLink(int rezeptID)
        {
            HyperLink_Edit.NavigateUrl = "Users/RezeptBearbeiten.aspx?RezeptID=" + rezeptID;
        }

        protected void Button_zurück_Click(object sender, EventArgs e)
        {
            Response.Redirect("RezeptSuche.aspx");
        }       
    }
}