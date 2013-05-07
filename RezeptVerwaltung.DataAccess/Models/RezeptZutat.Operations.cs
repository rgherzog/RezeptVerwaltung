using System;
using System.ComponentModel.DataAnnotations.Schema;
using RezeptVerwaltung.DataAccess.Exceptions;

namespace RezeptVerwaltung.DataAccess.Models
{
	partial class RezeptZutat
	{
		[NotMapped]
		public String Menge
		{
			get
			{
				return MengeBis == null
				       	? FractionConverter.Convert((decimal) MengeVon)
				       	: FractionConverter.Convert((decimal) MengeVon) + " - " + FractionConverter.Convert((decimal) MengeBis);
			}
			set {
				var stringComplete = value;

				if (String.IsNullOrEmpty(value))
					return;

				//remove whitespaces
				stringComplete = stringComplete.Replace(" ", String.Empty);
				var indexSeperator = stringComplete.IndexOf('-');

				const string exceptionMessage = "Zutatenmenge in ungültigem Format. Von - Bis Mengenangaben werden durch einen Bindestrich getrennt; Burchzahlen, Kommazahlen und ganze Zahlen können eingegeben werden";
				if (indexSeperator != -1)
				{
					//from to values present
					var stringVon = stringComplete.Split('-')[0];
					var stringBis = stringComplete.Split('-')[1];

					decimal valueVon = FractionConverter.Convert(stringVon);
					decimal valueBis = FractionConverter.Convert(stringBis);
					if (valueVon != -1 && valueBis != -1)
					{
						MengeVon = FractionConverter.Convert(stringVon);
						MengeBis = FractionConverter.Convert(stringBis);
					}
					else
					{
						throw new NumberFormatException(exceptionMessage + string.Format("Ungültiger Wert: {0}", stringComplete));
					}
				}
				else
				{
					//only one value
					decimal valueVon = FractionConverter.Convert(stringComplete);
					if (valueVon != -1)
					{
						MengeVon = valueVon;
					}
					else
					{
						throw new NumberFormatException(exceptionMessage + string.Format("Ungültiger Wert: {0}", stringComplete));
					}
				}
			}
		}
	}
}
