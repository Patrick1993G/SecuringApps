using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface IStudentAssignmentsService
    {
        Guid AddStudentAssignment(StudentAssignmentViewModel s);
        StudentAssignmentViewModel GetStudentAssignment(Guid id);

        IQueryable<StudentAssignmentViewModel> GetStudentAssignments();
        bool SubmitAssignment(string filePath,string signiture, string publicKey , Guid id);
        IQueryable<StudentAssignmentViewModel> GetStudentAssignmentById(Guid id);
    }
}
