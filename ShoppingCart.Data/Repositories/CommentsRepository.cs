using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Linq;

namespace ShoppingCart.Data.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        ShoppingCartDbContext _context;
        public CommentsRepository(ShoppingCartDbContext context)
        {
            _context = context;

        }

        public Guid AddComment(Comment comment)
        {
            comment.Student = null;
            comment.Teacher = null;
            comment.StudentAssignment = null;
            _context.Comments.Add(comment);
            _context.SaveChanges();
            return comment.Id;
        }

        public Comment GetComment(Guid id)
        {
            return _context.Comments.SingleOrDefault(c => c.Id == id);
        }

        public IQueryable<Comment> GetComments()
        {
            return _context.Comments;
        }

    }
}