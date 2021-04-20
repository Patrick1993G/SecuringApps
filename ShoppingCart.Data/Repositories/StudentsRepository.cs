using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class StudentsRepository : IStudentsRepository
    {
        ShoppingCartDbContext _context;
        public StudentsRepository(ShoppingCartDbContext context)
        {
            _context = context;

        }
        public Guid AddStudent(Student s)
        {
            throw new NotImplementedException();
        }

        public IQueryable<StudentAssignment> GetStudents()
        {
            throw new NotImplementedException();
        }
    }
}
