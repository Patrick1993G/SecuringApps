using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class StudentAssignmentsRepository : IStudentAssignmentsRepository
    {
        ShoppingCartDbContext _context;
        public StudentAssignmentsRepository(ShoppingCartDbContext context)
        {
            _context = context;

        }
        public Guid AddStudentAssignment(StudentAssignment s)
        {
            _context.StudentAssignments.Add(s);
            _context.SaveChanges();
            return s.Id;
        }

        public StudentAssignment GetStudentAssignment(Guid id)
        {
            return _context.StudentAssignments.SingleOrDefault(s => s.Id == id);
        }

        public IQueryable<StudentAssignment> GetStudentAssignments()
        {
            return _context.StudentAssignments;
        }

        public bool SubmitAssignment(string filePath, Guid id)
        {
            var assignment = GetStudentAssignment(id);
            assignment.File = filePath;
            assignment.Submitted = !assignment.Submitted;
            _context.StudentAssignments.Update(assignment);
            _context.SaveChanges();
            return assignment.Submitted;
        }

    }
}
