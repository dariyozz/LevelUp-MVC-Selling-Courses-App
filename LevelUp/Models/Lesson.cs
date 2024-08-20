using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LevelUp.Models
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }

}