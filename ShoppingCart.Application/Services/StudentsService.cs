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
    public class StudentsService : IStudentsService
    {
        private IMapper _mapper;
        private IStudentsRepository _studentsRepo;
        public StudentsService(IStudentsRepository studentsRepository, IMapper mapper)
        {
            _mapper = mapper;
            _studentsRepo = studentsRepository;
        }
        public Guid AddStudent(StudentViewModel s)
        {
            var student = _mapper.Map<Student>(s);
            _studentsRepo.AddStudent(student);
            return student.Id;
        }

        public IQueryable<StudentViewModel> GetStudents()
        {
            var students = _studentsRepo.GetStudents().ProjectTo<StudentViewModel>(_mapper.ConfigurationProvider);
            return students;
        }
    }
}
