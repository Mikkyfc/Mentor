using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Mentor.Models
{
    public class State : BaseModel
    {
        public int CountryId { get; set; }
        [ForeignKey("CountryId")]
        public virtual Country Country{ get; set; }
    }
   
}
