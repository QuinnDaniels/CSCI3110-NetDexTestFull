using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Models.ViewModels;
using NetDexTest_01.Services;
using Microsoft.AspNetCore.Authorization;

namespace NetDexTest_01.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public partial class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPersonRepository _personRepo;
        private readonly ILogger<PeopleController> _logger;
        public PeopleController(ILogger<PeopleController> logger, ApplicationDbContext context)
        {
            _context = context;
            _logger = logger;
    }

        // GET: api/People
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            return Ok(await _context.Person.ToListAsync());
        }

        // GET: api/People/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPeople(int id)
        {
            //var person = _context.Person.FirstOrDefault(a => a.Id.Equals(id));
            var person = await _context.Person.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        // POST: api/People
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Person>> InsertPerson(Person person)
        {
            //person.Id = Guid.NewGuid();
            _context.Person.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPeople), new { id = person.Id }, person);
        }

        // POST: api/People
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Person>> InsertPerson(NewPersonVM person)
        {
            //person.Id = Guid.NewGuid();


            Person inPerson = await _personRepo.CreatePersonAsync(person);

            //_context.Person.Add(inPerson);
            //await _context.SaveChangesAsync();

            if (inPerson != null) 
            {
            return CreatedAtAction(nameof(GetPeople), new { id = inPerson.Id }, inPerson);
            }
            else
            {
                return BadRequest();
            }
        }


        // PUT: api/People/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, Person person) // TODO: add DexHolder id
        {
            if (id != person.Id) //(id != person.Id.ToString())
            {
                return BadRequest();
            }

            var personToUpdate = _context.Person.FirstOrDefault(a => a.Id.Equals(id));

            if (personToUpdate == null) // added to conform to tutorial. is likely redundant, considering scaffold result
            {
                return NotFound();
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    //throw;
                    personToUpdate.Nickname =  person.Nickname;
                    personToUpdate.DexHolder = person.DexHolder ;
                    personToUpdate.DateOfBirth = person.DateOfBirth;
                    personToUpdate.Gender = person.Gender ;
                    personToUpdate.Pronouns =   person.Pronouns ;
                    personToUpdate.Rating =   person.Rating ;
                    personToUpdate.Favorite =   person.Favorite ;

                    _context.Person.Update(personToUpdate);
                    _context.SaveChanges();

                }


            }

            return NoContent();
        }




        // DELETE: api/People/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePerson(int id)
        {
            //var person = await _context.Person.FindAsync(id);
            var personToDelete = _context.Person.FirstOrDefault(a => a.Id.Equals(id));

            if (personToDelete == null)
            {
                return NotFound();
            }

            _context.Person.Remove(personToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }




        private bool PersonExists(int id)
        {
            var personToFind = _context.Person.FirstOrDefault(a => a.Id.Equals(id));
            
            return _context.Person.Any(e => e.Id == id);
        }
    }
}
