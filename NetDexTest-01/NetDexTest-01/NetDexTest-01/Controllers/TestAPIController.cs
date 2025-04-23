using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Services;
using NuGet.Protocol;
using System.Text.Json.Serialization;
using System.Text.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NetDexTest_01.Controllers
{



    [Route("testapi")]
    [ApiController]
    public class TestAPIController : ControllerBase
    {



        private readonly ApplicationDbContext _db; //_context
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly ILogger<ApplicationUser> _logger;
        private readonly IUserRepository _userRepo;
        private readonly IPersonRepository _personRepo;
        private readonly IDebugRepository _debugRepo;


        public TestAPIController(ApplicationDbContext db,
        UserManager<ApplicationUser> userManager,
        IUserStore<ApplicationUser> userStore,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<IdentityRole> roleManager,
        ILogger<ApplicationUser> logger,
        IUserRepository userRepo,
        IPersonRepository personRepo,
        IDebugRepository debugRepo
        )
        {
            _db = db; //context
            _userManager = userManager;
            _roleManager = roleManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
            _userRepo = userRepo;
            _personRepo = personRepo;
            _debugRepo = debugRepo;
        }




        // GET: api/<TestAPIController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/<TestAPIController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<TestAPIController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        // PUT api/<TestAPIController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        // DELETE api/<TestAPIController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}



        //[HttpGet("all")]
        //public async Task<IActionResult> GetAsync()
        //{ }




        [HttpGet("all")]
        public async Task<IActionResult> GetAsync()
        {
            var users = await _debugRepo.ReadAllUsersDebugAsync();
            var people = await _debugRepo.ReadAllPeopleDebugAsync();
            var peoplePeople = await _debugRepo.ReadAllRelationsDebugAsync();
            var dexHolders = await _debugRepo.ReadAllDexHoldersDebugAsync();
            //var 
            var all = await _debugRepo.ReadAbsolutelyAllDebugAsync();


            //var model = from p in people
            //            join pp in peoplePeople
            //                on p.Id equals pp.PersonParentId
            //            orderby p.Nickname, p.FullName.NameLast, p.FullName.NameFirst
            //            select new
            //            {
            //                Alias = p.Nickname,
            //                Name = p.FullName.NameFirst + " " + p.FullName.NameLast,
            //                = 
            //            }

            JsonSerializerOptions options = new()
            {
                ReferenceHandler = ReferenceHandler.Preserve, //IgnoreCycles, //Preserve
                WriteIndented = true
            };

            string modelJson = JsonSerializer.Serialize(all, options);

            //var model = all;

            await Console.Out.WriteLineAsync($"\n\n\n\n serialized: {modelJson}\n \n\n\n\n");

            var model = modelJson;
            return Ok(model);

            //var students = await _studentRepo.ReadAllAsync();
            //var studentCourseGrades =
            //   await _studentCourseRepo.ReadAllAsync();
            //var model = from s in students
            //            join scg in studentCourseGrades
            //                on s.ENumber equals scg.StudentENumber
            //            orderby s.LastName, s.FirstName
            //            select new
            //            {
            //                StudentName = s.FirstName + " " + s.LastName,
            //                CourseFullCode = scg.Course!.Code,
            //                scg.LetterGrade
            //            };

            return Ok(model);


        }





        //[HttpGet("all1")]
        //public async Task<IActionResult> GetAsync1()
        //{
        //    var users = await _debugRepo.ReadAllUsersDebugAsync();
        //    var people = await _debugRepo.ReadAllPeopleDebugAsync();
        //    var peoplePeople = await _debugRepo.ReadAllRelationsDebugAsync();
        //    var dexHolders = await _debugRepo.ReadAllDexHoldersDebugAsync();
        //    //var 
        //    var all = await _debugRepo.ReadAbsolutelyAllDebugAsync();


        //    //var model = from p in people
        //    //            join pp in peoplePeople
        //    //                on p.Id equals pp.PersonParentId
        //    //            orderby p.Nickname, p.FullName.NameLast, p.FullName.NameFirst
        //    //            select new
        //    //            {
        //    //                Alias = p.Nickname,
        //    //                Name = p.FullName.NameFirst + " " + p.FullName.NameLast,
        //    //                = 
        //    //            }

        //    var model = dexHolders;



        //}}
    }
}
