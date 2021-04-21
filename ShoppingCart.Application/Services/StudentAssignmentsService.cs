using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Services
{
    public class StudentAssignmentsService : IStudentAssignmentsService
    {
        public Guid AddStudentAssignment(StudentAssignmentViewModel s)
        {
            throw new NotImplementedException();
        }

        public StudentAssignmentViewModel GetStudentAssignment(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<StudentAssignmentViewModel> GetStudentAssignmentById(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<StudentAssignmentViewModel> GetStudentAssignments()
        {
            throw new NotImplementedException();
        }

        public bool SubmitAssignment(string filePath, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
