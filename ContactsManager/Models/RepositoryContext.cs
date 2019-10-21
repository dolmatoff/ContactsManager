using ContactsManager.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace ContactsManager
{
    /// <summary>
    /// 
    /// </summary>
    public class RepositoryContext : DbContext
    {
        // Pass options
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Init Contacts table with test data
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            List<Career> careers = new List<Career>();

            Array.ForEach(new string[] {
                "Medicine",
                "Engineering",
                "Computer science",
                "Management",
                "Accounting",
                "Teaching",
                "Art",
                "Sales",
                "Driver",
                "Student"
            }, e => careers.Add(new Career { Name = e }));

            modelBuilder.Entity<Career>(options =>
            {
                options.HasData(careers);
            });

            modelBuilder.Entity<Contact>(options =>
            {
                options.HasData(
                    new Contact { Id=1, Name = "Cristiano", Surname = "Ramirez", Gender = "M", Birthdate = DateTime.Parse("1996-09-01"), Phone = "+79845673432", Career = "Medicine" },
                    new Contact { Id=2, Name = "Meredith",  Surname = "Alonso",  Gender = "M", Birthdate = DateTime.Parse("1983-09-01"), Phone = "+79800073432", Career = "Ingineering" },
                    new Contact { Id=3, Name = "Arturo",    Surname = "Amand",   Gender = "M", Birthdate = DateTime.Parse("1987-09-01"), Phone = "+79849574324", Career = "Management" }
                );
            });
        }
    }
}
