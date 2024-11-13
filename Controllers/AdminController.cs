using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LAB_FINAL_ACT.Models;

namespace LAB_FINAL_ACT.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult CreateEvent()
        {
            using (var context = new ApplicationDbContext())
            {
                var events = context.Events.ToList(); // Get all events
                return View(events); // Pass events to the view
            }
        }

        [HttpPost]
        public ActionResult Login(string Email, string Password)
        {
            using (var context = new ApplicationDbContext())
            {
                var admin = context.Admin.FirstOrDefault(a => a.Email == Email && a.Password == Password);
                
                if (admin != null)
                {
                    // Create session or authentication cookie
                    Session["AdminId"] = admin.Id;
                    Session["AdminEmail"] = admin.Email;
                    
                    // Redirect to Dashboard
                    return RedirectToAction("Dashboard", "Admin");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password");
                    return View();
                }
            }
        }

        // Add this action method if not already present
        [AdminAuth]
        public ActionResult Dashboard()
        {
            using (var context = new ApplicationDbContext())
            {
                ViewBag.PastEvents = context.Events.Count(e => e.DateAdded < DateTime.Today);
                ViewBag.UpcomingEvents = context.Events.Count(e => e.DateAdded >= DateTime.Today);
                ViewBag.AllEvents = context.Events.Count();
                ViewBag.TotalParticipants = context.Users.Count();
                
                // Get admin name from session
                ViewBag.AdminName = Session["AdminEmail"]?.ToString().Split('@')[0];
                
                return View();
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult CreateEvent(Event model)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var newEvent = new Event
                    {
                        BidetNumber = model.BidetNumber,
                        EventName = model.EventName,
                        SetupLocation = model.SetupLocation,
                        DateAdded = DateTime.ParseExact(model.DateAddedString, "yyyy-MM-dd", null),
                        OpeningHour = TimeSpan.Parse(model.OpeningHourString),
                        ClosingHour = TimeSpan.Parse(model.ClosingHourString),
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    context.Events.Add(newEvent);
                    context.SaveChanges();

                    return Json(new { success = true, message = "Event created successfully!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to create event: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult UpdateEvent(Event model)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var existingEvent = context.Events.Find(model.EventID);
                    if (existingEvent != null)
                    {
                        existingEvent.BidetNumber = model.BidetNumber;
                        existingEvent.EventName = model.EventName;
                        existingEvent.SetupLocation = model.SetupLocation;
                        
                        // Parse date with invariant culture
                        existingEvent.DateAdded = DateTime.ParseExact(
                            model.DateAddedString, 
                            "yyyy-MM-dd", 
                            System.Globalization.CultureInfo.InvariantCulture
                        );
                        
                        // Parse time with proper handling
                        if (TimeSpan.TryParse(model.OpeningHourString, out TimeSpan openingHour))
                        {
                            existingEvent.OpeningHour = openingHour;
                        }
                        
                        if (TimeSpan.TryParse(model.ClosingHourString, out TimeSpan closingHour))
                        {
                            existingEvent.ClosingHour = closingHour;
                        }
                        
                        existingEvent.UpdatedAt = DateTime.Now;

                        context.SaveChanges();
                        return Json(new { success = true, message = "Event updated successfully!" });
                    }
                    return Json(new { success = false, message = "Event not found!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to update event: " + ex.Message });
            }
        }

        [HttpPost]
        public ActionResult DeleteEvent(int id)
        {
            try
            {
                using (var context = new ApplicationDbContext())
                {
                    var eventToDelete = context.Events.Find(id);
                    if (eventToDelete != null)
                    {
                        context.Events.Remove(eventToDelete);
                        context.SaveChanges();
                        return Json(new { success = true, message = "Event deleted successfully!" });
                    }
                    return Json(new { success = false, message = "Event not found!" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Failed to delete event: " + ex.Message });
            }
        }
    }

    // Create this class in your project
    public class AdminAuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["AdminId"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                        { "Controller", "Admin" },
                        { "Action", "Login" }
                    });
            }
        }
    }
}