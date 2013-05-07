using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using RezeptVerwaltung.DataAccess.Models;

namespace RezeptVerwaltung.Web
{
    public static class Helper
    {
		public const string REZEPBEARBEITEN_IDENT_ZUTAT = "_Zutat";
		public const string REZEPBEARBEITEN_IDENT_MENGE = "_Menge";
		public const string REZEPBEARBEITEN_IDENT_EINHEIT = "_Einheit";
		public const string REZEPBEARBEITEN_IDENT_REZEPABTEILUNG = "Rezeptabteilung";
		public const string REZEPBEARBEITEN_IDENT_REZEPABTEILUNG_PANEL = "RezeptabteilungPanel";
		public const string REZEPBEARBEITEN_IDENT_NEU = "neu";
		public const string REZEPBEARBEITEN_IDENT_NEUE_ZUTAT_BUTTON = "NeueZutatButton";
		public const string REZEPBEARBEITEN_IDENT_NEUE_REZEPTABTEILUNG_BUTTON = "NeueRezeptButton";
    	public const string ZUTAT_MENGE_FORMAT = "0.00";
        public const string REZEPBEARBEITEN_VIEWSTATE_RezeptAbteilungPanelAnzahl = "RezeptAbteilungPanelAnzahl";

    	public static void SetImageFromByteArray(Image cltr, byte[] bytes)
        {
            var base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
            cltr.ImageUrl = "data:image/png;base64," + base64String;
            cltr.Visible = true;
        }

		public static int? FindIdOnDynamicControl(TextBox zutatCtrl)
		{
			var idStr = zutatCtrl.ID.Remove(0, REZEPBEARBEITEN_IDENT_ZUTAT.Length);

			int id;
			if (Int32.TryParse(idStr, out id))
				return id;

			return null;
		}        

		public static int? FindIdOnDynamicNewControl(TextBox zutatCtrl)
		{
            var idStr = zutatCtrl.ID.Remove(0, REZEPBEARBEITEN_IDENT_ZUTAT.Length + Helper.REZEPBEARBEITEN_IDENT_NEU.Length + Helper.REZEPBEARBEITEN_IDENT_REZEPABTEILUNG_PANEL.Length + Helper.REZEPBEARBEITEN_IDENT_NEU.Length);

			int id;
			if (Int32.TryParse(idStr, out id))
				return id;

			return null;
		}

        public static int? FindIdOnDynamicControl(Panel rezeptAbteilungPanel)
        {
            var idStr = rezeptAbteilungPanel.ID.Remove(0, REZEPBEARBEITEN_IDENT_REZEPABTEILUNG_PANEL.Length);

            int id;
            if (Int32.TryParse(idStr, out id))
                return id;

            return null;
        }  

        public static int? FindIdOnDynamicNewControl(Panel rezeptAbteilungPanel)
        {
            var idStr = rezeptAbteilungPanel.ID.Remove(0, REZEPBEARBEITEN_IDENT_REZEPABTEILUNG_PANEL.Length + Helper.REZEPBEARBEITEN_IDENT_NEU.Length);

            int id;
            if (Int32.TryParse(idStr, out id))
                return id;

            return null;
        } 

		//Line break in panel
		public static void InsertLineBreak(Panel panel)
		{
			var literalBreak = new Literal { Text = "<br/>" };
			panel.Controls.Add(literalBreak);
		}

		//Isolates the RezeptAbteilung ID
		public static string IsolateRezepAbteilungPanelIDFromEventtargetString(string eventTarget)
		{
			if (eventTarget == null || !eventTarget.Contains(Helper.REZEPBEARBEITEN_IDENT_NEUE_ZUTAT_BUTTON)) return null;

			var endIndex = eventTarget.IndexOf(REZEPBEARBEITEN_IDENT_NEUE_ZUTAT_BUTTON, StringComparison.Ordinal);
			endIndex += REZEPBEARBEITEN_IDENT_NEUE_ZUTAT_BUTTON.Length;
			return eventTarget.Remove(0, endIndex);
		}

        public static Control FindControl(Control parentControl, string partOfControlID)
        {
            foreach (Control c in parentControl.Controls)
            {
                if (c.ID != null && c.ID.Contains(partOfControlID))
                    return c;
            }

            return null;
        }
    }
}