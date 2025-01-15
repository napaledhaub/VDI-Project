using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VDIProject.Models
{
    [Table("transactions")]
    public class TotalTransaction
    {
        [Key]
        [Column("id")]
        public long ID { get; set; }
        [Column("transaction_id")]
        public string TransactionID { get; set; }
        [Column("type_id")]
        public long TypeID { get; set; }
        [Column("reward_points")]
        public decimal RewardPoints { get; set; }
        [Column("sub_total")]
        public decimal SubTotal { get; set; }
        [Column("discount")]
        public decimal Discount { get; set; }
        [Column("grand_total")]
        public decimal GrandTotal { get; set; }
    }
}
