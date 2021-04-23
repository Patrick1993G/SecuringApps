using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecuringApps_WebApplication.Controllers
{
    public class AssignmentsController : Controller
    {
        private readonly IAssignmentsService _assignmentsService;
        private readonly ITeachersService _teachersService;
        public AssignmentsController(IAssignmentsService assignmentsService, ITeachersService teachersService)
        {
            _assignmentsService = assignmentsService;
            _teachersService = teachersService;
        }

        // GET: AssignmentsController
        [Authorize(Roles = "Teacher")]
        public ActionResult Index()
        {
            var teacher = _teachersService.getTeacherByEmail(User.Identity.Name);
            var assignments = _assignmentsService.GetAssignmentsByTeacherId(teacher.Id);
            return View(assignments);
        }

        // GET: AssignmentsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AssignmentsController/Create
        [Authorize(Roles = "Teacher")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: AssignmentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult Create(AssignmentViewModel model)
        {
            try
            {
                model.Teacher = _teachersService.getTeacherByEmail(User.Identity.Name);
                model.Deadline = DateTime.Parse(model.Deadline).ToString("dd/MM/yyyy");
                //adding date validation
                if (DateTime.Parse(model.Deadline) > DateTime.Now)
                {
                    model.PublishedDate = DateTime.Today.ToString("dd/MM/yyyy");
                    _assignmentsService.AddAssignment(model);
                    TempData["feedback"] = "Assignment was added successfully";
                }
                else
                {
                    TempData["warning"] = "Deadline needs to be after Published Date !";
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                TempData["warning"] = "Assignment was not added !" + e.Message;
                return View();
            }
        }

        
    }
}
