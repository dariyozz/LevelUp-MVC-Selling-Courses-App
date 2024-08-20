using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LevelUp.Models
{
    public class Course
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Add title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Add description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Add price")] public decimal Price { get; set; }

        public Category Category { get; set; }
        public string ImageUrl { get; set; }
        public int CreatorId { get; set; }
        public Creator Creator { get; set; }
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
        public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
        public virtual ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();


        public Course()
        {
        }

        public Course(int id, string title, string description, decimal price, Enum category, string imageUrl,
            Creator creator)
        {
            Id = id;
            Title = title;
            Description = description;
            Price = price;
            Category = (Category)category;
            ImageUrl = imageUrl;
            Creator = creator;
            CreatorId = creator.Id;
        }
    }
}