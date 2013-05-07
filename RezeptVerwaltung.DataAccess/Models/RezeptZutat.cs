using System;
using System.Collections.Generic;

namespace RezeptVerwaltung.DataAccess.Models
{
    public 
	
partial class RezeptZutat
    {
        public int ID { get; set; }
        public int RezeptID { get; set; }
        public int ZutatID { get; set; }
        public Nullable<decimal> MengeVon { get; set; }
        public Nullable<int> EinheitID { get; set; }
        public Nullable<int> RezeptabteilungID { get; set; }
        public Nullable<decimal> MengeBis { get; set; }
        public virtual Einheit Einheit { get; set; }
        public virtual Rezept Rezept { get; set; }
        public virtual Rezeptabteilung Rezeptabteilung { get; set; }
        public virtual Zutat Zutat { get; set; }
    }
}
