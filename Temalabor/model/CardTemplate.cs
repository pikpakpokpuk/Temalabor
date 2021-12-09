using System;
using System.Collections.Generic;
using System.Text;

namespace Temalabor.model
{
    /// <summary>
    /// Kártya sablonok szavainak tárolására csinált osztály
    /// </summary>
    public class CardTemplate
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int CardID { get; set; }
        public int Strenght { get; set; }
    }
}
