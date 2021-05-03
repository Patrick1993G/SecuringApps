using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShoppingCart.Application.ViewModels
{
    public class StudentAssignmentViewModel
    {
        public Guid Id { get; set; }

        public string File { get; set; }

        [Required(ErrorMessage = "Please input the submitted status")]
        public bool Submitted { get; set; }

        [Required(ErrorMessage = "Please input the student")]
        public StudentViewModel Student { get; set; }

        [Required(ErrorMessage = "Please input the Assignment")]
        public AssignmentViewModel Assignment { get; set; }

        public string Signiture { get; set; }

        public string PublicKey { get; set; }
    }
}
