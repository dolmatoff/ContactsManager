using ContactsManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactsManager.Extentions
{
    public static class ContactEx
    {
        public static void Map(this Contact dbContact, Contact contact)
        {
            dbContact.Name = contact.Name;
            dbContact.Surname = contact.Surname;
            dbContact.Gender = contact.Gender;
            dbContact.Phone = contact.Phone;
            dbContact.Career = contact.Career;
            dbContact.Birthdate = contact.Birthdate;
            dbContact.Comment = contact.Comment;
        }
    }
}
