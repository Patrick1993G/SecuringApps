using AutoMapper;
using AutoMapper.QueryableExtensions;
using ShoppingCart.Application.Interfaces;
using ShoppingCart.Application.ViewModels;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Application.Services
{
    public class TeachersService : ITeachersService
    {
        private IMapper _mapper;
        private ITeachersRepository _teachersRepo;
        public TeachersService(ITeachersRepository teachersRepository, IMapper mapper)
        {
            _mapper = mapper;
            _teachersRepo = teachersRepository;
        }

        public Guid AddTeacher(TeacherViewModel teacherViewModel)
        {
            var teacher = _mapper.Map<Teacher>(teacherViewModel);
            _teachersRepo.AddTeacher(teacher);
            return teacher.Id;
        }

        public TeacherViewModel getTeacherByEmail(string email)
        {
            return _mapper.Map<TeacherViewModel>(_teachersRepo.GetTeacherByEmail(email));
        }

        public IQueryable<TeacherViewModel> GetTeachers()
        {
            var teachers = _teachersRepo.GetTeachers().ProjectTo<TeacherViewModel>(_mapper.ConfigurationProvider);
            return teachers;
        }
    }
}
