using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShoppingCart.Domain.Models
{
    public class Assignment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string PublishedDate { get; set; }

        [Required]
        public string Deadline { get; set; }

        [Required]
        public virtual Teacher Teacher { get; set; }
        [ForeignKey("Teacher")]
        public Guid TeacherId { get; set; }
    }
}
