using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using PasswordGenerator;
using SecuringApps_WebApplication.Data;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Models;

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
                var user = new ApplicationUser { UserName = Input.Email, Email = Input.Email };
                var pwd = new Password();
                var passResult = pwd.Next();
                Input.Password = passResult;
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

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
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                    
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
