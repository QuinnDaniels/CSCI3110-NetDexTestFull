using Microsoft.AspNetCore.Identity;
using NetDexTest_01.Models.Entities;
using System.Collections.Generic;
using NetDexTest_01.Contexts;
using NetDexTest_01.Constants;
using System.Net;
using System;


namespace NetDexTest_01.Services
{
    public class Initializer
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserRepository _userRepository;
        private readonly IPersonRepository _personRepository;

        private readonly NetDexTest_01.Constants.Authorization _authorization;

        public Initializer(
            ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserRepository userRepository,
            IPersonRepository personRepository,
            NetDexTest_01.Constants.Authorization authorization
            )
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _userRepository = userRepository;
            _personRepository = personRepository;
            _authorization = authorization;
        }

        /// <summary>
        /// Seeds the users into the database upon creation.
        /// </summary>
        /// <returns>A Task.</returns>
        public async Task SeedUsersAsync()
        {
            // Ensure database is created
            _db.Database.EnsureCreated();


            #region seedRoles

            if (!_db.Roles.Any(r => r.Name == "Administrator"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Administrator" } );//( Authorization.Roles.Administrator.ToString() );
            }
            if (!_db.Roles.Any(r => r.Name == "Fake"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Fake" });
            }
            if (!_db.Roles.Any(r => r.Name == "Trainer"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Trainer" });
            }

            if (!_db.Roles.Any(r => r.Name == "Moderator"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Moderator" });
            }
            if (!_db.Roles.Any(r => r.Name == "User"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "User" });
            }

            #endregion seedRoles






            //if (!_db.Users.Any(u => u.UserName == "admin@test.com"))
            //{
            //    var user = new ApplicationUser
            //    {
            //        Email = "admin@test.com",
            //        UserName = "admin@test.com",
            //    };
            //    await _userManager.CreateAsync(user, "Pass123!");
            //    await _userManager.AddToRoleAsync(user, "Admin");
            //}

            await SeedUserMethodAsync();
            await ApplicationDbContextSeed.SeedEssentialsAsync(_userManager, _roleManager); // From JWT Tutorial
        }

        /// <summary>
        /// Creates Users with DexHolder information to seed the database with
        /// </summary>
        /// <returns></returns>
        public async Task SeedUserMethodAsync()
        {
            var users = new List<ApplicationUser> { };
            var dexHolders = new List<DexHolder> { };
            List<String> passes = new List<string> { };

            ApplicationUser adminUser = new ApplicationUser
            {
                Email = "admin@test.com",
                UserName = "admin@test.com",
            };

            ApplicationUser fakeUser = new ApplicationUser
            {
                Email = "fake@email.com",
                UserName = "fake@email.com",
            };


            

            /*begin default users*/
            if (!_db.Users.Any(u => u.UserName == adminUser.UserName))
            {
                users.Add(adminUser);
                dexHolders.Add(new DexHolder { FirstName = null, LastName = null, Gender = null });
                passes.Add("AdminPass123!");
            }
            if (!_db.Users.Any(u => u.UserName == fakeUser.UserName))
            {
                users.Add(fakeUser);
                dexHolders.Add(new DexHolder { FirstName = "fake", LastName = "user", Gender = null });
                passes.Add("Pass123!");

            }
            /*end default users*/

            if (!_db.Users.Any(u => u.UserName == fakeUser.UserName))
            {
                users.Add(new ApplicationUser
                {
                    Email = "janedoe@email.com",
                    UserName = "janedoe@email.com",
                });
                dexHolders.Add(new DexHolder { FirstName = "Jane", LastName = "Doe", Gender = "woman" });
                passes.Add("Pass123!");

            }
            
            if (!_db.Users.Any(u => u.UserName == "Cossack"))
            {
                users.Add(new ApplicationUser
                {
                    Email = "Cossack@CossackIndustries.com",
                    UserName = "Cossack",
                });
                dexHolders.Add(new DexHolder { FirstName = "Dr", LastName = "Cossack", Gender = "Man" });
                passes.Add("Pass123!");

            };

            if (!_db.Users.Any(u => u.UserName == "Wily"))
            {
                users.Add(new ApplicationUser
                {
                    Email = "Wily@dwn.com",
                    UserName = "Wily",
                });
                dexHolders.Add(new DexHolder 
                {
                    FirstName = "Dr",
                    LastName = "Wily",
                    Gender = "Man"
                    
                });
                passes.Add("Pass123!");

            };




            await _userRepository.CreateUserDexHolderAsync(users, dexHolders, passes);

            Console.WriteLine($"\n\n\n---------------------------------"
                   + $"\n\n\n"
                   + $"NOTICE:\tATTEMPTING TO FIND A DEXHOLDER\n\n"
                   + $"\"Cossack\"\n\n-------------------------------\n");


            Person? p1 = null;
            Person? p2 = null;


            string tempNickname1 = "PipeBurstingRobot";
            string tempNickname2 = "skullybones";

            var drillman = await _userRepository.GetDexHolderByUserNameAsync("Cossack");
            if (drillman != null)
            {
                Console.WriteLine($"\n\n\n---------------------------------"
                   + $"\n\n\n"
                   + $"NOTICE:\tInstantiating a person for DEXHOLDER\n\n"
                   + $"\"{drillman}\"\n\n-------------------------------\n");

                var person = new Person
                {
                    Nickname = tempNickname1,
                    DexHolder = drillman,
                    FullName = new FullName()
                    {
                        NameFirst = "Drill",
                        NameLast = "Man",
                        PhNameLast = "Drillman"
                    },
                    RecordCollector = new RecordCollector()
                    {
                        NoteText = "Testing a new way to add multiple things at once",
                        EntryItems = new List<EntryItem> 
                        {
                            new EntryItem { ShortTitle = "Creation" , FlavorText = "Created by Dr Cossack (me!)" },
                            new EntryItem { ShortTitle = "Conversion in MM4" , FlavorText = "Modified by Dr. Wily and reclassified as DWN 027" },
                            new EntryItem { ShortTitle = "Defeat", FlavorText = "Defeated at the hands of Megaman" }
                        }
                    },
                    ContactInfo = new ContactInfo()
                    {
                        NoteText = "Testing a new way to add multiple things at once",
                        SocialMedias = new List<SocialMedia>
                        { 
                            new SocialMedia { CategoryField = "Reddit", SocialHandle = "u/dwn027" },
                            new SocialMedia { CategoryField = "Twitter", SocialHandle = "@DrillBabyDrillMan" },
                            new SocialMedia { CategoryField = "Email", SocialHandle = "drillman027@dwn.com" }
                        }
                    }

                };


                //Person oldP = new();
                var findOldPerson = !_db.Person.Any(p => p.Nickname == person.Nickname && p.DexHolder == drillman);
                if (findOldPerson)
                { 
                
                    Console.WriteLine($"\n\n\n---------------------------------"
                                       +$"\n\n\n"
                                       +$"NOTICE:\tATTEMPTING TO ADD A PERSON\n\n"
                                       +$"{person}\n\n\tTO THE DEXHOLDER\n"
                                       +$"{drillman}\n\n-------------------------------\n");
                    var newAdd = await _db.Person.AddAsync(person);
                    person = _db.Person.FirstOrDefault(p => p.Nickname == person.Nickname && p.DexHolder == person.DexHolder);

                }
                else
                {
                    Console.WriteLine($"\n\n\n---------------------------------"
                   + $"\n\n\n"
                   + $"NOTICE:\tEXISTING PipeBurstingRobot FOUND, MODIFYING person\n\n"
                   + $"{person}\n\n\tTO THE DEXHOLDER\n"
                   + $"{drillman}\n\n-------------------------------\n");

                    person = _db.Person.FirstOrDefault(p => p.Nickname == person.Nickname && p.DexHolder == person.DexHolder);
                }
                
                
                
                p1 = person;


                if (drillman != null)
                {
                    var new2 = new Person(tempNickname2, drillman);
                    
                    Console.WriteLine($"\n\n\n---------------------------------"
                   + $"\n\n\n"
                   + $"NOTICE:\tATTEMPTING TO ADD A PERSON\n\n"
                   + $"{new2}\n\n\tTO THE DEXHOLDER\n"
                   + $"{drillman}\n\n-------------------------------\n");

                    if (_db.Person.Any(p => p.Nickname == new2.Nickname && p.DexHolder == drillman))
                    {
                        //new2 = _db.Person.FirstOrDefault(p => p.Nickname == person.Nickname && p.DexHolder == person.DexHolder);


                    }
                    else
                    {
                        var newAdd2 = await _db.Person.AddAsync(new2);
                        //new2 = _db.Person.FirstOrDefault(p => p.Nickname == person.Nickname && p.DexHolder == person.DexHolder);
                    }

                    //p2 = new2;
                }

                await _db.SaveChangesAsync();
                //
                //if (!_db.PersonPerson.Any())
                //{

                await Console.Out.WriteLineAsync("\n\n\n--------------------\n\n"
                    +"Searching for p1 & p2..."
                    +"\n\n"
                    +"\n\n\n-------------------\n\n\n\n");


                    p1 = _db.Person.FirstOrDefault(p => p.Nickname == tempNickname1 && p.DexHolder == drillman);
                if (p1 == null) { await Console.Out.WriteLineAsync("ERROR: p1 IS NULL"); }
                else
                    await Console.Out.WriteLineAsync($"p1: \t {p1}"
                            +$"{p1.Nickname} - dex: {p1.DexHolder.ApplicationUserName}"
                            +"\n\n");
                {
                }




                    p2 = _db.Person.FirstOrDefault(p => p.Nickname == tempNickname2 && p.DexHolder == drillman);
                if (p2 == null) { await Console.Out.WriteLineAsync("ERROR: p2 IS NULL"); }
                else
                {
                    await Console.Out.WriteLineAsync($"p1: \t {p2}"
                        + $"{p2.Nickname} - dex: {p2.DexHolder.ApplicationUserName}"
                        + "\n\n");
                    await Console.Out.WriteLineAsync("\n\n\n--------------------\n\n");
                }



                if (p1 != null && p2 != null)
                    {
                    await Console.Out.WriteLineAsync("\n\n\n--------------------\n\n"
                            + "p1 & p2 found!..."
                            + "\n\n\n-------------------\n\n\n\n");



                    await Console.Out.WriteLineAsync("\n\n\n--------------------\n\n"
                        + "Re-getting p1 & p2..."
                        + "\n\n"
                        + "\n\n\n-------------------\n\n\n\n");



                    p1 = await _personRepository.GetPersonByNickNameWithDex(p1.Nickname, p1.DexHolder);
                        p2 = await _personRepository.GetPersonByNickNameWithDex(p2.Nickname, p2.DexHolder);

                    await Console.Out.WriteLineAsync($"\n\n\np1: \t {p1}"
                        + $"{p1.Nickname} - dex: {p1.DexHolder.ApplicationUserName}"
                        + "\n\n");

                    await Console.Out.WriteLineAsync($"\n\n\n\np2: \t {p2}"
                        + $"{p2.Nickname} - dex: {p2.DexHolder.ApplicationUserName}"
                        + "\n\n");


                    await Console.Out.WriteLineAsync("\n\n\n--------------------\n\n"
                        + "Attempting redundant check...."
                        + "\n\n"
                        + "\n\n\n-------------------\n\n\n\n");


                    if (p1 == null) { await Console.Out.WriteLineAsync("ERROR: p1 IS NULL"); }
                        if (p2 == null) { await Console.Out.WriteLineAsync("ERROR: p2 IS NULL"); }
                        if (p1 != null && p2 != null)
                        {

                        await Console.Out.WriteLineAsync("\n\n\n--------------------\n\n"
                            + "Entered conditional statement...."
                            + "\n\n"
                            + "\n\n\n-------------------\n\n\n\n");


                        PersonPerson p3 = new PersonPerson(p1, p2);
                        PersonPerson p4 = new PersonPerson(p2, p1);
                        //p3!.RelationshipDescription = string.Empty;
                        if (!_personRepository.FindMatch(p3))
                            {
                                p2.PersonParents.Add(p3);
                                p1.PersonChildren.Add(p3);
                                await Console.Out.WriteLineAsync("\n\n\nNOTICE: p1 & p2 added! Attempting to save changes\n\n\n");
                                await _db.SaveChangesAsync();

                            }


                            // NOTICE FOR CODE:
                            // --------------------------
                            //   //p3!.RelationshipDescription = "Mother of";
                            //   //_personRepository.AddPersonPersonCheck(p1, p2, p3);
                            //   //await Console.Out.WriteLineAsync("NOTICE: p1 & p2 added! Attempting to save changes");
                            //-->//await _db.SaveChangesAsync();
                            // --------------------------------
                            // An error occurred while seeding the database. The property 'PersonPerson.RelationshipDescription' is part of a key and so cannot be modified or marked as modified. To change the principal of an existing entity with an identifying foreign key, first delete the dependent and invoke 'SaveChanges', and then associate the dependent with the new principal.
                            // -------------------------------


                            string desc = string.Empty;
                        string notice = "\n\n\n\nNOTICE: p1 & p2 added! Attempting to save changes\n";


                            desc = "Mother of";         //  <----   /*p3!.RelationshipDescription = "Mother of";*/
                        if (!_personRepository.FindMatch(p3, desc) && !_db.PersonPerson.Any(pp => pp.PersonParentId == p3.PersonParentId && pp.PersonChildId == p3.PersonChildId && pp.RelationshipDescription == desc))
                        {
                            var check = await _personRepository.AddPersonPersonCheckAsync(p3, desc); //p1, p2, p3);
                            await Console.Out.WriteLineAsync($"\n\n----motherof-------\n\n\n{check}\n\n\n---------\n\n");
                            await _db.SaveChangesAsync();
                        }
                            desc = "Daughter of";         //  <----   /*p3!.RelationshipDescription = "Daughter of";*/

                        if (!_personRepository.FindMatch(p4, desc) && !_db.PersonPerson.Any(pp => pp.PersonParentId == p4.PersonParentId && pp.PersonChildId == p4.PersonChildId && pp.RelationshipDescription == desc))
                        {
                            await _personRepository.AddPersonPersonCheckAsync(p4, desc) ; //p2, p1, p3);
                            await Console.Out.WriteLineAsync(notice);
                        }

                            desc = "Teaches to";         //  <----   /*p3!.RelationshipDescription = "Teaches to";*/
                        if (!_personRepository.FindMatch(p3, desc) && !_db.PersonPerson.Any(pp => pp.PersonParentId == p3.PersonParentId && pp.PersonChildId == p3.PersonChildId && pp.RelationshipDescription == desc))
                        {

                            await _personRepository.AddPersonPersonCheckAsync(p3, desc) ; //p2, p1, p3);
                            await Console.Out.WriteLineAsync(notice);
                        }

                            desc = "Learns from";         //  <----   /*p3!.RelationshipDescription = "Learns from";*/
                        if (!_personRepository.FindMatch(p4, desc) && !_db.PersonPerson.Any(pp => pp.PersonParentId == p4.PersonParentId && pp.PersonChildId == p4.PersonChildId && pp.RelationshipDescription == desc))
                        {
                            await _personRepository.AddPersonPersonCheckAsync(p4, desc) ; //p1, p2, p3);
                            await Console.Out.WriteLineAsync(notice);
                        }

                        await Console.Out.WriteLineAsync("\n\n\n--------------------\n\n"
                            + "Exiting conditional statement...."
                            + "\n\n"
                            + "\n\n\n-------------------\n\n\n\n");

                    }

                }
                    else
                    {
                    await Console.Out.WriteLineAsync("\n\n\n--------------------\n\n"
                        + "p1 & p2 not found!..."
                        + "\n\n\n-------------------\n\n\n\n");


                    await Console.Out.WriteLineAsync("\n\nNOTICE: p1 and/or p2 were returned as null! PersonPerson entries were not added!!\n\n\n");
                    }

                //} // END if PersonPerson.Any()
                


            }


            if (users.Contains(adminUser)) { await _userManager.AddToRoleAsync(adminUser, "Administrator"); }
            if (users.Contains(fakeUser)) { await _userManager.AddToRoleAsync(fakeUser, "Fake"); }


        }








        // TODO: Seed Persons 
       

    } 
}
        
