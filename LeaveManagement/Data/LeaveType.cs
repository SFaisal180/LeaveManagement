using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Data
{
    public class LeaveType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }

        [Required]
        [Range(1,25,ErrorMessage ="Please Enter A Valid Range")]
        [Display(Name = "Number Of Days")]
        public int DefaultDays { get; set; }
    }
}
