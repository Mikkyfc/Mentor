using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mentor.Models
{
    public class Room : BaseModel
    {
        public bool IsAvailable { get; set; }
        public int? HostelId { get; set; }
        [ForeignKey("HostelId")]
        public virtual Hostel Hostel { get; set; }
    }
}
