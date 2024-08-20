using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LevelUp.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Student Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Student Email")]
        public string Email { get; set; }

        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

        public Student()
        {
        }

        public Student(string name, string email)
        {
            Name = name;
            Email = email;
        }
        public Student(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }
    }
}