using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Temalabor.helper
{
    //String[] és string -ekkel való műveleteket segítő osztály
    static class StringHelper
    {
        /// <summary>
        /// Visszaadja, hogy az s string tartalmazza-e a sub stringet nagyjából 0.74 es pontossággal nézi
        /// </summary>
        /// <param name="s"></param>
        /// <param name="sub"></param>
        /// <returns> -1-et ha nem tartalamazza és másképp az indexet ahonnan kezdve tartalmaza</returns>
        static public int containsSubString(string s, string sub)
        {
            int len = sub.Length - 1;
            int found = -1;
            for (int i = 0; i < s.Length - len; i++)
                if (compareStr(s, sub, i) > 0.74)
                {
                    found = i;
                    break;
                }
            return found;
        }
        /// <summary>
        /// Összehasonlítja, hogy az első string i. indexétől kezdve mennyire hasonlít az s2 stringre
        /// </summary>
        /// <param name="s1"></param>
        /// <param name="s2"></param>
        /// <param name="idx"></param>
        /// <returns> Hogy mennyire hasonlítanak 1-ha ugyanazok és 0 ha egy közös karakterük sincs ugyanazon a helyen</returns>
        static public double compareStr(string s1, string s2, int idx = 0)
        {
            int sim = 0;
            int minLenght = (s1.Length < s2.Length) ? s1.Length : s2.Length;
            for (int i = idx; i < idx + minLenght; i++)
                if (s1[i] == s2[i - idx])
                    sim++;
            double simil = (double)sim / (double)minLenght;
            return simil;
        }

        /// <summary>
        /// megnézi, hogy s string üres-e nagyjából
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static public bool isNotEmpty(string s)
        {
            return s.Length > 2;
        }

        /// <summary>
        /// Egy string tömbből kiveszi az üres sorokat
        /// </summary>
        /// <param name="sa"></param>
        /// <returns>Visszaadja a stringet amiben nincsenek üres sorok</returns>
        static public string[] removeEmptyLines(string[] sa)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in sa)
                if (StringHelper.isNotEmpty(s))
                {
                    sb.Append(s);
                    sb.Append("\n");
                }

            return Regex.Split(sb.ToString(), "\n");
        }
        /// <summary>
        /// Feldatabol egy strignet a whitespace-k mentén
        /// </summary>
        /// <param name="full"></param>
        /// <returns>A feldarabolt string</returns>
        static public string[] cutIntoWords(string full)
        {
            StringBuilder sb = new StringBuilder();
            string[] words = Regex.Split(full, "\\s");
            foreach (string s in words)
                if (StringHelper.isNotEmpty(s))
                {
                    sb.Append(s);
                    sb.Append("\n");
                }
            return Regex.Split(sb.ToString(), "\n");

        }
        /// <summary>
        /// MEgkeresi hogy egy reguláris kifejezésre melyik stringek találnak egy nagyobb stringben és összerakja azokat egy tömmbe
        /// </summary>
        /// <param name="reg"></param>
        /// <param name="from"></param>
        /// <returns>a stringek amik találnak a reguláris kifejezésre</returns>
        static public string[] getMatches(Regex reg, string from)
        {
            StringBuilder sb = new StringBuilder();
            MatchCollection matches = reg.Matches(from);
            foreach (Match m in matches)
            {
                sb.Append(m.Value);
                sb.Append("\n");
            }
            return Regex.Split(sb.ToString(), "\n");
        }
        /// <summary>
        /// Egy stringben megváltoztatja az összes ékezetes betűt nem ékezetessé
        /// </summary>
        /// <param name="text"></param>
        /// <returns>az ékezet néküli string</returns>
        static public string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Egy stringből kitöröl egy másik stringet ha az benne van
        /// </summary>
        /// <param name="from"></param>
        /// <param name="subStr"></param>
        /// <returns>a string amiből ki lett törölve a részlet</returns>
        static public string removeSubString(string from, string subStr)
        {
            int loc = StringHelper.containsSubString(from, subStr);
            if (loc > -1)
                return from.Remove(loc, subStr.Length);
            return from;
        }
        /// <summary>
        /// Egy string tömbből kiveszi az összes olyan stringet amiben nincs szam
        /// </summary>
        /// <param name="from"></param>
        /// <returns>A string tömb amiben csak olyan stringek vannak amikben van szám</returns>
        static public string[] removeNonNumber(string[] from)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in from)
            {
                if (containsNum(s))
                {
                    sb.Append(s);
                    sb.Append("\n");
                }
            }
            return Regex.Split(sb.ToString(), "\n");
        }
        /// <summary>
        /// Megnézi hogy egy stringben van-e szám
        /// </summary>
        /// <param name="s"></param>
        /// <returns>Visszaadja hogy van-e szám a stringben</returns>
        static public bool containsNum(string s)
        {
            bool containsNum = false;
            foreach (char c in s)
            {
                if (c >= '0' && c <= '9')
                    containsNum = true;
            }
            return containsNum;
        }
    }

}
