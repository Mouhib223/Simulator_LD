using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SimulatorLD.DBLayer.DAOs
{
    [Table("FIXMessages")]
    public partial class Fixmessage
    {
        [Key]
        public int MsgId { get; set; }
        [Column("msgBody")]
        public string MsgBody { get; set; } = null!;
    }
}
