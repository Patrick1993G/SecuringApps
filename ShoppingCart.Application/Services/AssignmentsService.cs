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
    public class AssignmentsService : IAssignmentsService
    {
        private IMapper _mapper;
        private IAssignmentsRepository _assignmentsRepo;
        public AssignmentsService(IAssignmentsRepository assignmentsRepository, IMapper mapper)
        {
            _mapper = mapper;
            _assignmentsRepo = assignmentsRepository;
        }

        public void AddAssignment(AssignmentViewModel model)
        {
            var assignment = _mapper.Map<Assignment>(model);
            _assignmentsRepo.AddAssignment(assignment);
        }

        public AssignmentViewModel GetAssignment(Guid id)
        {
            var assignment = _assignmentsRepo.GetAssignment(id);
            var assignmentModel = _mapper.Map<AssignmentViewModel>(assignment);
            return assignmentModel;
        }

        public IQueryable<AssignmentViewModel> GetAssignments()
        {
            var assignments = _assignmentsRepo.GetAssignments().ProjectTo<AssignmentViewModel>(_mapper.ConfigurationProvider);
            return assignments;
        }

        public IQueryable<AssignmentViewModel> GetAssignmentsByTeacherId(Guid id)
        {
            var assignments = _assignmentsRepo.GetAssignments().Where(t => t.Teacher.Id == id).ProjectTo<AssignmentViewModel>(_mapper.ConfigurationProvider);
            return assignments;
        }
    }
}
