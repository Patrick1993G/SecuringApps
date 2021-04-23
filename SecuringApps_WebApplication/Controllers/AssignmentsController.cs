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
        private readonly IStudentAssignmentsService _studentAssignmentsService;
        private readonly ITeachersService _teachersService;
        private readonly IStudentsService _studentsService;
        public AssignmentsController(IAssignmentsService assignmentsService, ITeachersService teachersService, IStudentsService studentsService, IStudentAssignmentsService studentAssignmentsService)
        {
            _assignmentsService = assignmentsService;
            _studentAssignmentsService = studentAssignmentsService;
            _teachersService = teachersService;
            _studentsService = studentsService;
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
        [Authorize(Roles = "Teacher")]
        public ActionResult Details(Guid id)
        {
            var assignment = _assignmentsService.GetAssignment(id);
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
                if (DateTime.Parse(model.Deadline) > DateTime.Now)
                {
                    model.PublishedDate = DateTime.Today.ToString("dd/MM/yyyy");
                    Guid assignmentId = _assignmentsService.AddAssignment(model);
                    AssignmentViewModel assignment = _assignmentsService.GetAssignment(assignmentId);
                    //add assignment to the teacher's students
                    AllocateAssignmentsToStudents(students, assignment);
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
                }

            }
        }

    }
}
