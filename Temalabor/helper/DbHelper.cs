using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Temalabor.db;
using Temalabor.model;

namespace Temalabor.helper
{
    /// <summary>
    /// Az adatbázisból való kiolvasás és írásta csinált osztály
    /// </summary>
    class DbHelper
    {
        /// <summary>
        /// Kiolvassa az adatbáziból az össze kártya azonosítót (a neveik)
        /// </summary>
        /// <returns>Egy string tömbbe a kártyaazonosítók</returns>
        static public string[] getCardIds()
        {
            StringBuilder sb = new StringBuilder();
            var repo = new CardTemplateRepository();
            var allCardIds = repo.ListCards();
            foreach (Card cardId in allCardIds)
            {
                sb.Append(cardId.Name);
                sb.Append("\n");
            }

            return Regex.Split(sb.ToString(), "\n");
        }

        /// <summary>
        /// beír az adatbázisba egy új kártyaazonosítót
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static public int insertCard(string s)
        {
            var repo = new CardTemplateRepository();
            return repo.InsertCard(s);
        }

        /// <summary>
        /// Egy adott kártyához tartozó sablon szavak
        /// </summary>
        /// <param name="id"></param>
        /// <returns>egy stringben az összes sablon szó ami az adatábázisban van</returns>
        static public string getCardTemplates(int id)
        {
            StringBuilder sb = new StringBuilder();
            var repo = new CardTemplateRepository();
            var cardTemplates = repo.List(id);
            foreach (CardTemplate cardTemplate in cardTemplates)
            {
                sb.Append(cardTemplate.Name);
                sb.Append("\n");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Egy sablonszót update-l
        /// </summary>
        /// <param name="val"></param>
        static public void updateTemplate(CardTemplate val)
        {
            var repo = new CardTemplateRepository();
            repo.Update(val);
        }

        /// <summary>
        /// egy új sablonszót rak bele az adatbázisba
        /// </summary>
        /// <param name="names"></param>
        /// <param name="cardId"></param>
        static public void insertNewCardTemplates(String[] names, int cardId)
        {
            var repo = new CardTemplateRepository();
            foreach(string s in names)
            {
                CardTemplate newTemp = new CardTemplate();
                newTemp.Name = s;
                newTemp.Strenght = 1;
                newTemp.CardID = cardId;
                repo.Insert(newTemp);
            }
        }
    }
}
