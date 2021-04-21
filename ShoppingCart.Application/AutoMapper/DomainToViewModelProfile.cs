﻿using AutoMapper;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Application.AutoMapper
{
    public class DomainToViewModelProfile: Profile
    {
        public DomainToViewModelProfile()
        {
            CreateMap<Assignment, AssignmentViewModel>();
            CreateMap<Comment, CommentViewModel>();
            CreateMap<StudentAssignment, StudentAssignmentViewModel>();
            CreateMap<Student, StudentViewModel>();
            CreateMap<Teacher, TeacherViewModel>();
            //Product class was used to model the database
            //ProductViewModel class was used to pass on the data to/from the Presentation project/layer
        }

    }
}
