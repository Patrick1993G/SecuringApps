using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class AssignmentViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Please input the title of the assignment")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please input a description describing the assignment")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please input the date published")]
        public string PublishedDate { get; set; }

        [Required(ErrorMessage = "Please input the deadline date")]
        public string Deadline { get; set; }

        [Required(ErrorMessage = "Please input a Teacher")]
        public Teacher Teacher { get; set; }
    }
}
