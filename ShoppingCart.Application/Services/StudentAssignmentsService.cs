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
    public class StudentAssignmentsService : IStudentAssignmentsService
    {
        private IMapper _mapper;
        private IStudentAssignmentsRepository _studentAssignmentsRepo;
        public StudentAssignmentsService(IStudentAssignmentsRepository studentAssignmentsRepository, IMapper mapper)
        {
            _mapper = mapper;
            _studentAssignmentsRepo = studentAssignmentsRepository;
        }

        public Guid AddStudentAssignment(StudentAssignmentViewModel studentAssignmentViewModel)
        {
            var studentAssignment = _mapper.Map<StudentAssignment>(studentAssignmentViewModel);
            _studentAssignmentsRepo.AddStudentAssignment(studentAssignment);
            return studentAssignment.Id;
        }

        public StudentAssignmentViewModel GetStudentAssignment(Guid id)
        {
            var assignment = _studentAssignmentsRepo.GetStudentAssignment(id);
            var assignmentModel = _mapper.Map<StudentAssignmentViewModel>(assignment);
            return assignmentModel;
        }

        public IQueryable<StudentAssignmentViewModel> GetStudentAssignmentById(Guid id)
        {
            var assignments = _studentAssignmentsRepo.GetStudentAssignments().Where(s => s.Student.Id == id).ProjectTo<StudentAssignmentViewModel>(_mapper.ConfigurationProvider); ;
            return assignments;
        }

        public IQueryable<StudentAssignmentViewModel> GetStudentAssignments()
        {
            var assignments = _studentAssignmentsRepo.GetStudentAssignments().ProjectTo<StudentAssignmentViewModel>(_mapper.ConfigurationProvider);
            return assignments;
        }

        public bool SubmitAssignment(string filePath,string signiture,string publicKey, String privateKey, byte[] Key, byte[] Iv, Guid id)
        {
            _studentAssignmentsRepo.SubmitAssignment(filePath, signiture, publicKey, privateKey, Key, Iv, id);
            return true;
        }
    }
}
