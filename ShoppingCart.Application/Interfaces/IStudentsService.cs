using ShoppingCart.Application.ViewModels;
using System;
using System.Linq;

namespace ShoppingCart.Application.Interfaces
{
    public interface IStudentsService
    {
        IQueryable<StudentViewModel> GetStudents();
        IQueryable<StudentViewModel> GetStudentsByTeacherId(Guid id);
        Guid AddStudent(StudentViewModel studentViewModel);
        StudentViewModel GetStudentByEmail(string email);
    }
}
