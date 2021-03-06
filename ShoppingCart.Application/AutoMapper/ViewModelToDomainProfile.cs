using AutoMapper;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

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
