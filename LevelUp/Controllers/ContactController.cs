using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script;
using LevelUp.Models;

namespace LevelUp.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        private readonly ApplicationDbContext _dbContext = new ApplicationDbContext();

        public ContactController()
        {
        }

        // GET: /Contact
        public ActionResult Index()
        {
            return View();
        }

        // POST: /Contact/SendEmail
        [HttpPost]
        public ActionResult SendEmail(Contact contact)
        {
            string message = "";
            if (ModelState.IsValid)
            {
                try
                {
                    // Save contact to database
                    _dbContext.Contacts.Add(contact);
                    _dbContext.SaveChanges();
                    message = "Your message has been sent successfully.";
                }
                catch (Exception ex)
                {
                    message = "An error occurred while saving your message: " + ex.Message;
                }
            }
            else
            {
                message = "Please provide valid contact information.";
            }

            // Redirect back to the contact page
            return PartialView("SendEmail",message);
        }
    }
}