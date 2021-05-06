using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Domain.Interfaces
{
    public interface ITeachersRepository
    {
        IQueryable<Teacher> GetTeachers();

        Guid AddTeacher(Teacher t);
        Teacher GetTeacherByEmail(String email);
    }
}
