﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebPocHub.Models;

namespace WebPocHu.dal
{
    public class WebPocHubDbContext : DbContext
    {
        public WebPocHubDbContext()
        {

        }
        public WebPocHubDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer("Data Source = localhost; Initial Catalog = UdemyWebApiDb; Integrated Security = true; TrustServerCertificate=True");
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee() { EmployeeId = 1, EmployeeName = "John Mark", Address = "East Wing, Z/101", City = "London", Country = "United Kingdom", Zipcode = "473837", Phone = "+044 73783783", Email = "john.mark@webpochub.com", Skillsets = "DBA", Avatar = "/images/john-mark.png" },
                new Employee() { EmployeeId = 2, EmployeeName = "Alisha C.", Address = "North Wing, Moon-01", City = "Mumbai", Country = "India", Zipcode = "367534", Phone = "+91 7865678645", Email = "alisha.c@webpochub.com", Skillsets = "People Management", Avatar = "/images/alisha-c.png" },
                new Employee() { EmployeeId = 3, EmployeeName = "Pravinkumar Dabade", Address = "Suncity, A8/404", City = "Pune", Country = "India", Zipcode = "411051", Phone = "+044 73783783", Email = "dabade.pravinkumar@webpochub.com", Skillsets = "Trainer & Consultant", Avatar = "/images/dabade-pravinkumar.png" }
            );
            modelBuilder.Entity<Role>().HasData(
                new Role() { RoleId = 1, RoleName = "Employee", RoleDescription = "Employee of WebPocHub Organization!" },
                new Role() { RoleId = 2, RoleName = "Hr", RoleDescription = "Hr of WebPocHub Organization!" }
            );
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Role> Roles { get; set;  }
        public DbSet<User> Users { get; set; }
    }
}
