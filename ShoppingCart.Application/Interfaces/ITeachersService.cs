using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface ITeachersService
    {
        Guid AddTeacher(TeacherViewModel t);
        IQueryable<TeacherViewModel> GetTeachers();
        TeacherViewModel getTeacherByEmail(String email);
    }
}
