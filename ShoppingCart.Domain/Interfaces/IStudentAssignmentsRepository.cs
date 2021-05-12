using ShoppingCart.Domain.Models;
using System;
using System.Linq;

namespace ShoppingCart.Domain.Interfaces
{
    public interface IStudentAssignmentsRepository
    {
        StudentAssignment GetStudentAssignment(Guid id);
        IQueryable<StudentAssignment> GetStudentAssignments();
        Guid AddStudentAssignment(StudentAssignment studentAssignment);

        bool SubmitAssignment(String filePath,String signiture,String publicKey,String privateKey, byte[] Key, byte[] Iv, Guid id);
    }
}
