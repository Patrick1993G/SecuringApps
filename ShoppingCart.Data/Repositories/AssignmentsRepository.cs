using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Linq;

namespace ShoppingCart.Data.Repositories
{
    public class AssignmentsRepository : IAssignmentsRepository
    {
        ShoppingCartDbContext _context;
        public AssignmentsRepository(ShoppingCartDbContext context)
        {
            _context = context;

        }
        public Guid AddAssignment(Assignment assignment)
        {
            assignment.Teacher = null;
            _context.Assignments.Add(assignment);
            _context.SaveChanges();
            return assignment.Id;
        }

        public Assignment GetAssignment(Guid id)
        {
            return _context.Assignments.SingleOrDefault(a => a.Id == id);

        }

        public IQueryable<Assignment> GetAssignments()
        {
            return _context.Assignments;
        }

    }
}
