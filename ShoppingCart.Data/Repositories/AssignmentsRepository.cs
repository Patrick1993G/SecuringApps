﻿using ShoppingCart.Data.Context;
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
            throw new NotImplementedException();
        }

        public Assignment GetAssignment(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Assignment> GetAssignments()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Assignment> GetAssignmentsByStudentId(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Assignment> GetAssignmentsByTeacherId(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
