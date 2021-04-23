using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface IStudentsService
    {
        IQueryable<StudentViewModel> GetStudents();
        IQueryable<StudentViewModel> GetStudentsByTeacherId(Guid id);
        Guid AddStudent(StudentViewModel s);
    }
}
