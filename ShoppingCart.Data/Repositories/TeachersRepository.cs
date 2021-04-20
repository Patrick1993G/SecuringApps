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
        public Guid AddTeacher(Teacher t)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Teacher> GetTeachers()
        {
            throw new NotImplementedException();
        }
    }
}
