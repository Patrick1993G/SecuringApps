using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Linq;

namespace ShoppingCart.Data.Repositories
{
    public class TeachersRepository : ITeachersRepository
    {
        ShoppingCartDbContext _context;
        public TeachersRepository(ShoppingCartDbContext context)
        {
            _context = context;

        }

        public Guid AddTeacher(Teacher teacher)
        {
            _context.Teachers.Add(teacher);
            _context.SaveChanges();
            return teacher.Id;
        }

        public Teacher GetTeacherByEmail(string email)
        {
           return _context.Teachers.SingleOrDefault(x => x.Email == email);
        }

        public IQueryable<Teacher> GetTeachers()
        {
            return _context.Teachers;
        }
    }
}
