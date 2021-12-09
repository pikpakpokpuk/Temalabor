using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Temalabor.db
{
    public class CardTemplateDbContext: DbContext
    {
        public CardTemplateDbContext(DbContextOptions<CardTemplateDbContext> options)
            : base(options)
        {
        }

        public DbSet<DbCardTemplate> CardTemplates { get; set; }
        public DbSet<DbCard> Cards { get; set; }
    }
}
