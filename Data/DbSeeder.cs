using LeaveApplicationApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace LeaveApplicationApp.Data
{
    public static class DbSeeder
    {
        public static void SeedData(ApplicationDbContext dbContext)
        {
            if (!dbContext.Departments.Any())
            {
                SeedDepartments(dbContext);
            }

            if (!dbContext.Employees.Any())
            {
                SeedEmployees(dbContext);
            }

            if (!dbContext.LeaveApplications.Any())
            {
                SeedLeaveApplications(dbContext);
            }
        }

        private static void SeedDepartments(ApplicationDbContext dbContext)
        {
            var departments = new[]
            {
                new Department { DepartmentName = "Administration" },
                new Department { DepartmentName = "Sales" },
                new Department { DepartmentName = "IT" },
                new Department { DepartmentName = "Economy" },
                new Department { DepartmentName = "Human Resources" }
            };

            dbContext.Departments.AddRange(departments);
            dbContext.SaveChanges();
        }

        private static void SeedEmployees(ApplicationDbContext dbContext)
        {
            var random = new Random();
            var departments = dbContext.Departments.ToList();

            for (int i = 0; i < 30; i++) // Create 30 employees
            {
                var firstName = $"Employee{i + 1}";
                var lastName = $"Lastname{i + 1}";
                var birthDate = DateTime.Now.AddYears(-30).AddDays(-random.Next(365 * 30));
                var department = departments[random.Next(departments.Count)];

                var employee = new Employee
                {
                    FirstName = firstName,
                    LastName = lastName,
                    BirthDate = birthDate,
                    FKDepartmentId = department.DepartmentId
                };

                dbContext.Employees.Add(employee);
            }

            dbContext.SaveChanges();
        }

        private static void SeedLeaveApplications(ApplicationDbContext dbContext)
        {
            var random = new Random();
            var employees = dbContext.Employees.ToList();

            for (int i = 0; i < 60; i++) // Create 60 leave applications
            {
                var employee = employees[random.Next(employees.Count)]; // Randomly select an employee

                var startDate = DateTime.Now.AddYears(-1).AddMonths(-random.Next(12)).AddDays(-random.Next(30));
                var endDate = startDate.AddDays(random.Next(5, 15));
                var applicationTime = startDate.AddMonths(random.Next(12)); // Randomize application time within the last 12 months

                var leaveApplication = new LeaveApplication
                {
                    WorkLeaveType = (LeaveType)random.Next(0, Enum.GetNames(typeof(LeaveType)).Length),
                    ApplicationTime = applicationTime,
                    StartDate = startDate,
                    EndDate = endDate,
                    FkEmployeeId = employee.EmployeeId
                };

                dbContext.LeaveApplications.Add(leaveApplication);
            }

            dbContext.SaveChanges();
        }
    }
}