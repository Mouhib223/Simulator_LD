using System.ComponentModel.DataAnnotations.Schema;

namespace SimulatorLD.BuisnessLayer.Models
{
    public class FixMessage
    {
        public int MsgId { get; set; }
        public string MsgBody { get; set; } = null!;
    }
}
