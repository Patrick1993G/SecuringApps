using ShoppingCart.Domain.Models;
using System;
using System.Linq;

namespace ShoppingCart.Domain.Interfaces
{
    public interface ITeachersRepository
    {
        IQueryable<Teacher> GetTeachers();

        Guid AddTeacher(Teacher teacher);
        Teacher GetTeacherByEmail(String email);
    }
}
