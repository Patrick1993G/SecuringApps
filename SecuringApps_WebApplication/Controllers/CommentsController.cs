using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using System;
using WebApplication.ActionFilters;

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
        private readonly ILogger<CommentsController> _logger;
        public CommentsController(ILogger<CommentsController> logger,IAssignmentsService assignmentsService, ITeachersService teachersService,
            IStudentsService studentsService, IStudentAssignmentsService studentAssignmentsService, ICommentsService commentsService)
        {
            _assignmentsService = assignmentsService;
            _studentAssignmentsService = studentAssignmentsService;
            _teachersService = teachersService;
            _studentsService = studentsService;
            _commentsService = commentsService;
            _logger = logger;
        }
        // GET: CommentsController
        [ActionFilter]
        public ActionResult Index( String id)
        {
            byte[] encoded = Convert.FromBase64String(id);
            Guid decId = new Guid(System.Text.Encoding.UTF8.GetString(encoded));
            _logger.LogInformation($"User {User.Identity.Name} viewed comment for assignment id= {decId}! at date {DateTime.Now} with ip address {HttpContext.Connection.RemoteIpAddress}");
            return View(_commentsService.GetCommentsByAssignmentId(decId));
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
            if (User.Identity.Name != null)
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
                    _logger.LogInformation($"User {User.Identity.Name} created a comment for assignment {model.StudentAssignment.Id}! at date {DateTime.Now} with ip address {HttpContext.Connection.RemoteIpAddress}");
                    var encoded = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(HttpContext.Session.GetString(SessionKeyName)));
                    return RedirectToAction("Index", new { id = encoded });
                }
                catch
                {
                    TempData["error"] = ("Error occured Oooopppsss! We will look into it!");
                    _logger.LogError($"User {User.Identity.Name} tried to create a comment on assignment {model.StudentAssignment.Id}! at date {DateTime.Now} with ip address {HttpContext.Connection.RemoteIpAddress}");
                    return RedirectToAction("Error", "Home");
                }
            }
            else
            {
                TempData["error"] = ("Error occured Oooopppsss! We will look into it!");
                _logger.LogError($"User Anon tried to create a comment on assignment! at date {DateTime.Now} with ip address {HttpContext.Connection.RemoteIpAddress}");
                return RedirectToAction("Error", "Home");
            }
            
        }

    }
}
