using Microsoft.EntityFrameworkCore;
using UKParliament.CodeTest.Data.Entities;

namespace UKParliament.CodeTest.Data;

public class PersonManagerContext : DbContext
{
    public PersonManagerContext(DbContextOptions<PersonManagerContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Department>().HasData(
            new Department { Id = 1, Name = "Sales" },
            new Department { Id = 2, Name = "Marketing" },
            new Department { Id = 3, Name = "Finance" },
            new Department { Id = 4, Name = "HR" });

        modelBuilder.Entity<Person>().HasData(
            new Person { Id = 1, FirstName = "John", LastName = "Smith", DateOfBirth = new DateTime(1980, 1, 1), DepartmentId = 1, Email="John.Smith@test.com" },
            new Person { Id = 2, FirstName = "Jane", LastName = "Doe", DateOfBirth = new DateTime(1985, 2, 2), DepartmentId = 2, Email = "Jane.Doe@test.com" },
            new Person { Id = 3, FirstName = "James", LastName = "Johnson", DateOfBirth = new DateTime(1990, 3, 3), DepartmentId = 3, Email = "James.Johnsonh@test.com" },
            new Person { Id = 4, FirstName = "Jill", LastName = "Brown", DateOfBirth = new DateTime(1995, 4, 4), DepartmentId = 4 , Email = "Jill.Brown@test.com" });
    }

    public DbSet<Person> People { get; set; }

    public DbSet<Department> Departments { get; set; }
}