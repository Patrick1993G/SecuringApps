using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Services
{
    public class TeachersService : ITeachersService
    {
        public Guid AddTeacher(TeacherViewModel t)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TeacherViewModel> GetTeachers()
        {
            throw new NotImplementedException();
        }
    }
}
