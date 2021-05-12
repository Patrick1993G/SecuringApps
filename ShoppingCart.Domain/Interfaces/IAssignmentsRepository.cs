using ShoppingCart.Domain.Models;
using System;
using System.Linq;

namespace ShoppingCart.Domain.Interfaces
{
    public interface IAssignmentsRepository
    {
        Assignment GetAssignment(Guid id);
        IQueryable<Assignment> GetAssignments();
        Guid AddAssignment(Assignment assignment);

    }
}
