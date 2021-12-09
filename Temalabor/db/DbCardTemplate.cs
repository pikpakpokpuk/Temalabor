using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Temalabor.db
{
    //A CardTemplates tábla megfelelője az EntityFrameworkben
    [Table("CardTemplates")]
    public class DbCardTemplate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Strenght { get; set; }
        //Idegen kulcs
        [ForeignKey("CardID")]
        public DbCard CardID { get; set; }
    }
}
