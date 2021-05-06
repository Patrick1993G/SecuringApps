using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Domain.Interfaces
{
    public interface IStudentsRepository
    {
        IQueryable<Student> GetStudents();
        IQueryable<Student> GetStudentsByTeacher(Guid id);
        Guid AddStudent(Student s);

        Student GetStudentByEmail(string email);
    }
}
