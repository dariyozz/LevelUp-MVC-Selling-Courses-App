using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LevelUp.Models
{
    public class Creator
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Creator Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Creator Email")]
        public string Email { get; set; }

        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

        public Creator() { }

        public Creator(int id,string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}