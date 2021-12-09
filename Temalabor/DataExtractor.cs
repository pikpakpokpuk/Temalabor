
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Temalabor.helper;

namespace Temalabor
{
    class DataExtractor
    {
        //a programból kapott StringBuilder
        StringBuilder sbBasis;
        //Az össze elfogadott magyar keresztnév 
        string[] firstNames = System.IO.File.ReadAllLines(@"D:\Egyetem\5.felev\Temalabor\firstNames.txt");
        //a kártyák azonosító szavai (a legelső szó amit ki tud olvasni a pdf-ból és nem név, dátum vagy szám
        string[] cardIds;
        //egy adott kártyához tartozó sablon szavak, mint például Név: stb.
        string template;
        //a kártyának amihez tartozik az Id-je
        int cardId;
        //a kiolvasott nevek
        string[] names;
        //a kiolvasott dátumok
        string[] dates;
        //a kiolvasott számokat tratalmazó sorok
        string[] numbers;
        //a teljes szöveg amit ki tudtunk olvasni 
        string full;
        /// <summary>
        /// konstruktor, beállítja az sbBasis-t és a full-t
        /// </summary>
        /// <param name="stringBuilder"></param>
        public DataExtractor(StringBuilder stringBuilder)
        {
            sbBasis = stringBuilder;
            full = sbBasis.ToString();
        }
        /// <summary>
        /// Fő függvény ami kiolvassa az adatokat és kiírja a képernyőre
        /// </summary>
        public void extract()
        {
            initComponents(); //a többi adattag inicializálása
            cardId = setTemplate(); //kiválasztja, hogy milyen fajta kártyából bányászik
            full = StringHelper.RemoveDiacritics(full); //az ékezeteket eltünteti, mert esetek több részében rosszul olvasta be
            //neveket amiket talált kitöröli a full-ból
            foreach (string s in names)
            {
                full = StringHelper.removeSubString(full, s);
            }
            //a sablon alapján kitöröl minden nem hasznos adatot
            string[] important;
            if (cardId > 0)
            {
                important = removeFrameData(StringHelper.cutIntoWords(full), StringHelper.cutIntoWords(template));
                foreach (string s in important)
                {
                    full = StringHelper.removeSubString(full, s);
                }
            }
            else
            {
                //ha eddig nem volt ilyenfajta kartya akkor most az ossze szóját berakja az adatbázisba a sablonhoz
                important = StringHelper.cutIntoWords(full);
                cardId = DbHelper.insertCard(important[0]);
            }
            //a számmal rendelkező sorokat kiszedi és aztán ki is törli a full-ból
            numbers = StringHelper.removeNonNumber(important);
            foreach (string s in numbers)
            {
                full = StringHelper.removeSubString(full, s);
            }
            //a nem szam sorokat amik maradtak betölti az adatbázisba, hogy a következő kártyánál egyből már lehessen törölni, mert nem adatok
            DbHelper.insertNewCardTemplates(StringHelper.cutIntoWords(full), cardId);
            foreach (string s in names)
                Console.WriteLine(s);
            foreach(string s in dates)
                Console.WriteLine(s);
            foreach(string s in numbers)
                Console.WriteLine(s);

        }

        /// <summary>
        /// inicializálja a names és dates adattagokat
        /// a names-be betölti a talált neveket
        /// a dates-be a talált dátumokat, minden dátumok ki is töröl a full-ból
        /// </summary>
        private void initComponents()
        {
            cardIds = DbHelper.getCardIds(); 
            //a keresztneveket nagybetűssé változtatja, mert a kártyákon is úgy van
            toUpperStringArray(firstNames);
            StringBuilder sbDates = new StringBuilder();

            names = getNameFromMatchCollection(StringHelper.getMatches(RegexHelper.nevReg, full), firstNames);

            string[] valDates = StringHelper.getMatches(RegexHelper.dateReg, full);
            removeStrings(valDates);
            foreach (string s in valDates) {
                sbDates.Append(s);
                sbDates.Append("\n");
            }

            valDates = StringHelper.getMatches(RegexHelper.reverseDateReg, full);
            removeStrings(valDates);
            foreach (string s in valDates)
            {
                sbDates.Append(s);
                sbDates.Append("\n");
            }

            valDates = StringHelper.getMatches(RegexHelper.dateBankReg, full);
            removeStrings(valDates);
            foreach (string s in valDates)
            {
                sbDates.Append(s);
                sbDates.Append("\n");
            }
            dates = Regex.Split(sbDates.ToString().Trim(), "\n");
        }

        /// <summary>
        /// Egy stringet nagybetüssé változtat
        /// </summary>
        /// <param name="sa"></param>
        private void toUpperStringArray(string[] sa)
        {
            for (int i = 0; i < sa.Length; i++)
                sa[i] = sa[i].ToUpper();
        }

        /// <summary>
        /// Egy string tömbből megkeresi azokat a stringeket amik tartalmaznak hivatalos keresztnevet
        /// </summary>
        /// <param name="names"></param>
        /// <param name="firstNames"></param>
        /// <returns>visszaadja azokat a stringket amik tartalmaztak hivatalos keresztnevet</returns>
        private string[] getNameFromMatchCollection(string[] names, string[] firstNames)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string name in names)
            {
                foreach (string s in firstNames)
                {
                    if (name.Contains(s))
                    {
                        sb.Append(name);
                        break;
                    }
                }
            }

            string full = sb.ToString();
            return Regex.Split(full.Trim(), "\n");
        }

        /// <summary>
        /// kitörki a data stringtöbből a frame-ben levő stringeket ha benne vannak
        /// </summary>
        /// <param name="data"></param>
        /// <param name="frame"></param>
        /// <returns> a stringtömb amiből ki lettek törölve az elemek</returns>
        private string[] removeFrameData(string[] data, string[] frame)
        {
            StringBuilder sb = new StringBuilder();
            foreach(string s1 in data)
            {
                bool contains = false;
                foreach (string s2 in frame)
                    if(StringHelper.containsSubString(s1, s2) > -1)
                        contains = true;
                if (!contains)
                {
                    sb.Append(s1);
                    sb.Append("\n");
                }
            }
            return StringHelper.removeEmptyLines(Regex.Split(sb.ToString(), "\n"));
        }

        /// <summary>
        /// A fullból kitörli a sa-ban levő stringeket ha benn vannak
        /// </summary>
        /// <param name="sa"></param>
        private void removeStrings(string[] sa)
        {
            foreach (string s in sa)
            {
                full = StringHelper.removeSubString(full, s);
            }
        }

        /// <summary>
        /// megkeresi hogy melyik kártysablonra talál a kártya amit olvas
        /// </summary>
        /// <returns></returns>
        private int setTemplate()
        {
            int id = -1;
            for(int i = 0; i < cardIds.Length; i++)
            {
                if (StringHelper.containsSubString(full, cardIds[i]) > -1)
                {
                    id = i;
                    template = DbHelper.getCardTemplates(id);
                    break;
                }
            }

            return id;
        }
    }
}
