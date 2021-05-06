using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShoppingCart.Domain.Models
{
    public class Student
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required] 
        public string Password { get; set; }

        [Required]
        public virtual Teacher Teacher { get; set; }
        [ForeignKey("Teacher")]
        public Guid TeacherId { get; set; }
    }
}
