using System;
using System.Collections.Generic;

namespace RezeptVerwaltung.DataAccess.Models
{
    public 
	
 class Kategorie
    {
        public Kategorie()
        {
            this.Rezepts = new List<Rezept>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Rezept> Rezepts { get; set; }
    }
}
