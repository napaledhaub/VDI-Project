using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VDIProject.Model
{
    [Table("types")]
    public class CustomerType
    {
        [Key]
        [Column("type_id")]
        public long TypeID { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("div")]
        public int Div { get; set; }
        [Column("low")]
        public int Low { get; set; }
        [Column("mid")]
        public int Mid { get; set; }
        [Column("high")]
        public int High { get; set; }
    }

}
