using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Services
{
    public class CommentsService : ICommentsService
    {
        public void AddComment(CommentViewModel model)
        {
            throw new NotImplementedException();
        }

        public CommentViewModel GetComment(Guid id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<CommentViewModel> GetComments()
        {
            throw new NotImplementedException();
        }

        public IQueryable<Comment> GetCommentsByAssignmentId(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
