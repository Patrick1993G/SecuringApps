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
    public class CommentsController : Controller
    {
        const string SessionKeyName = "_Id";
        private readonly IAssignmentsService _assignmentsService;
        private readonly IStudentAssignmentsService _studentAssignmentsService;
        private readonly ITeachersService _teachersService;
        private readonly IStudentsService _studentsService;
        private readonly ICommentsService _commentsService;
        public CommentsController(IAssignmentsService assignmentsService, ITeachersService teachersService,
            IStudentsService studentsService, IStudentAssignmentsService studentAssignmentsService, ICommentsService commentsService)
        {
            _assignmentsService = assignmentsService;
            _studentAssignmentsService = studentAssignmentsService;
            _teachersService = teachersService;
            _studentsService = studentsService;
            _commentsService = commentsService;
        }
        // GET: CommentsController
        public ActionResult Index( Guid id)
        {
            return View(_commentsService.GetCommentsByAssignmentId(id));
        }

        // GET: CommentsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CommentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CommentViewModel model)
        {
            try
            {
                model.Timestamp = DateTime.Now;
                bool isTeacher = User.IsInRole("Teacher");

                if (isTeacher)
                {
                    var person = _teachersService.getTeacherByEmail(User.Identity.Name);
                    model.Teacher = person;
                }
                else
                {
                    var person = _studentsService.GetStudentByEmail(User.Identity.Name);
                    model.Student = person;
                }
                model.StudentAssignment = _studentAssignmentsService.GetStudentAssignment(new Guid(HttpContext.Session.GetString(SessionKeyName)));
                _commentsService.AddComment(model);
                return RedirectToAction("Index",new { id= new Guid(HttpContext.Session.GetString(SessionKeyName))});
            }
            catch
            {
                return View();
            }
        }

    }
}
