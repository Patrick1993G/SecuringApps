using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class CommentViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Please input a comment")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Please input the timeStamp")]
        public DateTime Timestamp { get; set; }

        public Teacher Teacher { get; set; }

        public Student Student { get; set; }

        [Required(ErrorMessage = "Please input a StudentAssignment")]
        public StudentAssignment StudentAssignment { get; set; }

    }
}
