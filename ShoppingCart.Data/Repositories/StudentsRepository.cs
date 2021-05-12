using ShoppingCart.Data.Context;
using ShoppingCart.Domain.Interfaces;
using ShoppingCart.Domain.Models;
using System;
using System.Linq;

namespace ShoppingCart.Data.Repositories
{
    public class StudentsRepository : IStudentsRepository
    {
        ShoppingCartDbContext _context;
        public StudentsRepository(ShoppingCartDbContext context)
        {
            _context = context;

        }
        public Guid AddStudent(Student student)
        {
            student.Teacher = null;
            _context.Students.Add(student);
            _context.SaveChanges();
            return student.Id;
        }

        public Student GetStudentByEmail(string email)
        {
            return _context.Students.SingleOrDefault(x => x.Email == email);
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
