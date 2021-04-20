using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShoppingCart.Domain.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        public virtual Teacher Teacher { get; set; }
        [ForeignKey("Teacher")]
        public Guid? TeacherId { get; set; }

        public virtual Student Student { get; set; }
        [ForeignKey("Student")]
        public Guid? StudentId { get; set; }

        [Required]
        public virtual StudentAssignment StudentAssignment { get; set; }
        [ForeignKey("StudentAssignment")]
        public Guid StudentAssignmentId { get; set; }
    }
}
