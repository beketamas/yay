using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gyakorlas
{
    internal class Adatok
    {
        int id;
        string orszag;
        int terulet;
        int nepesseg;
        string fovaros;
        int fovarosNepessege;

        public Adatok(string sor)
        {
            string[] tomb = sor.Split(";");
            this.id = int.Parse(tomb[0]);
            this.orszag = tomb[1];
            this.terulet = int.Parse(tomb[2]);
            if (tomb[3].EndsWith('g'))
                this.nepesseg = int.Parse(tomb[3].Trim('g')) * 10000;
            else
                this.nepesseg = int.Parse(tomb[3].Trim('g'));
            this.fovaros = tomb[4];
            this.fovarosNepessege = int.Parse(tomb[5]);
        }
        public int Id => id;
        public string Orszag  => orszag;
        public int Terulet  => terulet;
        public int Nepesseg => nepesseg;
        public string Fovaros => fovaros;
        public int FovarosNepessege => fovarosNepessege;
    }
}
