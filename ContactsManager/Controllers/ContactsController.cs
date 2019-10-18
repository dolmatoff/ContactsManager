using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ContactsManager.ContactsApp;
using ContactsManager.ContactsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;

namespace ContactsManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ContactContext _dbContext;

        private readonly ILogger<ContactsController> _logger;

        public ContactsController(ILogger<ContactsController> logger, ContactContext cCtx)
        {
            _logger = logger;
            _dbContext = cCtx;
        }

        // GET api/contacts
        [HttpGet]
        public async Task<ActionResult<List<Contact>>> Get()
        {
            return await _dbContext.Contacts.ToListAsync();
        }

        // GET api/contacts/2
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> Get(int id)
        {
            return await _dbContext.Contacts.FindAsync(id);
        }

        // POST api/contacts
        [HttpPost]
        public async Task Post(Contact model)
        {
            _dbContext.Contacts.Add(model);

            await _dbContext.SaveChangesAsync();
        }

        // PUT api/contacts/2
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, Contact model)
        {
            var isExist = await _dbContext.Contacts.AnyAsync(f => f.Id == id);
            if (!isExist)
            {
                return NotFound();
            }

            _dbContext.Contacts.Update(model);

            await _dbContext.SaveChangesAsync();

            return Ok();

        }

        // DELETE api/contacts/2
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var entity = await _dbContext.Contacts.FindAsync(id);

            _dbContext.Contacts.Remove(entity);

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
