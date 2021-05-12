using ShoppingCart.Application.ViewModels;
using System;
using System.Linq;

namespace ShoppingCart.Application.Interfaces
{
    public interface IAssignmentsService
    {
        IQueryable<AssignmentViewModel> GetAssignments();

        IQueryable<AssignmentViewModel> GetAssignmentsByTeacherId(Guid id);
        AssignmentViewModel GetAssignment(Guid id);

        Guid AddAssignment(AssignmentViewModel assignmentViewModel);


    }
}
