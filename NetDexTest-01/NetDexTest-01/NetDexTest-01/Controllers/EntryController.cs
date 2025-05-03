using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Models.ViewModels;
using NetDexTest_01.Services;
using SQLitePCL;

namespace NetDexTest_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEntryItemRepository _entryRepo;
        private readonly IPersonRepository _personRepo;
        private readonly IUserRepository _userRepo;
        private readonly ILogger<EntryController> _logger;
        private ApplicationDbContext _db;// = _context;
        public EntryController(ApplicationDbContext context,
            IPersonRepository personRepo,
            IUserRepository userRepo,
            IEntryItemRepository entryRepo,
            ILogger<EntryController> logger
            )
        {
            _userRepo = userRepo;
            _personRepo = personRepo;
            _context = context;
            _logger = logger;
            _entryRepo = entryRepo;
            _context = context;
            
        }




        // GET: api/Entry
        [HttpGet]
        public async Task<ActionResult<ICollection<EntryItem>>> GetEntryItem()
        {
            return 
                await //_entryRepo.ReadAllEntriesAsync();//_context.EntryItem.ToListAsync();
                _context.EntryItem
                    .Include(e => e.RecordCollector)
                        .ThenInclude(r => r.Person)
                            .ThenInclude(p => p.FullName)
                    .ToListAsync();
                
        }

        // GET: api/Entry
        [HttpGet("/entries/{id}")]
        public async Task<ActionResult<IEnumerable<EntryItem>>> GetEntryItemVMById(string id)
        {
            //var entries = await _context.EntryItem.ToListAsync();
            
            //IEnumerable<EntryItemVM> enumEntry = new List<EntryItemVM>();
            //var entries = await entries.ToList().ForEach(e => enumEntry.Append(e)); ;
            if (int.TryParse(id, out int idout))
            {
                await Console.Out.WriteLineAsync($"\nReadAllEntries:\n\t Criteria, {id}, is an int. Checking PersonId!!!\n");
                return await _db.EntryItem
                    .Include(e => e.RecordCollector)
                        .ThenInclude(r => r.Person)
                            .ThenInclude(p => p.FullName)
                        .Where(e => e.RecordCollector.Person.Id == idout)
                    .ToListAsync();
            }
            else
            {
                await Console.Out.WriteLineAsync($"\n\nReadAllEntries:\n\t Criteria, {id}, is not an int. Checking nickname!!!\n");
                return await _db.EntryItem
                    .Include(e => e.RecordCollector)
                        .ThenInclude(r => r.Person)
                            .ThenInclude(p => p.FullName)
                        .Where(e => e.RecordCollector.Person.Nickname == id)
                    .ToListAsync();
            }

            //return
        }

        // GET: api/Entry/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EntryItem>> GetEntryItem(long id)
        {
            var entryItem = await _context.EntryItem.FindAsync(id);

            if (entryItem == null)
            {
                return NotFound();
            }

            return entryItem;
        }

        // PUT: api/Entry/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntryItem(long id, EntryItem entryItem)
        {
            if (id != entryItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(entryItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntryItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Entry
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EntryItem>> PostEntryItem(EntryItem entryItem)
        {
            _context.EntryItem.Add(entryItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEntryItem", new { id = entryItem.Id }, entryItem);
        }

        // DELETE: api/Entry/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntryItem(long id)
        {
            var entryItem = await _context.EntryItem.FindAsync(id);
            if (entryItem == null)
            {
                return NotFound();
            }

            _context.EntryItem.Remove(entryItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EntryItemExists(long id)
        {
            return _context.EntryItem.Any(e => e.Id == id);
        }
    }
}
