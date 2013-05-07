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

	public static class FractionConverter
	{
		public static string Convert(decimal value)
		{
			// get the whole value of the fraction
			decimal mWhole = Math.Truncate(value);

			// get the fractional value
			decimal mFraction = value - mWhole;

			// initialize a numerator and denomintar
			uint mNumerator = 0;
			uint mDenomenator = 1;

			// ensure that there is actual a fraction
			if (mFraction > 0m)
			{
				// convert the value to a string so that you can count the number of decimal places there are
				string strFraction = mFraction.ToString().Remove(0, 2);

				// store teh number of decimal places
				uint intFractLength = (uint)strFraction.Length;

				// set the numerator to have the proper amount of zeros
				mNumerator = (uint)Math.Pow(10, intFractLength);

				// parse the fraction value to an integer that equals [fraction value] * 10^[number of decimal places]
				uint.TryParse(strFraction, out mDenomenator);

				// get the greatest common divisor for both numbers
				uint gcd = GreatestCommonDivisor(mDenomenator, mNumerator);

				// divide the numerator and the denominator by the gratest common divisor
				mNumerator = mNumerator / gcd;
				mDenomenator = mDenomenator / gcd;
			}

			// create a string builder
			StringBuilder mBuilder = new StringBuilder();

			// add the whole number if it's greater than 0
			if (mWhole > 0m)
			{
				mBuilder.Append(mWhole);
			}

			// add the fraction if it's greater than 0m
			if (mFraction > 0m)
			{
				if (mBuilder.Length > 0)
				{
					mBuilder.Append(" ");
				}

				mBuilder.Append(mDenomenator);
				mBuilder.Append("/");
				mBuilder.Append(mNumerator);
			}

			return mBuilder.ToString();
		}

		public static decimal Convert(string value)
		{
			if (value.Contains("/"))
			{
				var zn = value.Split('/');
				var zählerStr = zn[0];
				var nennerStr = zn[1];
				var zähler = decimal.Parse(zählerStr);
				var nenner = decimal.Parse(nennerStr);

				return zähler / nenner;
			}
			if (!string.IsNullOrEmpty(value))
			{
				return decimal.Parse(value);
			}

			return -1;
		}

		private static uint GreatestCommonDivisor(uint valA, uint valB)
		{
			// return 0 if both values are 0 (no GSD)
			if (valA == 0 &&
			  valB == 0)
			{
				return 0;
			}
			// return value b if only a == 0
			else if (valA == 0 &&
				  valB != 0)
			{
				return valB;
			}
			// return value a if only b == 0
			else if (valA != 0 && valB == 0)
			{
				return valA;
			}
			// actually find the GSD
			else
			{
				uint first = valA;
				uint second = valB;

				while (first != second)
				{
					if (first > second)
					{
						first = first - second;
					}
					else
					{
						second = second - first;
					}
				}

				return first;
			}
		}
	}
}