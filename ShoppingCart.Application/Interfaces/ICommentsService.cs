using ShoppingCart.Application.ViewModels;
using System;
using System.Linq;

namespace ShoppingCart.Application.Interfaces
{
    public interface ICommentsService
    {
        IQueryable<CommentViewModel> GetComments();
        CommentViewModel GetComment(Guid id);
        IQueryable<CommentViewModel> GetCommentsByAssignmentId(Guid id);
        void AddComment(CommentViewModel commentViewModel);
    }
}
