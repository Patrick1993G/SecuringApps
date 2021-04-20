using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class AssignmentsRepository : IAssignmentsRepository
    {
        ShoppingCartDbContext _context;
        public AssignmentsRepository(ShoppingCartDbContext context)
        {
            _context = context;

        }
        public Guid AddAssignment(Assignment a)
        {
            _context.Assignments.Add(a);
            _context.SaveChanges();
            return a.Id;
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
