using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        ShoppingCartDbContext _context;
        public CommentsRepository(ShoppingCartDbContext context)
        {
            _context = context;

        }
        public Guid AddComment(StudentAssignment c)
        {
            throw new NotImplementedException();
        }

        public StudentAssignment GetComment(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<StudentAssignment> GetComments()
        {
            throw new NotImplementedException();
        }

        public IQueryable<StudentAssignment> GetCommentsByAssignmentId(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
