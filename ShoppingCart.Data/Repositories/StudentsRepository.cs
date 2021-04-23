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
            s.Teacher = null;
            _context.Students.Add(s);
            _context.SaveChanges();
            return s.Id;
        }

        public IQueryable<Student> GetStudents()
        {
           return _context.Students;
        }

        public IQueryable<Student> GetStudentsByTeacher(Guid id)
        {
            return _context.Students.Where(t => t.TeacherId == id);
        }
    }
}
