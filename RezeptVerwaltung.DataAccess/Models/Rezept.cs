using System;
using System.Collections.Generic;

namespace RezeptVerwaltung.DataAccess.Models
{
    public 
	
 class Rezept
    {
        public Rezept()
        {
            this.RezeptZutats = new List<RezeptZutat>();
        }

        public int ID { get; set; }
        public int KategorieID { get; set; }
        public string Name { get; set; }
        public byte[] Bild { get; set; }
        public string Menge { get; set; }
        public int EinheitID { get; set; }
        public string Anleitung { get; set; }
        public string Kommentar { get; set; }
        public virtual Einheit Einheit { get; set; }
        public virtual Kategorie Kategorie { get; set; }
        public virtual ICollection<RezeptZutat> RezeptZutats { get; set; }
    }
}
