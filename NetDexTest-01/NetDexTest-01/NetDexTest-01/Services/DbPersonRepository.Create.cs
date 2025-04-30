using NetDexTest_01.Models.Entities;
using NetDexTest_01.Models.ViewModels;

namespace NetDexTest_01.Services
{
    // TODO: do this!
    /// <summary>
    /// interacts with Extensions of Person (Ci, Fn, Rc)
    /// </summary>
    public partial class DbPersonRepository : IPersonRepository
    {
        // create just a person

        // create a person with ContactInfo, FullName, RecordCollector

        public void CreatePersonAsync()
        {

        }



        public async Task<Person?> CreatePersonAsync(ApplicationUser user, string personNickname)
        {
            var pp = await GetPersonByNickNameWithUser(personNickname, user);
            if (pp != null)
            {
                var newPerson = new Person
                {
                    Nickname = personNickname,
                    DexHolder = user.DexHolder,
                    FullName = new FullName(),
                    RecordCollector = new RecordCollector()
                    {
                        EntryItems = new List<EntryItem>
                        {
                            new EntryItem { ShortTitle = "Example", FlavorText = $"This is an example entry! It was automatically created on [ ${DateTime.Now} ]!" }
                        }
                    },
                    ContactInfo = new ContactInfo()
                    {
                        SocialMedias = new List<SocialMedia>
                        {
                            new SocialMedia {  CategoryField = "example category", SocialHandle = "@NetDexTest-01_Example_SocialHandle" }
                        }
                    }
                };

                await _db.Person.AddAsync(newPerson);
                await SaveChangesAsync();

                try
                {
                    return await GetPersonByNickName(personNickname, user);

                }
                catch (Exception ex)
                {
                    string title = "CreatePersonAsync";
                    QuinnException qEx = new QuinnException(title, ex);
                    var logger = _logger;
                    logger.LogError(
                         $"\n\n---------------- GetPersonWithNickName --- log ---------\n\n"
                        + $"An error occurred while adding lists to the database. {ex.Message}"
                        //        +$"\n\n---------------- SEED DATA ASYNC --- log ---------\n\n");
                        //    Console.WriteLine(
                        + $"\n\n----- 1 -------- GetPersonWithNickName --- console ----\n\n"
                        //        + $"An error occurred while adding lists to the database. {ex.Message}"
                        //        + $"\n\n----- 2 -------- SEED DATA ASYNC --- console ----\n\n"
                        + $"{ex}"
                        + $"\n\n---- end ------- GetPersonWithNickName --- console ----\n\n");
                    return null;
                }





            }
            else { return null; }

        }
        public async Task<Person?> CreatePersonAsync(DexHolder dex, string personNickname)
        {
            var pp = await GetPersonByNickNameWithDex(personNickname, dex);
            if (pp != null)
            {
                var newPerson = new Person(personNickname, dex);
                await _db.Person.AddAsync(newPerson);
                await SaveChangesAsync();

                try
                {
                    return await GetPersonByNickName(personNickname, dex);

                }
                catch (Exception ex)
                {
                    string title = "CreatePersonAsync";
                    QuinnException qEx = new QuinnException(title, ex);
                    var logger = _logger;
                    logger.LogError(qEx.Message);
                    return null;
                }
            }
            else
            {
                throw new NullReferenceException();
            }


        }


        public async Task<Person?> CreatePersonAsync(PropertyField pType, string inputProperty, string personNickname)
        {
            DexHolder? dex = null;
            switch (pType)
            {
                case PropertyField.id:
                    dex = await _userRepo.ReadDexByIdAsync(inputProperty);
                    break;
                case PropertyField.username:
                    dex = await _userRepo.ReadDexByUsernameAsync(inputProperty);
                    break;
                case PropertyField.email:
                    dex = await _userRepo.GetDexHolderByEmailAsync(inputProperty);
                    break;
                default:
                    throw new ArgumentException();
            }

            if (dex != null)
            {
                var dexId = dex.Id;
                var newPerson = new Person
                {
                    Nickname = personNickname,
                    DexHolder = dex,
                    FullName = new FullName(),
                    RecordCollector = new RecordCollector()
                    {
                        EntryItems = new List<EntryItem>
                        {
                            new EntryItem { ShortTitle = "Example", FlavorText = $"This is an example entry! It was automatically created on [ ${DateTime.Now} ]!" }
                        }
                    },
                    ContactInfo = new ContactInfo()
                    {
                        SocialMedias = new List<SocialMedia>
                        {
                            new SocialMedia {  CategoryField = "example category", SocialHandle = "@NetDexTest-01_Example_SocialHandle" }
                        }
                    }
                };

                await _db.Person.AddAsync(newPerson);
                await SaveChangesAsync();


                try
                {
                    return await GetPersonByNickName(personNickname, dex);

                }
                catch (Exception ex)
                {
                    string title = "CreatePersonAsync";
                    QuinnException qEx = new QuinnException(title, ex);
                    var logger = _logger;
                    logger.LogError(
                         $"\n\n---------------- GetPersonWithNickName --- log ---------\n\n"
                        + $"An error occurred while adding lists to the database. {ex.Message}"
                        //        +$"\n\n---------------- SEED DATA ASYNC --- log ---------\n\n");
                        //    Console.WriteLine(
                        + $"\n\n----- 1 -------- GetPersonWithNickName --- console ----\n\n"
                        //        + $"An error occurred while adding lists to the database. {ex.Message}"
                        //        + $"\n\n----- 2 -------- SEED DATA ASYNC --- console ----\n\n"
                        + $"{ex}"
                        + $"\n\n---- end ------- GetPersonWithNickName --- console ----\n\n");
                    return null;
                }
            }
            else
            {
                throw new NullReferenceException();
            }


        }


        public async Task<Person?> CreatePersonAsync(PropertyField pType, string inputProperty, Person person)
        {
            DexHolder? dex = null;
            switch (pType)
            {
                case PropertyField.id:
                    dex = await _userRepo.ReadDexByIdAsync(inputProperty);
                    break;
                case PropertyField.username:
                    dex = await _userRepo.ReadDexByUsernameAsync(inputProperty);
                    break;
                case PropertyField.email:
                    dex = await _userRepo.GetDexHolderByEmailAsync(inputProperty);
                    break;
                default:
                    throw new ArgumentException();
            }

            if (dex != null)
            {
                var dexId = dex.Id;
                var newPerson = new Person
                {
                    Nickname = person.Nickname,
                    DateOfBirth = person.DateOfBirth,
                    Gender = person.Gender,
                    Pronouns = person.Pronouns,
                    Rating = person.Rating,
                    DexHolder = dex,
                    FullName = new FullName(),
                    RecordCollector = new RecordCollector()
                    {
                        EntryItems = new List<EntryItem>
                        {
                            new EntryItem { ShortTitle = "Example", FlavorText = $"This is an example entry! It was automatically created on [ ${DateTime.Now} ]!" }
                        }
                    },
                    ContactInfo = new ContactInfo()
                    {
                        SocialMedias = new List<SocialMedia>
                        {
                            new SocialMedia {  CategoryField = "example category", SocialHandle = "@NetDexTest-01_Example_SocialHandle" }
                        }
                    }
                };

                await _db.Person.AddAsync(newPerson);
                await SaveChangesAsync();


                try
                {
                    return await GetPersonByNickName(person.Nickname, dex);

                }
                catch (Exception ex)
                {
                    string title = "CreatePersonAsync";
                    QuinnException qEx = new QuinnException(title, ex);
                    var logger = _logger;
                    logger.LogError(
                         $"\n\n---------------- GetPersonWithNickName --- log ---------\n\n"
                        + $"An error occurred while adding lists to the database. {ex.Message}"
                        //        +$"\n\n---------------- SEED DATA ASYNC --- log ---------\n\n");
                        //    Console.WriteLine(
                        + $"\n\n----- 1 -------- GetPersonWithNickName --- console ----\n\n"
                        //        + $"An error occurred while adding lists to the database. {ex.Message}"
                        //        + $"\n\n----- 2 -------- SEED DATA ASYNC --- console ----\n\n"
                        + $"{ex}"
                        + $"\n\n---- end ------- GetPersonWithNickName --- console ----\n\n");
                    return null;
                }
            }
            else
            {
                throw new NullReferenceException();
            }


        }




        public async Task<Person?> CreatePersonAsync(NewPersonVM person)
        {
            DexHolder? dex = null;

            dex = await _userRepo.GetDexHolderByEmailAsync(person.Email);

            if (dex != null)
            {
                var dexId = dex.Id;
                var newPerson = new Person
                {
                    Nickname = person.Nickname,
                    DateOfBirth = person.DateOfBirth,
                    Gender = person.Gender,
                    Pronouns = person.Pronouns,
                    Rating = person.Rating,
                    DexHolder = dex,
                    FullName = new FullName(),
                    RecordCollector = new RecordCollector()
                    {
                        EntryItems = new List<EntryItem>
                        {
                            new EntryItem { ShortTitle = "Example", FlavorText = $"This is an example entry! It was automatically created on [ ${DateTime.Now} ]!" }
                        }
                    },
                    ContactInfo = new ContactInfo()
                    {
                        SocialMedias = new List<SocialMedia>
                        {
                            new SocialMedia {  CategoryField = "example category", SocialHandle = "@NetDexTest-01_Example_SocialHandle" }
                        }
                    }
                };

                await _db.Person.AddAsync(newPerson);
                await SaveChangesAsync();


                try
                {
                    return await GetPersonByNickName(person.Nickname, dex);

                }
                catch (Exception ex)
                {
                    string title = "CreatePersonAsync";
                    QuinnException qEx = new QuinnException(title, ex);
                    var logger = _logger;
                    logger.LogError(
                         $"\n\n---------------- GetPersonWithNickName --- log ---------\n\n"
                        + $"An error occurred while adding lists to the database. {ex.Message}"
                        //        +$"\n\n---------------- SEED DATA ASYNC --- log ---------\n\n");
                        //    Console.WriteLine(
                        + $"\n\n----- 1 -------- GetPersonWithNickName --- console ----\n\n"
                        //        + $"An error occurred while adding lists to the database. {ex.Message}"
                        //        + $"\n\n----- 2 -------- SEED DATA ASYNC --- console ----\n\n"
                        + $"{ex}"
                        + $"\n\n---- end ------- GetPersonWithNickName --- console ----\n\n");
                    return null;
                }
            }
            else
            {
                throw new NullReferenceException();
            }


        }


    }
}
