using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Temalabor.model;

namespace Temalabor.db
{
    public class CardTemplateRepository
    {
        //Az adatbazis elérési címe
        private readonly string connectionString = @"data source=(localdb)\mssqllocaldb;initial catalog=temalabor;integrated security=true";

        //DbContext létrehozása
        private CardTemplateDbContext createDbContext()
        {
            var contextOptionsBuilder = new DbContextOptionsBuilder<CardTemplateDbContext>();
            contextOptionsBuilder.UseSqlServer(connectionString);
            return new CardTemplateDbContext(contextOptionsBuilder.Options);
        }

        /// <summary>
        /// Kiolvasssa az össze elemet a CardTemplates táblából
        /// </summary>
        /// <returns>Az össze elem CardTemplate osztályokban</returns>
        public IReadOnlyCollection<CardTemplate> List()
        {
            IReadOnlyCollection<CardTemplate> cardTemplates = new HashSet<CardTemplate>();

            using (var db = createDbContext())
            {


                var allCardTemplates = db.CardTemplates.Include(t => t.CardID).ToList();

                List<CardTemplate> cards = new List<CardTemplate>();

                foreach (var dp in allCardTemplates)
                {
                    CardTemplate card = new CardTemplate();
                    card.ID = dp.ID;
                    card.Name = dp.Name;
                    card.Strenght = dp.Strenght;
                    card.CardID = dp.CardID.ID;
                    cards.Add(card);
                }

                cardTemplates = cards;


            }
            return cardTemplates;
        }
        /// <summary>
        /// Kiolvasssa az összes elemet a Cards táblából
        /// </summary>
        /// <returns>Az összes elem a Card táblából Card osztályban megadva</returns>
        public IReadOnlyCollection<Card> ListCards()
        {
            IReadOnlyCollection<Card> cardIds = new HashSet<Card>();

            using (var db = createDbContext())
            {


                var allCardIds = db.Cards.ToList();

                List<Card> cards = new List<Card>();

                foreach (var dp in allCardIds)
                {
                    Card card = new Card();
                    card.ID = dp.ID;
                    card.Name = dp.Name;
                    cards.Add(card);
                }

                cardIds = cards;


            }
            return cardIds;
        }

        /// <summary>
        /// Kiolvassa az egy adott kártyához tartozó CardTemplate elemet
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Az adott indezű kártyához tartozó sablon elemek</returns>

        public IReadOnlyCollection<CardTemplate> List(int id)
        {
            IReadOnlyCollection<CardTemplate> cardTemplates = new HashSet<CardTemplate>();

            using (var db = createDbContext())
            {


                var allCardTemplates = from c in db.CardTemplates
                                        where c.CardID.ID == id
                                        select c;

                List<CardTemplate> cards = new List<CardTemplate>();

                foreach (var dp in allCardTemplates)
                {
                    CardTemplate card = new CardTemplate();
                    card.ID = dp.ID;
                    card.Name = dp.Name;
                    card.Strenght = dp.Strenght;
                    card.CardID = dp.CardID.ID;
                    cards.Add(card);
                }

                cardTemplates = cards;


            }
            return cardTemplates;
        }

        /// <summary>
        /// Egy új elemet rak bele a CardTemplate táblába
        /// </summary>
        /// <param name="value"></param>
        /// <returns>az új elem ID-je</returns>
        public int Insert(CardTemplate value)
        {
            int id = 0;

            using (var db = createDbContext())
            {

                var instCardId = (from c in db.Cards
                               where c.ID == value.CardID
                               select c).SingleOrDefault();

                var newCardTemplate = new DbCardTemplate
                {
                    ID = value.ID,
                    Name = value.Name,
                    Strenght = value.Strenght,
                    CardID = instCardId
                };

                db.CardTemplates.Add(newCardTemplate);
                db.SaveChanges();

                id = newCardTemplate.ID;
            }
            return id;
        }

        /// <summary>
        /// Egy új elemet rak bele a Card táblába
        /// </summary>
        /// <param name="s"></param>
        /// <returns>az új elem ID-je</returns>
        public int InsertCard(string s)
        {
            int id = 0;

            using (var db = createDbContext())
            {

                var newCardId = new DbCard
                {
                    Name = s
                };

                db.Cards.Add(newCardId);
                db.SaveChanges();

                id = newCardId.ID;
            }
            return id;
        }

        /// <summary>
        /// frissít egy elemet a CardTemplate táblában
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Update(CardTemplate value)
        {
            int id = 0;

            using(var db = createDbContext())
            {
                var cardTemplate = (from c in db.CardTemplates
                                    where c.ID == value.ID
                                    select c).SingleOrDefault();
                if (cardTemplate != null)
                {
                    cardTemplate.Strenght = value.Strenght;
                    id = cardTemplate.ID;
                }
            }
            return id;
        }
    }
}
