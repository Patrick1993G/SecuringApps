using ShoppingCart.Application.ViewModels;
using System;
using System.Linq;

namespace ShoppingCart.Application.Interfaces
{
    public interface IStudentAssignmentsService
    {
        Guid AddStudentAssignment(StudentAssignmentViewModel studentAssignmentViewModel);
        StudentAssignmentViewModel GetStudentAssignment(Guid id);

        IQueryable<StudentAssignmentViewModel> GetStudentAssignments();
        bool SubmitAssignment(string filePath,string signiture, string publicKey , string privateKey, byte[] key, byte[] iv, Guid id);
        IQueryable<StudentAssignmentViewModel> GetStudentAssignmentById(Guid id);
    }
}
