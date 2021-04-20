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
        public Guid AddStudentAssignment(StudentAssignment c)
        {
            throw new NotImplementedException();
        }

        public StudentAssignment GetStudentAssignment(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<StudentAssignment> GetStudentAssignmentById(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<StudentAssignment> GetStudentAssignments()
        {
            throw new NotImplementedException();
        }

        public bool SubmitAssignment(string filePath)
        {
            throw new NotImplementedException();
        }
    }
}
