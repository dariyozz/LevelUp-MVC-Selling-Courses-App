using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LevelUp.Models;
using Microsoft.AspNet.Identity;
using WebGrease.Css.Extensions;

namespace LevelUp.Controllers
{
    public class CoursesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Courses
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(db.Courses.ToList());
        }

       

            // GET: Review/AddReview
            public ActionResult AddReview(int id)
            {
                // Ensure the course exists
                var course = db.Courses.Find(id);
                if (course == null)
                {
                    return HttpNotFound();
                }

                // Pass the courseId to the view
                ViewBag.CourseId = id;

                return View();
            }

            // POST: Review/AddReview
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult AddReview(Review review)
            {
                if (ModelState.IsValid)
                {
                    // Find the associated course
                    var course = db.Courses.Find(review.CourseId);
                    if (course == null)
                    {
                        return HttpNotFound();
                    }

                    // Add the review to the course
                    course.Reviews.Add(review);
                    db.SaveChanges();

                    return RedirectToAction("Details", "Courses", new { id = review.CourseId });
                }

                // If ModelState is invalid, return to the AddReview view with the model
                return View(review);
            }
        

        
        [Authorize]
        public ActionResult Enroll(int id)
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // If not authenticated, redirect to the login page with a return URL
                return RedirectToAction("Login", "Account");
            }

            // Fetch the course including the students
            var course = db.Courses.Include(c => c.Students).FirstOrDefault(c => c.Id == id);
            if (course == null)
            {
                return HttpNotFound();
            }

            var studentEmail = User.Identity.GetUserName();
            if (course.Students.Any(s => s.Email == studentEmail))
            {
                return RedirectToAction("Lessons", new { id = id });
            }

            // Create a student record
          
            var createStudent = new Student { Name = User.Identity.Name.Split('@')[0], Email = studentEmail };

            // Check if the student is already enrolled in the course
           

            // Add the student to the course
            course.Students.Add(createStudent);
            db.SaveChanges();

            // Redirect to the Lessons action
            return RedirectToAction("Lessons", new { id = id });
        }



        [Authorize(Roles = "Admin")]
        public ActionResult AddLesson(int id)
        {
            var course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }

            ViewBag.CourseId = id;
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult AddLesson(Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                db.Lessons.Add(lesson);
                db.SaveChanges();
                return RedirectToAction("Lessons","Courses",new{id = lesson.CourseId});
            }
                
            ViewBag.CourseId = lesson.CourseId;
            return View(lesson);
        }

        [Authorize]
        public ActionResult Lessons(int id)
        {
            var course = db.Courses.Include(c => c.Lessons).FirstOrDefault(c => c.Id == id);
            if (course == null)
            {
                return HttpNotFound();
            }

            return View(course.Lessons);
        }

        // GET: Courses/Details/5
        [AllowAnonymous]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var course = db.Courses
                .Include(c => c.Creator)
                .Include(c => c.Students)
                .Include(c => c.Reviews)
                .FirstOrDefault(c => c.Id == id);

            if (course == null)
            {
                return HttpNotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course model)
        {
            if (ModelState.IsValid)
            {
                Creator creator;
                if (db.Creators.Any(c => c.Email == model.Creator.Email))
                {
                    // Create the Creator entity
                    creator = db.Creators.FirstOrDefault(s => s.Email == model.Creator.Email);
                }
                else
                {
                    creator = new Creator
                    {
                        Name = model.Creator.Name,
                        Email = model.Creator.Email
                    };
                    db.Creators.Add(creator);
                    db.SaveChanges();
                }


                // Add the creator to the database
                    

                // Create the Course entity
                var course = new Course
                {
                    Title = model.Title,
                    Description = model.Description,
                    Price = model.Price,
                    Category = model.Category,
                    ImageUrl = model.ImageUrl,
                    Creator = creator,
                    CreatorId = creator.Id // Assign the creator's ID to the Course's CreatorId property
                };

                // Add the course to the database
                db.Courses.Add(course);
                creator.Courses.Add(course);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: Courses/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Course course = db.Courses.Include(c => c.Creator).FirstOrDefault(c => c.Id == id);
            if (course == null)
            {
                return HttpNotFound();
            }

            // Pass the list of creators to the view

            ViewBag.Creators = new SelectList(db.Creators, "Id", "Name", course.CreatorId);
            ViewBag.Categories = Enum.GetNames(typeof(Category))
                .Select(name => new SelectListItem
                {
                    Text = name,
                    Value = name
                });
           
            return View(course);
        }

// POST: Courses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Course course)
        {
            if (ModelState.IsValid)
            {
                var existingCourse = db.Courses.Include(c => c.Creator).FirstOrDefault(c => c.Id == course.Id);
                if (existingCourse != null)
                {
                    existingCourse.Title = course.Title;
                    existingCourse.Description = course.Description;
                    existingCourse.Price = course.Price;
                    existingCourse.Category = course.Category;
                    existingCourse.ImageUrl = course.ImageUrl;

                   

                    db.Entry(existingCourse).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return HttpNotFound();
            }
            
            ViewBag.Creators = new SelectList(db.Creators, "Id", "Name", course.CreatorId);
            ViewBag.Categories = Enum.GetNames(typeof(Category))
                .Select(name => new SelectListItem
                {
                    Text = name,
                    Value = name
                });

            return View(course);
        }


        // GET: Courses/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }


        // GET: Lessons/Edit/5
        public ActionResult EditLesson(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Title", lesson.CourseId);
            return View(lesson);
        }

// POST: Lessons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditLesson([Bind(Include = "Id,Title,Content,CourseId")] Lesson lesson)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lesson).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Lessons", "Courses", new { id = lesson.CourseId }); // Redirect to the lesson list of the course
            }
            ViewBag.CourseId = new SelectList(db.Courses, "Id", "Title", lesson.CourseId);
            return View(lesson);
        }



        // GET: Lessons/Delete/5
        public ActionResult DeleteLesson(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesson lesson = db.Lessons.Find(id);
            if (lesson == null)
            {
                return HttpNotFound();
            }
            return View(lesson);
        }

// POST: Lessons/Delete/5
        [HttpPost, ActionName("DeleteLesson")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedLesson(int id)
        {
            Lesson lesson = db.Lessons.Find(id);
            db.Lessons.Remove(lesson);
            db.SaveChanges();
            return RedirectToAction("Lessons","Courses",new{id = lesson.CourseId}); // Redirect to the lesson list or course details as needed
        }

    }
}