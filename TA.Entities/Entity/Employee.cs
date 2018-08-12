using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using TA.Entities.Interfaces;

namespace TA.Entities.Entity
{
    public class Employee : IEmployeeDbEntity
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required] 
        public int Age { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

    }
}
