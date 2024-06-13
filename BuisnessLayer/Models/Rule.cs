using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SimulatorLD.BuisnessLayer.Models
{
    public class Rule
    {

        public int RuleId { get; set; }
        public string? RuleType { get; set; }
        public string? Symbol { get; set; }
        public float? MinPrice { get; set; }
        public float? MaxPrice { get; set; }
        public float? MinQty { get; set; }
        public float? MaxQty { get; set; }
        public string? Description { get; set; }
    }
}
