using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Linq;

namespace ShoppingCart.Data.Repositories
{
    public class StudentAssignmentsRepository : IStudentAssignmentsRepository
    {
        ShoppingCartDbContext _context;
        public StudentAssignmentsRepository(ShoppingCartDbContext context)
        {
            _context = context;

        }
        public Guid AddStudentAssignment(StudentAssignment studentAssignment)
        {
            studentAssignment.Assignment = null;
            studentAssignment.Student = null;
            _context.StudentAssignments.Add(studentAssignment);
            _context.SaveChanges();
            return studentAssignment.Id;
        }

        public StudentAssignment GetStudentAssignment(Guid id)
        {
            return _context.StudentAssignments.SingleOrDefault(s => s.Id == id);
        }

        public IQueryable<StudentAssignment> GetStudentAssignments()
        {
            return _context.StudentAssignments;
        }

        public bool SubmitAssignment(string filePath,string signiture,string publicKey, String privateKey, byte[] Key, byte[] Iv, Guid id)
        {

            var assignment = GetStudentAssignment(id);
            assignment.Iv = Iv;
            assignment.Key = Key;
            assignment.PrivateKey = privateKey;
            assignment.Signiture = signiture;
            assignment.PublicKey = publicKey;
            assignment.File = filePath;
            assignment.Submitted = !assignment.Submitted;
            _context.StudentAssignments.Update(assignment);
            _context.SaveChanges();
            return assignment.Submitted;
        }

    }
}
