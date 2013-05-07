using System;
using System.Collections.Generic;

namespace RezeptVerwaltung.DataAccess.Models
{
    public 
	
 class Rezeptabteilung
    {
        public Rezeptabteilung()
        {
            this.RezeptZutats = new List<RezeptZutat>();
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<RezeptZutat> RezeptZutats { get; set; }
    }
}
