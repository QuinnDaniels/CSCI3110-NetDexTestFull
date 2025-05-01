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
using NuGet.Protocol;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Cors;

using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace NetDexTest_01.Controllers
{

    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public partial class PeopleController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IPersonRepository _personRepo;
        private readonly IUserRepository _userRepo;
        private readonly ILogger<PeopleController> _logger;
        public PeopleController(ILogger<PeopleController> logger,
            ApplicationDbContext context,
            IPersonRepository personRepo,
            IUserRepository userRepo
            )
        {
            _userRepo = userRepo;
            _personRepo = personRepo;
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


        [Route("retrieveViewModel")]
        [HttpGet()]
        public async Task<ActionResult<PersonDexListVM>> GetPersonWithVM([FromBody] PersonRequest personRequest)
        {
            //var person = _context.Person.FirstOrDefault(a => a.Id.Equals(id));
            await Console.Out.WriteLineAsync($"\n\n\n\n serialized: {personRequest.ToJson()}\n \n\n\n\n");
            var id = personRequest.Criteria;
            var userId = personRequest.UserInput;


            PersonPlusDexListVM? personVM = null;

            if (int.TryParse(id, out int idout)) // in case you want to try to use the local counter
            {
                personVM = await _userRepo.GetPersonPlusDexListVMAsync(userId, id);
            }
            else
            {
                personVM = await _userRepo.GetPersonPlusDexListVMAsync(userId, id);

            }

            await Console.Out.WriteLineAsync($"\n\n\nLOG:\n\t{personVM.ToJson()} \n\n\n");


            if (personVM == null)
            {
                return NotFound();
            }
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles, //IgnoreCycles, //Preserve
                WriteIndented = true
            };

            string modelJson = JsonSerializer.Serialize(personVM, options);

            //var model = all;

            await Console.Out.WriteLineAsync($"\n\n\n\n serialized: {modelJson}\n \n\n\n\n");

            var model = modelJson;
            return Ok(model);
        }





        [Route("retrieveRequestpath/{input}/{criteria}")]
        [HttpGet()]
        public async Task<ActionResult<PersonDexListVM>> GetPersonWithVM(string input, string criteria)
        {
            //var person = _context.Person.FirstOrDefault(a => a.Id.Equals(id));
            //await Console.Out.WriteLineAsync($"\n\n\n\n serialized: {personRequest.ToJson()}\n \n\n\n\n");
            var id = criteria;
            var userId = input;


            PersonPlusDexListVM? personVM = null;

            if (int.TryParse(id, out int idout)) // in case you want to try to use the local counter
            {
                personVM = await _userRepo.GetPersonPlusDexListVMAsync(userId, id);
            }
            else
            {
                personVM = await _userRepo.GetPersonPlusDexListVMAsync(userId, id);

            }

            await Console.Out.WriteLineAsync($"\n\n\nLOG:\n\t{personVM.ToJson()} \n\n\n");


            if (personVM == null)
            {
                return NotFound();
            }
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles, //IgnoreCycles, //Preserve
                WriteIndented = true
            };

            string modelJson = JsonSerializer.Serialize(personVM, options);

            //var model = all;

            await Console.Out.WriteLineAsync($"\n\n\n\n serialized: {modelJson}\n \n\n\n\n");

            var model = modelJson;
            return Ok(model);
        }




        //all
        // all by user input
        // one via request
        // all matching request 


        [Route("retrieveRelations/all")]
        [HttpGet()]
        public async Task<ActionResult<ICollection<RelationshipVM>>> retrieveAllRelations()
        {
            var relationshipList = await _personRepo.GetAllRelationshipsAsync();
            
            if (relationshipList != null)
            {
                JsonSerializerOptions options = new()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles, //IgnoreCycles, //Preserve
                    WriteIndented = true
                };

                string modelJson = JsonSerializer.Serialize(relationshipList, options);

                //var model = all;

                await Console.Out.WriteLineAsync($"\n\n\n\n serialized: {modelJson}\n \n\n\n\n");

                var model = modelJson;
                return Ok(model);
            }
            return BadRequest();
        }

        [Route("retrieveRelations/User/{id}")]
        [HttpGet()]
        public async Task<ActionResult<ICollection<RelationshipVM>>> retrieveAllRelationsByUser(string id)
        {
            var relationshipList = await _personRepo.GetAllRelationshipsByUserAsync(id);

            if (relationshipList != null)
            {
                JsonSerializerOptions options = new()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles, //IgnoreCycles, //Preserve
                    WriteIndented = true
                };

                string modelJson = JsonSerializer.Serialize(relationshipList, options);

                //var model = all;

                await Console.Out.WriteLineAsync($"\n\n\n\n serialized: {modelJson}\n \n\n\n\n");

                var model = modelJson;
                return Ok(model);
            }
            return BadRequest();

        }

        [Route("retrieveRelations/specific")] // search by nicknames, ignoring description
        [HttpGet()]
        public async Task<ActionResult<ICollection<RelationshipVM>>> retrieveAllRelationsSpecific([FromBody] RelationshipRequest relation)
        {
            var relationshipList = await _personRepo.GetAllRelationshipsWithPeopleRequestAsync(relation);

            if (relationshipList != null)
            {
                JsonSerializerOptions options = new()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles, //IgnoreCycles, //Preserve
                    WriteIndented = true
                };

                string modelJson = JsonSerializer.Serialize(relationshipList, options);

                //var model = all;

                await Console.Out.WriteLineAsync($"\n\n\n\n serialized: {modelJson}\n \n\n\n\n");

                var model = modelJson;
                return Ok(model);
            }
            return BadRequest();

        }

        [Route("retrieveRelations/one")] // use the request model to get the specific relation
        [HttpGet()]
        public async Task<ActionResult<RelationshipVM>> retrieveRelations([FromBody] RelationshipRequest relation)
        {
            var relationshipList = await _personRepo.GetOneRelationshipWithRequestAsync(relation);

            if (relationshipList != null)
            {
                JsonSerializerOptions options = new()
                {
                    ReferenceHandler = ReferenceHandler.IgnoreCycles, //IgnoreCycles, //Preserve
                    WriteIndented = true
                };

                string modelJson = JsonSerializer.Serialize(relationshipList, options);

                //var model = all;

                await Console.Out.WriteLineAsync($"\n\n\n\n serialized: {modelJson}\n \n\n\n\n");

                var model = modelJson;
                return Ok(model);
            }
            return BadRequest();

        }




        // NOTE: From Uri????
        // GET: api/People/n/skullybones
        [HttpGet("u/{userId}/n/{id}")]
        public async Task<ActionResult<PersonDexListVM>> GetPerson(string userId, string id)
        {
            //var person = _context.Person.FirstOrDefault(a => a.Id.Equals(id));
            PersonPlusDexListVM? personVM = null;

            if (int.TryParse(id, out int idout)) // in case you want to try to use the local counter
            {
                personVM = await _userRepo.GetPersonPlusDexListVMAsync(userId, id);
            }
            else
            {
                personVM = await _userRepo.GetPersonPlusDexListVMAsync(userId, id);

            }

            await Console.Out.WriteLineAsync($"\n\n\nLOG:\n\t{personVM.ToJson()} \n\n\n");


            if (personVM == null)
            {
                return NotFound();
            }
            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles, //IgnoreCycles, //Preserve
                WriteIndented = true
            };

            string modelJson = JsonSerializer.Serialize(personVM, options);

            //var model = all;

            await Console.Out.WriteLineAsync($"\n\n\n\n serialized: {modelJson}\n \n\n\n\n");

            var model = modelJson;
            return Ok(model);
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
        public async Task<ActionResult<Person>> InsertPerson(NewPersonVM Person)
        {
            //person.Id = Guid.NewGuid();
            

            Person inPerson = await _personRepo.CreatePersonAsync(Person);

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


        // POST: api/People/Create
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Route("Forms/Create")]
        [HttpPost]
        public async Task<ActionResult> CreatePerson([FromForm]NewPersonVM person)
        {
            //person.Id = Guid.NewGuid();

            await Console.Out.WriteLineAsync("\n\n\n ----------------------- \n\n CreatePerson Endpoint Reached! \n\n -------------------\n\n ");

            //NewPersonVM personVM = person.GetNewPersonVMInstance();

            Person inPerson = await _personRepo.CreatePersonAsync(person);

            //_context.Person.Add(inPerson);
            //await _context.SaveChangesAsync();

            if (inPerson != null)
            {
                return Ok(new
                {
                    Nickname = inPerson.Nickname,
                    DateOfBirth = inPerson.DateOfBirth,
                    Gender = inPerson.Gender,
                    Pronouns = inPerson.Pronouns,
                    Rating = inPerson.Rating,
                    Favorite = inPerson.Favorite,
                    Email = person.Email

                });

            }
            else
            {
                return BadRequest();
            }
        }




        // PUT: api/People/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("forms/update/{id}")]
        public async Task<ActionResult> UpdatePerson([FromForm]EditPersonVM person, int id) // TODO: add DexHolder id
        {
            await Console.Out.WriteLineAsync("\n\n\n ----------------------- \n\n CreatePerson Endpoint Reached! \n\n -------------------\n\n ");

            await Console.Out.WriteLineAsync($"\n\t {person?.Nickname ?? "Nickname is null!"}\n  ");
            
            await Console.Out.WriteLineAsync($"\n\t {person?.Gender ?? "Gender is null!"}\n");
            await Console.Out.WriteLineAsync($"\n\t {person?.Pronouns ?? "Pronouns is null!"}\n  ");
            await Console.Out.WriteLineAsync($"\n\t {person?.Rating ?? -404}\n  ");
            



            var personToUpdate = _context.Person.FirstOrDefault(a => a.Id.Equals(person.Id) || a.Id == id  || a.Id == person.Id);

            if (personToUpdate == null) // added to conform to tutorial. is likely redundant, considering scaffold result
            {
                return NotFound($"Could not find a person for id, {person.Id}\n{person.Nickname}\n{person.DateOfBirth}\n{person.Gender}\n{person.Pronouns}\n{person.Rating}\n{person.Favorite}\n\n");
            }

            //TODO replace this with an email check?
            if (id != personToUpdate.Id) //(id != person.Id.ToString())
            {
                return BadRequest($"id, {id}, did not match personToUpdate id, {personToUpdate.Id}. Person.Id: {person.Id}");
            }
            //if (id != person.Id) //(id != person.Id.ToString())
            //{
            //    return BadRequest($"id, {id}, did not match person id, {person.Id}. PersonToUpdate id: {personToUpdate.Id}");
            //}
            await Console.Out.WriteLineAsync("\n\n\n ----------------------- \n\n Conditional Checks Passed! \n\n -------------------\n\n ");

            _context.Entry(personToUpdate).State = EntityState.Modified;

            try
            {
                await Console.Out.WriteLineAsync($"\n\t {personToUpdate.Nickname} -> {person?.Nickname ?? personToUpdate.Nickname}\n  ");
                personToUpdate.Nickname = person?.Nickname ?? personToUpdate.Nickname;
                //personToUpdate.DexHolder = person.DexHolder ;
                await Console.Out.WriteLineAsync($"\n\t {personToUpdate.DateOfBirth} => {person?.DateOfBirth ?? personToUpdate.DateOfBirth}\n  ");
                personToUpdate.DateOfBirth = person?.DateOfBirth ?? personToUpdate.DateOfBirth;
                await Console.Out.WriteLineAsync($"\n\t {personToUpdate.Gender} => {person?.Gender ?? personToUpdate.Gender}\n  ");
                personToUpdate.Gender = person?.Gender ?? personToUpdate.Gender;
                await Console.Out.WriteLineAsync($"\n\t {personToUpdate.Pronouns} => {person?.Pronouns ?? personToUpdate.Pronouns}\n  ");
                personToUpdate.Pronouns = person?.Pronouns ?? personToUpdate.Pronouns;
                await Console.Out.WriteLineAsync($"\n\t {personToUpdate.Rating} => {person?.Rating ?? personToUpdate.Rating}\n  ");
                personToUpdate.Rating = person?.Rating ?? personToUpdate.Rating;
                await Console.Out.WriteLineAsync($"\n\t {personToUpdate.Favorite} => {person?.Favorite ?? personToUpdate.Favorite}\n  ");
                personToUpdate.Favorite = person?.Favorite ?? personToUpdate.Favorite;

                _context.Person.Update(personToUpdate);

                await _context.SaveChangesAsync();
                await Console.Out.WriteLineAsync("\n\n\n ----------------------- \n\n Changes saved to context! \n\n -------------------\n\n ");
            }
            catch (DbUpdateConcurrencyException ex)
            {
                await Console.Out.WriteLineAsync(ex.ToString());
                if (!PersonExists(id))
                {
                    return NotFound();
                }
                else
                {
                    //throw;
                    await Console.Out.WriteLineAsync($"\n\n\t {personToUpdate.Nickname} -> {person?.Nickname ?? personToUpdate.Nickname}\n  ");
                    personToUpdate.Nickname =  person?.Nickname ?? personToUpdate.Nickname;
                    //personToUpdate.DexHolder = person.DexHolder ;
                    await Console.Out.WriteLineAsync($"\n\n\t {personToUpdate.DateOfBirth} => {person?.DateOfBirth ?? personToUpdate.DateOfBirth}\n  ");
                    personToUpdate.DateOfBirth = person?.DateOfBirth ?? personToUpdate.DateOfBirth;
                    await Console.Out.WriteLineAsync($"\n\n\t {personToUpdate.Gender} => {person?.Gender ?? personToUpdate.Gender}\n  ");
                    personToUpdate.Gender = person?.Gender  ?? personToUpdate.Gender;
                    await Console.Out.WriteLineAsync($"\n\n\t {personToUpdate.Pronouns} => {person?.Pronouns ?? personToUpdate.Pronouns}\n  ");
                    personToUpdate.Pronouns = person?.Pronouns  ?? personToUpdate.Pronouns;
                    await Console.Out.WriteLineAsync($"\n\n\t {personToUpdate.Rating} => {person?.Rating  ?? personToUpdate.Rating}\n  ");
                    personToUpdate.Rating = person?.Rating  ?? personToUpdate.Rating;
                    await Console.Out.WriteLineAsync($"\n\n\t {personToUpdate.Favorite} => {person?.Favorite  ?? personToUpdate.Favorite}\n  ");
                    personToUpdate.Favorite = person?.Favorite  ?? personToUpdate.Favorite;

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




        // DELETE: api/People/5
        [Route("delete/{input}/{criteria}")]
        [HttpDelete()]
        public async Task<IActionResult> DeletePersonSpecific(string input, string criteria)
        {
            //var person = await _context.Person.FindAsync(id);
            var id = criteria;
            var userId = input;


            PersonPlusDexListVM? personVM = null;

            if (int.TryParse(id, out int idout)) // in case you want to try to use the local counter
            {
                personVM = await _userRepo.GetPersonPlusDexListVMAsync(userId, id);
            }
            else
            {
                personVM = await _userRepo.GetPersonPlusDexListVMAsync(userId, id);

            }
            if (personVM == null)
            {
                return NotFound();
            }
            var personToDelete = _context.Person.FirstOrDefault(a => a.Id.Equals(personVM.Id));

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
