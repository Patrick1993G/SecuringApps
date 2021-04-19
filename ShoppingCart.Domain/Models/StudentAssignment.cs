using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShoppingCart.Domain.Models
{
    public class StudentAssignment
    {
        [Key]
        public Guid Id { get; set; }

        public string File { get; set; }

        [Required]
        [DefaultValue(false)]
        public bool Submitted { get; set; }

        [ForeignKey("Student")]
        public Guid StudentId { get; set; }
        public virtual Student Student { get; set; }

        [ForeignKey("Assignment")]
        public Guid AssignmentId { get; set; }
        public virtual Assignment Assignment { get; set; }
    }
}
