using ShoppingCart.Domain.Models;
using System;
using System.Linq;

namespace ShoppingCart.Domain.Interfaces
{
    public interface IStudentsRepository
    {
        IQueryable<Student> GetStudents();
        IQueryable<Student> GetStudentsByTeacher(Guid id);
        Guid AddStudent(Student student);

        Student GetStudentByEmail(string email);
    }
}
