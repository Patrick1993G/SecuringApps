using System;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Domain.Models
{
    public class Teacher
    {

        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
