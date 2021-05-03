using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Cryptography_SWD62B;
namespace WebApplication.Controllers
{
    public class StudentsAssignmentsController : Controller
    {
        const string SessionKeyName = "_Id";
        string password = "Pa$$w0rd?MY_";
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

        [Authorize(Roles = "Student,Teacher")]
        public ActionResult Index()
        {
            string email = User.Identity.Name;
            var student = _studentsService.GetStudentByEmail(email);
            var assignments = _studentAssignmentsService.GetStudentAssignmentById(student.Id);
            return View(assignments);
        }
        [Authorize(Roles = "Student,Teacher")]
        public ActionResult Details(String id)
        {
            byte[] encoded = Convert.FromBase64String(id);
            Guid decId = new Guid(System.Text.Encoding.UTF8.GetString(encoded));

            HttpContext.Session.SetString(SessionKeyName, decId.ToString());
            var assignment = _studentAssignmentsService.GetStudentAssignment(decId);
            return View(assignment);
        }

        [Authorize(Roles = "Student,Teacher")]
        public ActionResult Submit(String id)
        {
            byte[] encoded = Convert.FromBase64String(id);
            Guid decId = new Guid(System.Text.Encoding.UTF8.GetString(encoded));
            return View(_studentAssignmentsService.GetStudentAssignment(decId));
        }
        private bool checkPdf(Stream stream, String path)
        {
            bool isValid = true;
            IList<StudentAssignmentViewModel> files = _studentAssignmentsService.GetStudentAssignments().Where(f => f.File != path).ToList();
            foreach (StudentAssignmentViewModel fileName in files)
            {
                if (fileName.File != null)
                {
                    stream.Position = 0;
                    var filePath = _environment.ContentRootPath + fileName.File;
                    byte[] fileLocalBytes = System.IO.File.ReadAllBytes(filePath);
                    int len = fileLocalBytes.Length;
                    int counter = 0;

                    foreach (var b in fileLocalBytes)
                    {
                        if (stream.Length >= stream.Position)
                        {
                            if (Byte.Parse(stream.ReadByte().ToString()) == b)
                            {
                                counter++;
                            }
                            else
                            {
                                break;
                            }
                        }
                        if (counter == stream.Length)
                        {
                            isValid = !isValid;

                        }
                    }
                }
            }
            return isValid;
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
                if (deadline.Date < now)
                {
                    TempData["warning"] = "Assignment was not submitted Deadline date was exceeded !";
                }
                else
                {
                    bool isValid = false;
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
                                using (var stream = file.OpenReadStream())
                                {
                                    stream.Position = 0;
                                    //check if it is a genuine pdf
                                    int byte1 = stream.ReadByte();
                                    int byte2 = stream.ReadByte();
                                    //check for pdf
                                    if (byte1 == 37 && byte2 == 80)
                                    {
                                        //get all the submitted files from the db
                                        isValid = true;
                                    }
                                    else
                                    {
                                        TempData["warning"] = "Assignment needs to be .pdf !";
                                        return View();
                                    }
                                    stream.Position = 0;
                                }
                                if (isValid)
                                {

                                    string newFilename = Guid.NewGuid() + extension;
                                    newFilenameWithAbsolutePath = _environment.ContentRootPath + @"\Assignments\" + newFilename;
                                    string signiture = "";
                                    using (var stream = System.IO.File.Create(newFilenameWithAbsolutePath))
                                    {
                                        //encrypt file
                                        //stage 1 
                                        //encrypt using symetric key 
                                        MemoryStream fileMs = new MemoryStream();
                                        file.CopyTo(fileMs);
                                        Byte[] fileByte = fileMs.ToArray();
                                        Byte[] stage1 = CryptographicHelpers.SymmetricEncrypt(fileByte);
                                        //stage 2
                                        //encrypt using asymetric key

                                        file.CopyTo(stream);
                                        stream.Position = 0;
                                        //sign the document 
                                        RSA rsa = RSA.Create();
                                        MemoryStream ms = new MemoryStream();
                                        stream.CopyTo(ms);
                                        
                                        Byte[] objectArr = ms.ToArray();

                                        var keyPair = CryptographicHelpers.GenerateAsymmetricKeys();
                                        var publicKey = keyPair.Item1;
                                        var privateKey = keyPair.Item2;
                                        data.PublicKey = publicKey;
                                        signiture = CryptographicHelpers.CreateSigniture(objectArr, rsa, privateKey);
                                       
                                    }
                                    data.Signiture = signiture;
                                    data.File = @"\Assignments\" + newFilename;
                                    
                                }

                            }
                        }
                    }
                    if (isValid)
                    {
                        _studentAssignmentsService.SubmitAssignment(data.File, data.Signiture,data.PublicKey, data.Id);
                        TempData["feedback"] = "Assignment was submitted successfully";
                    }
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
        public ActionResult Download(String id)
        {
            byte[] encoded = Convert.FromBase64String(id);
            Guid decId = new Guid(System.Text.Encoding.UTF8.GetString(encoded));
            //get assignment by id
            var assignment = _studentAssignmentsService.GetStudentAssignment(decId);

            string path = assignment.File;

            string fileName = assignment.Assignment.Title + "/" + assignment.Student.FirstName + " " + assignment.Student.LastName;


            var filePath = _environment.ContentRootPath + path;
            byte[] fileLocalBytes = System.IO.File.ReadAllBytes(filePath);

            Stream localFile = new System.IO.MemoryStream(fileLocalBytes);

            using (localFile)
            {
                RSA rsa = RSA.Create();
                var keyPair = CryptographicHelpers.GenerateAsymmetricKeys();
               // var publicKey = keyPair.Item1;
                var privateKey = keyPair.Item2;
                var publicKey = assignment.PublicKey;
                bool isValid = CryptographicHelpers.VerifySigniture(fileLocalBytes, assignment.Signiture, rsa, publicKey);
                isValid = checkPdf(localFile, path);
                if (!isValid)
                {
                    TempData["warning"] = "Assignment is already uploaded !";
                }
                else
                {
                    var net = new System.Net.WebClient();
                    var data = net.DownloadData(filePath);
                    var ctnt = new System.IO.MemoryStream(data);
                    var type = "application/pdf";
                    var file = $"{fileName}.pdf";

                    return File(ctnt, type, file);
                }
            }
            return RedirectToAction("Details", new { id = id.ToString() });
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult GetAllSubmittedAssignments()
        {
            return View(_studentAssignmentsService.GetStudentAssignments());
        }
    }
}
