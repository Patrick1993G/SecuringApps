using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Domain.Interfaces
{
    public interface ICommentsRepository
    {
        StudentAssignment GetComment(Guid id);
        IQueryable<StudentAssignment> GetComments();
        IQueryable<StudentAssignment> GetCommentsByAssignmentId(Guid id);
        Guid AddComment(StudentAssignment c);

    }
}
