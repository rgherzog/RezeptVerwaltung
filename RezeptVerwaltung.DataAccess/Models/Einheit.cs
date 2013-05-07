using System;
using System.Collections.Generic;

namespace RezeptVerwaltung.DataAccess.Models
{
    public 
	
 class Einheit
    {
        public Einheit()
        {
            this.Rezepts = new List<Rezept>();
            this.RezeptZutats = new List<RezeptZutat>();
        }

        public int ID { get; set; }
        public string Bezeichnung { get; set; }
        public string Kurzzeichen { get; set; }
        public virtual ICollection<Rezept> Rezepts { get; set; }
        public virtual ICollection<RezeptZutat> RezeptZutats { get; set; }
    }
}
