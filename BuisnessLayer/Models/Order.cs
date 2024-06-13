using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SimulatorLD.BuisnessLayer.Models
{
    public class Order
    {

        public int OrderId { get; set; }
        public string BeginString { get; set; } = null!;
        public string SenderCompId { get; set; } = null!;
        public string ClientCompId { get; set; } = null!;
        public string Symbol { get; set; } = null!;
        public string Price { get; set; } = null!;
        public string OrderQuantity { get; set; } = null!;
        public string Side { get; set; } = null!;
        public DateTime TransactTime { get; set; }
        public int RuleId { get; set; }
    }
}
