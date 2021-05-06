using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Domain.Interfaces
{
    public interface IAssignmentsRepository
    {
        Assignment GetAssignment(Guid id);
        IQueryable<Assignment> GetAssignments();
        Guid AddAssignment(Assignment a);

    }
}
