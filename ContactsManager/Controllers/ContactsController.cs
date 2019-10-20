using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ContactsManager.Infrastructure;
using ContactsManager.Extentions;
using ContactsManager.Models;
using ContactsManager.Interfaces;

namespace ContactsManager.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ContactsController : ControllerBase
    {
        # region Fields
        //
        private readonly IAsyncRepository<Contact> _repo;
        //
        private readonly ILoggerManager _logger;
        # endregion

        public ContactsController(ILoggerManager logger, IAsyncRepository<Contact> cCtx)
        {
            _logger = logger;
            _repo = cCtx;
        }

        // GET api/v1/contacts
        [HttpGet]
        public async Task<ActionResult<List<Contact>>> Get(int page = 0, int pageSize = 5)
        {
            try
            {
                var contacts = await _repo.GetPaged(page, pageSize);
                return Ok(contacts);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Some error in the GetPaged method: {ex}");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET api/v1/contacts/2
        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> Get(int id)
        {
            try
            {
                var contact = await _repo.GetById(id);

                if (contact == null)
                {
                    _logger.LogError($"Contact with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned contact with id: {id}");
                    return Ok(contact);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // POST api/v1/contacts
        [HttpPost]
        public async Task<IActionResult> Post(Contact contact)
        {
            try
            {
                if (contact == null)
                {
                    _logger.LogError("Contact object sent from client is null.");
                    return BadRequest("Contact object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid Contact object sent from client.");
                    return BadRequest("Invalid Contact object");
                }

                await _repo.Add(contact);
                var added = await _repo.GetById(contact.Id);
                
                if(added == null)
                {
                    throw new Exception();
                }

                return Ok(added);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Add action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // PUT api/v2/contacts/2
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody]Contact contact)
        {
            try
            {
                if (contact == null)
                {
                    _logger.LogError("Contact object sent from client is null.");
                    return BadRequest("Contact object is null");
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid contact object sent from client.");
                    return BadRequest("Invalid contact object");
                }

                Contact contactToEdit = await _repo.GetById(id);
                if (contactToEdit == null)
                {
                    _logger.LogError($"Contact with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                contactToEdit.Map(contact);

                await _repo.Update(contactToEdit);
                return Ok(contactToEdit);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Update action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE api/v1/contacts/2
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var contact = await _repo.GetById(id);
                if (contact == null)
                {
                    _logger.LogError($"Contact with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                await _repo.Remove(contact);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Remove action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
