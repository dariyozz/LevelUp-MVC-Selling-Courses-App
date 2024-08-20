using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LevelUp.Models
{
    public class Review
    {
        public int Id { get; set; }
        [Display(Name = "Reviewer Name")]
        public string ReviewerName { get; set; }
        [Display(Name = "Reviewer Email")]
        public string ReviewerEmail { get; set; }
        [Required]
        public string Content { get; set; }
        public int Rating { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}