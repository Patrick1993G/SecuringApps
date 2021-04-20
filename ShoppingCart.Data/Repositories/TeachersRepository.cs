using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShoppingCart.Data.Repositories
{
    public class TeachersRepository : ITeachersRepository
    {
        ShoppingCartDbContext _context;
        public TeachersRepository(ShoppingCartDbContext context)
        {
            _context = context;

        }

        public Guid AddTeacher(Teacher t)
        {
            _context.Teachers.Add(t);
            _context.SaveChanges();
            return t.Id;
        }

        public IQueryable<Teacher> GetTeachers()
        {
            return _context.Teachers;
        }
    }
}
