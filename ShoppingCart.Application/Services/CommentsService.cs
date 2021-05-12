using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Services
{
    public class CommentsService : ICommentsService
    {
        private IMapper _mapper;
        private ICommentsRepository _commentsRepo;
        public CommentsService(ICommentsRepository commentsRepository, IMapper mapper)
        {
            _mapper = mapper;
            _commentsRepo = commentsRepository;
        }

        public void AddComment(CommentViewModel model)
        {
            var comment = _mapper.Map<Comment>(model);
            _commentsRepo.AddComment(comment);
        }

        public CommentViewModel GetComment(Guid id)
        {
            var comment = _commentsRepo.GetComment(id);
            var commentModel = _mapper.Map<CommentViewModel>(comment);
            return commentModel;
        }

        public IQueryable<CommentViewModel> GetComments()
        {
            var comments = _commentsRepo.GetComments().ProjectTo<CommentViewModel>(_mapper.ConfigurationProvider);
            return comments;
        }

        public IQueryable<CommentViewModel> GetCommentsByAssignmentId(Guid id)
        {
            var comments = _commentsRepo.GetComments().Where(a => a.StudentAssignment.Id == id).ProjectTo<CommentViewModel>(_mapper.ConfigurationProvider);
            return comments;
        }
    }
}
