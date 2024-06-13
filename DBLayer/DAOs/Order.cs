using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SimulatorLD.DBLayer.DAOs
{
    public partial class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public string BeginString { get; set; } = null!;
        [Column("SenderCompID")]
        public string SenderCompId { get; set; } = null!;
        [Column("ClientCompID")]
        public string ClientCompId { get; set; } = null!;
        public string Symbol { get; set; } = null!;
        public string Price { get; set; } = null!;
        public string OrderQuantity { get; set; } = null!;
        public string Side { get; set; } = null!;
        public DateTime TransactTime { get; set; }
        public int RuleId { get; set; }
    }
}
