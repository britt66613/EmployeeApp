using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TA.Entities.Models
{
    public class GetModel
    {
        [Required]        
        public string Id { get; set; }
    }
}
