using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Interfaces
{
    public interface IAssignmentsService
    {
        IQueryable<AssignmentViewModel> GetAssignments();
        IQueryable<AssignmentViewModel> GetAssignments(string keyword);

        AssignmentViewModel GetAssignment(Guid id);

        void AddAssignment(AssignmentViewModel model);


        void DeleteAssignment(Guid id);

    }
}
