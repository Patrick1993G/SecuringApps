using ShoppingCart.Application.ViewModels;
using System;
using System.Linq;

namespace ShoppingCart.Application.Interfaces
{
    public interface ITeachersService
    {
        Guid AddTeacher(TeacherViewModel teacherViewModel);
        IQueryable<TeacherViewModel> GetTeachers();
        TeacherViewModel getTeacherByEmail(String email);
    }
}
