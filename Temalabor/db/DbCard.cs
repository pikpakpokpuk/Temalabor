using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Temalabor.db
{
    //A Cards tábla megfelelője az EntityFrameworkben
    [Table("Cards")]
    public class DbCard
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }

        public List<DbCardTemplate> CardTemplates { get; set; }
    }
}
