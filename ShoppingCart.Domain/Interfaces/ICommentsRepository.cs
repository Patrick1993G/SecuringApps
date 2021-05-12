using ShoppingCart.Domain.Models;
using System;
using System.Linq;

namespace ShoppingCart.Domain.Interfaces
{
    public interface ICommentsRepository
    {
        Comment GetComment(Guid id);
        IQueryable<Comment> GetComments();
        Guid AddComment(Comment comment);

    }
}
