using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SimulatorLD.DBLayer.DAOs
{
    public partial class Rule
    {
        [Key]
        public int RuleId { get; set; }
        [Column("ruleType")]
        public RuleTypesEnum RuleType { get; set; }
        [Column("symbol")]
        public string? Symbol { get; set; }
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }
        public float? MinQty { get; set; }
        public float? MaxQty { get; set; }
        public string? Description { get; set; }
    }
}
