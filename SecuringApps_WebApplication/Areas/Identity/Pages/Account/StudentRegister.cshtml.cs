using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using PasswordGenerator;
using SecuringApps_WebApplication.Data;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;

namespace SecuringApps_WebApplication.Areas.Identity.Pages.Account
{
    public class StudentRegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<StudentRegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly ITeachersService _teacherService;
        private readonly IStudentsService _studentService;
        public StudentRegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<StudentRegisterModel> logger,
            IEmailSender emailSender,
            ITeachersService teachersService,
            IStudentsService studentsService)
        {
            _teacherService = teachersService;
            _studentService = studentsService;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "Name")]
            public string Name { get; set; }

            [Required]
            [DataType(DataType.Text)]
            [Display(Name = "LastName")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            public string Password { get; set; }

            [Required]
            public string Address { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }
        public static void SendEmail(string server, int port, string emailTo,string _subject,string _message)
        {
            string to = emailTo;
            string from = "patrickgrech6@outlook.com";
            string subject = _subject;
            string body = _message;
            // SMTP Client
            SmtpClient smtpClient = new SmtpClient(server, port);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(from, "123awsx?");

            // Mail Message
            MailMessage message = new MailMessage(from, to, subject, body);

            // Send Mail
            smtpClient.Send(message);
        }
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email,EmailConfirmed = true };
                var pwd = new Password();
                var passResult = pwd.Next();
                Input.Password = passResult;
                var result = await _userManager.CreateAsync(user, Input.Password);
               
                if (result.Succeeded)
                {
                    //add a role
                    var roleResult = await _userManager.AddToRoleAsync(user, "Student");
                    if (!roleResult.Succeeded)
                    {
                        ModelState.AddModelError("",
                        "Error while allocating role!");
                    }

                    _logger.LogInformation("User created a new account with password.");

                    //add student to student's table
                    StudentViewModel student = new StudentViewModel();
                    TeacherViewModel teacher = _teacherService.getTeacherByEmail(User.Identity.Name);

                    student.Id = Guid.Parse(user.Id);
                    student.Email = Input.Email;
                    student.FirstName = Input.Name;
                    student.LastName = Input.LastName;
                    student.Password = user.PasswordHash;
                    student.Teacher = teacher;

                    //add to db
                    _studentService.AddStudent(student);

                    //send details to student
                    string message = "Your email = "+ Input.Email + " and password= "+Input.Password;
                    SendEmail("smtp.live.com", 587, student.Email, "Student account details", message);

                    _logger.LogInformation($"User {User.Identity.Name} with ip address { HttpContext.Connection.RemoteIpAddress}" +
                        $"Created Student account for {Input.Email} at {DateTime.Now}");
        
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                   
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}
