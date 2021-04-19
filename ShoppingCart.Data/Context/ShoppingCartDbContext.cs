using Microsoft.EntityFrameworkCore;
using ShoppingCart.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingCart.Data.Context
{
    public class ShoppingCartDbContext: DbContext
    {
        public ShoppingCartDbContext(DbContextOptions options): base(options)
        {
        }

        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentAssignment> StudentAssignments { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Assignment>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Comment>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Student>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<StudentAssignment>().Property(x => x.Id).HasDefaultValueSql("NEWID()");
            modelBuilder.Entity<Teacher>().Property(x => x.Id).HasDefaultValueSql("NEWID()");

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }


    }
}
