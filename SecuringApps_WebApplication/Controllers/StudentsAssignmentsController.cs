using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication.Controllers
{
    public class StudentsAssignmentsController : Controller
    {
        private readonly IAssignmentsService _assignmentsService;
        private readonly IStudentAssignmentsService _studentAssignmentsService;
        private readonly ITeachersService _teachersService;
        private readonly IStudentsService _studentsService;
        private IWebHostEnvironment _environment;
        public StudentsAssignmentsController(IAssignmentsService assignmentsService, ITeachersService teachersService, IStudentsService studentsService, IStudentAssignmentsService studentAssignmentsService, IWebHostEnvironment environment)
        {
            _assignmentsService = assignmentsService;
            _studentAssignmentsService = studentAssignmentsService;
            _teachersService = teachersService;
            _environment = environment;
            _studentsService = studentsService;
        }

        [Authorize (Roles = "Student,Teacher")]
        public ActionResult Index()
        {
            string email = User.Identity.Name;
            var student = _studentsService.GetStudentByEmail(email);
            var assignments = _studentAssignmentsService.GetStudentAssignmentById(student.Id);
            return View(assignments);
        }
        [Authorize(Roles = "Student,Teacher")]
        public ActionResult Details(Guid id)
        {
            var assignment = _studentAssignmentsService.GetStudentAssignment(id);
            return View(assignment);
        }

        [Authorize(Roles = "Student,Teacher")]
        public ActionResult Submit(Guid id)
        {
            return View(_studentAssignmentsService.GetStudentAssignment(id));
        }
        [Authorize(Roles = "Student,Teacher")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Submit(StudentAssignmentViewModel data, IFormFile file)
        {
            var assignment = _studentAssignmentsService.GetStudentAssignment(data.Id);
            var studentAssignment = assignment.Assignment;

            try
            {
                string newFilenameWithAbsolutePath = "";
                DateTime deadline = DateTime.Parse(studentAssignment.Deadline);
                DateTime now = DateTime.Now.Date;
                if ( deadline.Date < now)
                {
                    TempData["warning"] = "Assignment was not submitted Deadline date was exceeded !";
                }
                else
                {
                    if (file != null)
                    {
                        string extension = System.IO.Path.GetExtension(file.FileName);
                        if (extension != ".pdf")
                        {
                            TempData["warning"] = "Assignment needs to be .pdf !";
                        }
                        else
                        {
                            if (file.Length > 0)
                            {
                                string newFilename = Guid.NewGuid() + extension;
                                newFilenameWithAbsolutePath = _environment.ContentRootPath + @"\Assignments\" + newFilename;
                                using (var stream = System.IO.File.Create(newFilenameWithAbsolutePath))
                                {
                                    file.CopyTo(stream);
                                }
                                data.File = @"\Assignments\" + newFilename;
                            }
                        }
                    }
                    _studentAssignmentsService.SubmitAssignment(data.File, data.Id);
                    TempData["feedback"] = "Assignment was submitted successfully";
                }
              
            }
            catch (Exception e)
            {
                TempData["warning"] = "Assignment was not submitted !" + e.Message;
               // _logger.LogError("Error occurred " + e.Message);
                TempData["error"] = ("Error occured Oooopppsss! We will look into it!");
                return RedirectToAction("Error", "Home");
            }
            return View(assignment);
        }
        [Authorize(Roles = "Student,Teacher")]
        public ActionResult Download(Guid id)
        {
            var assignment = _studentAssignmentsService.GetStudentAssignment(id);
            string path = _environment.ContentRootPath + assignment.File;

            string fileName = assignment.Assignment.Title + "/" + assignment.Student.FirstName + " " + assignment.Student.LastName;

            var net = new System.Net.WebClient();
            var data = net.DownloadData(path);
            var ctnt = new System.IO.MemoryStream(data);
            var type = "application/pdf";
            var file = $"{fileName}.pdf";

            return File(ctnt, type, file);
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult GetAllSubmittedAssignments()
        {
            return View(_studentAssignmentsService.GetStudentAssignments());
        }
    }
}
