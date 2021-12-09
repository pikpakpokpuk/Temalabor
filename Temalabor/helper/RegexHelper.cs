using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Temalabor.helper
{
    public static class RegexHelper
    {
        //Neveket megtaláló reguláris kifejezés
        static public Regex nevReg = new Regex(@"([A-Z]{2,}-?([A-Z]{2,})?\s[A-Z]{1,11}-?\s?([A-Z]{1,11})?)\s");
        //Dátumokat megtaláló reguláris kifejezések
        static public Regex dateReg = new Regex(@"\d{4}[\s./]\d{1,2}[\s./]\d{1,2}");
        static public Regex reverseDateReg = new Regex(@"\d{1,2}[\s./]\d{1,2}[\s./]\d{4}");
        static public Regex dateBankReg = new Regex(@"\d{2}[\s./]\d{2}");
    }
}
