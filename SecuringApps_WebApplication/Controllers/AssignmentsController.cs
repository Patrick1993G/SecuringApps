using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SecuringApps_WebApplication.Controllers
{
    public class AssignmentsController : Controller
    {
        private readonly IAssignmentsService _assignmentsService;
        private readonly IStudentAssignmentsService _studentAssignmentsService;
        private readonly ITeachersService _teachersService;
        private readonly IStudentsService _studentsService;
        private readonly ILogger<AssignmentsController> _logger;
        public AssignmentsController(ILogger<AssignmentsController> logger, IAssignmentsService assignmentsService, ITeachersService teachersService, IStudentsService studentsService, IStudentAssignmentsService studentAssignmentsService)
        {
            _assignmentsService = assignmentsService;
            _studentAssignmentsService = studentAssignmentsService;
            _teachersService = teachersService;
            _studentsService = studentsService;
            _logger = logger;
        }

        // GET: AssignmentsController
        [Authorize(Roles = "Teacher")]
        public ActionResult Index()
        {
            var teacher = _teachersService.getTeacherByEmail(User.Identity.Name);
            var assignments = _assignmentsService.GetAssignmentsByTeacherId(teacher.Id);
            _logger.LogInformation($"User {User.Identity.Name} viewed Teacher {teacher.Email} assignments! at date {DateTime.Now} with ip address {HttpContext.Connection.RemoteIpAddress}");
            return View(assignments);
        }

        // GET: AssignmentsController/Details/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Details(String id)
        {
            byte[] encoded = Convert.FromBase64String(id);
            Guid decId = new Guid(Encoding.UTF8.GetString(encoded)); 
            var assignment = _assignmentsService.GetAssignment(decId);
            _logger.LogInformation($"User {User.Identity.Name} viewed details for assignment id= {assignment.Id}! at date {DateTime.Now} with ip address {HttpContext.Connection.RemoteIpAddress}");
            return View(assignment);
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
                var students = _studentsService.GetStudentsByTeacherId(model.Teacher.Id);
                //adding date validation
                if (DateTime.Now.Date < DateTime.Parse(model.Deadline).Date)
                {
                    model.PublishedDate = DateTime.Today.ToString("dd/MM/yyyy");
                    Guid assignmentId = _assignmentsService.AddAssignment(model);
                    AssignmentViewModel assignment = _assignmentsService.GetAssignment(assignmentId);
                    //add assignment to the teacher's students
                    AllocateAssignmentsToStudents(students, assignment);
                    TempData["feedback"] = "Assignment was added successfully";
                    _logger.LogInformation($"User {User.Identity.Name} added assignment id= {assignment.Id}! at date {DateTime.Now} with ip address {HttpContext.Connection.RemoteIpAddress}");
                }
                else
                {
                    TempData["warning"] = "Deadline needs to be after Published Date !";
                    _logger.LogInformation($"User {User.Identity.Name} attempted to add assignment! at date {DateTime.Now} with ip address {HttpContext.Connection.RemoteIpAddress}");
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                TempData["error"] = ("Error occured Oooopppsss! We will look into it!");
                _logger.LogError($"User {User.Identity.Name} attempted to add assignment!! at date {DateTime.Now} with ip address {HttpContext.Connection.RemoteIpAddress} encountered error = {e.Message}");
                return RedirectToAction("Error", "Home");
            }
        }

        private void AllocateAssignmentsToStudents(IQueryable<StudentViewModel> students, AssignmentViewModel assignment)
        {
            IList<StudentViewModel> studentsList = students.ToList();
            foreach (var student in studentsList)
            {
                StudentAssignmentViewModel studentAssignmentViewModel = new StudentAssignmentViewModel();
                studentAssignmentViewModel.Assignment = assignment;
                studentAssignmentViewModel.File = null;
                studentAssignmentViewModel.Student = student;
                if (_studentAssignmentsService.AddStudentAssignment(studentAssignmentViewModel) == null)
                {
                    TempData["warning"] = "Assignment was not assigned to the student!";
                    _logger.LogInformation($"User {User.Identity.Name} attempted to assign assignment to student {student.Id}! at date {DateTime.Now} with ip address {HttpContext.Connection.RemoteIpAddress}");
                }
                else
                {
                    _logger.LogInformation($"User {User.Identity.Name} assigned assignment to student {student.Id} successfully! at date {DateTime.Now} with ip address {HttpContext.Connection.RemoteIpAddress}");
                }
            }
        }

    }
}
