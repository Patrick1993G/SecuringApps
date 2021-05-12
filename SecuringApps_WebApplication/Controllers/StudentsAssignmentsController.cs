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
using Cryptography_SWD62B;
using WebApplication.ActionFilters;
using Microsoft.Extensions.Logging;

namespace WebApplication.Controllers
{
    public class StudentsAssignmentsController : Controller
    {
        const string SessionKeyName = "_Id";
        private readonly IStudentAssignmentsService _studentAssignmentsService;
        private readonly IStudentsService _studentsService;
        private IWebHostEnvironment _environment;
        private readonly ILogger<StudentsAssignmentsController> _logger;
        public StudentsAssignmentsController(ILogger<StudentsAssignmentsController> logger,IStudentsService studentsService, IStudentAssignmentsService studentAssignmentsService, IWebHostEnvironment environment)
        {
            _studentAssignmentsService = studentAssignmentsService;
            _environment = environment;
            _studentsService = studentsService;
            _logger = logger;
        }

        [Authorize(Roles = "Student,Teacher")]
        public ActionResult Index()
        {
            string email = User.Identity.Name;
            var student = _studentsService.GetStudentByEmail(email);
            var assignments = _studentAssignmentsService.GetStudentAssignmentById(student.Id);
            _logger.LogInformation($"User {User.Identity.Name} viewed Student {student.Email} assignment! at date {DateTime.Now} with ip address {HttpContext.Connection.RemoteIpAddress}");
            return View(assignments);
        }
        [Authorize(Roles = "Student,Teacher")]
        [ActionFilter]
        public ActionResult Details(String id)
        {
            byte[] encoded = Convert.FromBase64String(id);
            Guid decId = new Guid(System.Text.Encoding.UTF8.GetString(encoded));
            HttpContext.Session.SetString(SessionKeyName, decId.ToString());
            var assignment = _studentAssignmentsService.GetStudentAssignment(decId);
            _logger.LogInformation($"This User {User.Identity.Name} viewed Student assignment! at date {DateTime.Now} with ip address {HttpContext.Connection.RemoteIpAddress}");

            return View(assignment);
        }

        [Authorize(Roles = "Student,Teacher")]
        public ActionResult Submit(String id)
        {
            Guid decId =new Guid();
            if (id != null)
            {
                byte[] encoded = Convert.FromBase64String(id);
                decId = new Guid(System.Text.Encoding.UTF8.GetString(encoded));
              
            }
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
                    var filePath = _environment.ContentRootPath + fileName.File;
                    byte[] fileLocalBytes = System.IO.File.ReadAllBytes(filePath);
                    byte[] decryptedKey = CryptographicHelpers.AsymmetricDecrypt(fileName.Key, fileName.PrivateKey);
                    byte[] decryptedIv = CryptographicHelpers.AsymmetricDecrypt(fileName.Iv, fileName.PrivateKey);
                    Tuple<byte[], byte[]> decryptedKeys = new Tuple<byte[], byte[]>(decryptedKey, decryptedIv);
                    byte[] decryptedFile = CryptographicHelpers.SymmetricDecrypt(fileLocalBytes, decryptedKeys);

                    stream.Position = 0;
                    
                   
                    int len = decryptedFile.Length;
                    int counter = 0;

                    foreach (var b in decryptedFile)
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
                    _logger.LogInformation($"This User {User.Identity.Name} tried to submit after the deadline was exceeded! at date {DateTime.Now} with ip address {HttpContext.Connection.RemoteIpAddress}");
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
                                   
                                    using (var stream = System.IO.File.Create(newFilenameWithAbsolutePath))
                                    {
                                        //generate keys
                                        var keyPair = CryptographicHelpers.GenerateAsymmetricKeys();
                                        var publicKey = keyPair.Item1;
                                        var privateKey = keyPair.Item2;
                                        data.PublicKey = publicKey;
                                        data.PrivateKey = privateKey;
                                        file.CopyTo(stream);
                                        stream.Position = 0;

                                        //sign the document 
                                        string signiture = SignDocument(stream, privateKey);

                                        Tuple<byte[], byte[], byte[]> hybridEncryption = HybridEncrypt(file, publicKey);
                                        data.Signiture = signiture;
                                        data.Key = hybridEncryption.Item2;
                                        data.Iv = hybridEncryption.Item3;
                                        stream.Position = 0;
                                        stream.Write(hybridEncryption.Item1);
                                        stream.Position = 0;

                                    }
                                    data.File = @"\Assignments\" + newFilename;
                                }
                            }
                        }
                    }
                    if (isValid)
                    {
                        _studentAssignmentsService.SubmitAssignment(data.File, data.Signiture,data.PublicKey, data.PrivateKey, data.Key, data.Iv, data.Id);
                        TempData["feedback"] = "Assignment was submitted successfully";
                    }
                }
            }
            catch (Exception e)
            {
                TempData["warning"] = "Assignment was not submitted !" + e.Message;
                _logger.LogError($"Error occurred while trying to submit assignment User {User.Identity.Name} tried to submit assignment at {DateTime.Now} with ip address {HttpContext.Connection.RemoteIpAddress} error message = {e.Message}");

                TempData["error"] = ("Error occured Oooopppsss! We will look into it!");
                return RedirectToAction("Error", "Home");
            }
            return View(assignment);
        }

        private static string SignDocument(FileStream stream, string privateKey)
        {
            RSA rsa = RSA.Create();
            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);

            Byte[] objectArr = ms.ToArray();

            string signiture = CryptographicHelpers.CreateSigniture(objectArr, rsa, privateKey);
            return signiture;
        }

        private static Tuple<byte[], byte[], byte[]> HybridEncrypt(IFormFile file, string publicKey)
        {
            //encrypt file
            //stage 1 
            //encrypt using symetric key on file 
            MemoryStream fileMs = new MemoryStream();
            file.CopyTo(fileMs);
            Byte[] fileByte = fileMs.ToArray();
            //generate the keys
            Tuple<byte[], byte[]> _keyIVPair = CryptographicHelpers.GenerateKeys();
            byte[] k = _keyIVPair.Item1;
            byte[] iv = _keyIVPair.Item2;

            Byte[] encryptedFile = CryptographicHelpers.SymmetricEncrypt(fileByte, _keyIVPair);

            //stage 2
            //encrypt using asymetric key on signiture and key 
            byte[] encryptedKey = CryptographicHelpers.AsymetricEncrypt(k, publicKey);
            byte[] encryptedIv = CryptographicHelpers.AsymetricEncrypt(iv, publicKey);

            return new Tuple<byte[], byte[], byte[]>(encryptedFile, encryptedKey , encryptedIv);
        }

        [Authorize(Roles = "Student,Teacher")]
        [ActionFilter]
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

            
            //decrypt File
            byte[] decryptedKey = CryptographicHelpers.AsymmetricDecrypt(assignment.Key, assignment.PrivateKey);
            byte[] decryptedIv = CryptographicHelpers.AsymmetricDecrypt(assignment.Iv, assignment.PrivateKey);
            
            Tuple <byte[], byte[]> decryptedKeys = new Tuple<byte[], byte[]>(decryptedKey, decryptedIv);
            byte[] decryptedFile = CryptographicHelpers.SymmetricDecrypt(fileLocalBytes, decryptedKeys);
            Stream localFile = new System.IO.MemoryStream(decryptedFile);
            
            using (localFile)
            {
                RSA rsa = RSA.Create();
                //decrypt Signiture
                var publicKey = assignment.PublicKey;
                bool isValid = CryptographicHelpers.VerifySigniture(decryptedFile,assignment.Signiture, rsa, publicKey);
                
                isValid = checkPdf(localFile, path);
                if (!isValid)
                {
                    TempData["warning"] = "Assignment is already uploaded !";
                    _logger.LogInformation($" User {User.Identity.Name} tried to download an already uploaded assignment! at {DateTime.Now} with ip address {HttpContext.Connection.RemoteIpAddress}");
                }
                else
                {
                    var ctnt = new System.IO.MemoryStream(decryptedFile);
                    var type = "application/pdf";
                    var file = $"{fileName}.pdf";
                    var fileDownloaded = File(ctnt, type, file);
                    _logger.LogInformation($" User {User.Identity.Name} downloaded sucessfully the assignment! at {DateTime.Now} with ip address {HttpContext.Connection.RemoteIpAddress}");
                    return fileDownloaded;
                }
            }
            return RedirectToAction("Details", new { id = id.ToString() });
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult GetAllSubmittedAssignments()
        {
            _logger.LogInformation($" User {User.Identity.Name} viewed the submitted assignments! at {DateTime.Now} with ip address {HttpContext.Connection.RemoteIpAddress}");
            return View(_studentAssignmentsService.GetStudentAssignments());
        }
    }
}
