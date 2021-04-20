using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class StudentViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Please input the email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please input the first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please input the last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please input the password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please input the teacher")]
        public Teacher Teacher { get; set; }

    }
}
