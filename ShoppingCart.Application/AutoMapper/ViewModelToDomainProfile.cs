using AutoMapper;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Models;

namespace ShoppingCart.Application.AutoMapper
{
    public class ViewModelToDomainProfile:Profile
    {
        public ViewModelToDomainProfile()
        {
            CreateMap<AssignmentViewModel, Assignment>();
            CreateMap<CommentViewModel, Comment>();
            CreateMap<StudentAssignmentViewModel, StudentAssignment>();
            CreateMap<StudentViewModel, Student> ();
            CreateMap<TeacherViewModel, Teacher>();
        }
    }
}
