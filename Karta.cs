using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GraMemory
{
    public class Karta
    {
        public string sciezka { get; set; }
        public Image obrazek { get; set; }

        public bool odslonieta;

        public bool zgadnieta;

        public Karta(Image obrazek)
        {
            this.obrazek = obrazek;
            odslonieta = false;
            zgadnieta = false;
        }
    }
}
