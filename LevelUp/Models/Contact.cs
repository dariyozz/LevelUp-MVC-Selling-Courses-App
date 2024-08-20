using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Serialization;

namespace LevelUp.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Внесете име")] 
        public string Name { get; set; }
        [Required(ErrorMessage = "Внесете е-маил")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Внесете текст")]
        public string Message { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Contact()
        {
        }

        public Contact(string name, string email, string message)
        {
            Name = name;
            Email = email;
            Message = message;
        }
    }
}