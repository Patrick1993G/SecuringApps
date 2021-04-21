using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Services
{
    public class StudentsService : IStudentsService
    {
        public Guid AddStudent(StudentViewModel s)
        {
            throw new NotImplementedException();
        }

        public IQueryable<StudentViewModel> GetStudents()
        {
            throw new NotImplementedException();
        }
    }
}
