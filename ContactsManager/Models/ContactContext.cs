using ContactsManager.ContactsApp.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ContactsManager.ContactsApp
{
    /// <summary>
    /// 
    /// </summary>
    public class ContactContext : DbContext
    {
        // Pass options
        public ContactContext(DbContextOptions<ContactContext> options) : base(options) 
        {
            Database.EnsureCreated();
        }

        // List of contacts
        public DbSet<Models.Contact> Contacts { get; set; }

        /// <summary>
        /// Init Contacts table with test data
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
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
