using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Services
{
    public class AssignmentsService : IAssignmentsService
    {
        public void AddAssignment(AssignmentViewModel model)
        {
            throw new NotImplementedException();
        }

        public void DeleteAssignment(Guid id)
        {
            throw new NotImplementedException();
        }

        public AssignmentViewModel GetAssignment(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<AssignmentViewModel> GetAssignments()
        {
            throw new NotImplementedException();
        }

        public IQueryable<AssignmentViewModel> GetAssignments(string keyword)
        {
            throw new NotImplementedException();
        }
    }
}
