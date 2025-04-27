using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Models.ViewModels;
using NetDexTest_01.Services;
using NuGet.Protocol;


namespace NetDexTest_01.Services
{
    public partial class DbUserRepository : IUserRepository
    {

        public async Task<DexHolderMiddleVM?> GetDexHolderMiddleVMAsync(string input)
        {
            var user = await GetDexHolderByUserNameAsync(input);

            if (user == null)
            {
                user = await GetDexHolderByUserIdAsync(input);
                if (user == null)
                {
                    user = await GetDexHolderByEmailAsync(input);
                    if (user == null)
                    {
                        try
                        {
                            if(int.TryParse(input, out int idout))
                            {
                                user = await GetDexHolderByIntIdAsync(idout);
                            }
                        }
                        catch (Exception ex)
                        {
                            await _tools.ConOut("GetDexHolderByIntIdAsync EXCEPTION", ex);
                        }
                    }
                }
            } // end if
            if (user == null) return null;


            List<DexHolder> dexList = [user];

            var rolelist = await _db.Roles.ToListAsync();
            var userrolelist = await _db.UserRoles.ToListAsync();


            // SEE: DbUserRepository.UAReadAll.cs - ReadAllApplicationUsersVMAsync()
            //set up a query that manually joins User -> UserRoles -> Roles
            // because Users, UserRoles, and Roles do not contain navigation properties
            // (and I do not want to add them),
            // this query operates off of a join that contains a subquery
            var joinresult =
                         from appUser in dexList
                         join usrSub in (
                             from ur in userrolelist
                             join role in rolelist
                                on ur.RoleId equals role.Id into r
                             //.Where(r => ur.RoleId == r.Id).DefaultIfEmpty() into r
                             from role in r.DefaultIfEmpty()
                             select new
                             {
                                 userId = ur.UserId,
                                 roleId = r != null ? role.Id : "<No Role Id>",
                                 roleName = r != null ? role.Name : "<No Role Name>"
                             })
                         on appUser.ApplicationUserId equals usrSub.userId//.DefaultIfEmpty() // into grouping
                         //from usrSub in grouping.DefaultIfEmpty()
                         select new
                         {
                             Id = usrSub.userId,
                             UserName = appUser.ApplicationUserName,
                             RoleId = usrSub != null ? usrSub.roleId : "<No RoleId>",
                             RoleName = usrSub != null ? usrSub.roleName : "<No Role>"
                         };
            /*-- end joinresult --*/
            int errorCounter = -900;
            int errorCounter2 = -4040;
            //List<DexHolderHomeVM> DexWithRoles = new List<AdminUserVM>();
            try
            {
                _tools.Writer($"creating VM for: {user?.ApplicationUserName ?? "<x Username Error in generation!>"}");
                if (user == null) return null;
                DexHolderMiddleVM dexViewModel = new DexHolderMiddleVM()
                {
                    DexId = user?.Id ?? errorCounter--,
                    ApplicationUserId = user?.ApplicationUserId ?? "ERROR-AppUserId",
                    ApplicationUserName = user?.ApplicationUserName ?? "ERROR-AppUserName",
                    ApplicationEmail = user?.ApplicationUser?.Email ?? "ERROR-Email",
                    FirstName = user?.FirstName ?? "---",
                    MiddleName = user?.MiddleName ?? "---",
                    LastName = user?.LastName ?? "---",
                    Gender = user?.Gender ?? "---",
                    Pronouns = user?.Pronouns ?? "---",
                    // HACK - how do I set a default value if null for DateTime?
                    DateOfBirth = user?.DateOfBirth, //?? "---",
                    PeopleCount = user?.PeopleCount ?? errorCounter2--,
                    //People = user?.People
                    //create vm
                };

                List<PersonDexListVM> peopleForDex = new();

                
                if (user == null) return null;
                var taskPeople = Task.Run(() => user.People.ToList().ForEach(p =>
                {
                    if (p.DexHolderId == user.Id)
                    {
                        PersonDexListVM pdl = new()
                        {
                            Id =                    p.Id,
                            DexId =                 p.DexHolderId,
                            Nickname =              p.Nickname,
                            NameFirst =             p?.FullName?.NameFirst ?? "---",
                            NameMiddle =            p?.FullName?.NameMiddle ?? "---",
                            NameLast =              p?.FullName?.NameLast ?? "---",
                            PhNameFirst =           p?.FullName?.PhNameFirst ?? "---",
                            PhNameMiddle =          p?.FullName?.PhNameMiddle ?? "---",
                            PhNameLast =            p?.FullName?.PhNameLast ?? "---",
                            DateOfBirth =           p?.DateOfBirth, // ?? "---",
                            Gender =                p?.Gender ?? "---",
                            Pronouns =              p?.Pronouns ?? "---",
                            Rating =                p?.Rating ?? 0,
                            Favorite =              p?.Favorite ?? false,
                            RcEntryItemsCount =     p?.RecordCollector?.EntryItemsCount ?? errorCounter2 --,
                            CiSocialMediasCount =   p?.ContactInfo?.SocialMediasCount ?? errorCounter2 --,
                            PersonParentsCount =    p?.PersonParentsCount ?? errorCounter2 --,
                            PersonChildrenCount =   p?.PersonChildrenCount ?? errorCounter2 --,

                        };

                        peopleForDex.Add(pdl);
                    }
                }));



                // ready-up a new List<RoleVM> that we can populate
                List<RoleVM> rolesForUser = new();


                // iterate through the query result from earlier
                // if there is a result item that matches the
                // UserId of the current AppUser, add it to the list
                //
                //NOTE: This should be replaced later
                //var iterator = joinresult.AsEnumerable();
                //iterator.ForEach(p =>
                //var iterator = joinresult.AsEnumerable();
                var task = Task.Run(() => joinresult.ToList().ForEach(p =>
                {
                    if (p.Id == user.ApplicationUserId)
                    {
                        RoleVM rvm = new()
                        {
                            RoleName = p.RoleName
                        };
                        rolesForUser.Add(rvm);
                    }
                }));

                // add the list of RoleVMs back to the AdminUserVM object
                dexViewModel.Roles = rolesForUser;
                await Console.Out.WriteLineAsync($"\n\n\n{rolesForUser.ToJson().ToString()}\n\n\n");
                await Console.Out.WriteLineAsync($"\n\n\n{dexViewModel.Roles.ToJson().ToString()}\n\n\n");
                dexViewModel.People = peopleForDex;
                /*-------------- end LINQ foreach-----------------*/
                return dexViewModel;
            }
            catch (Exception ex)
            {
                await _tools.ConOut("Create DexHolderMiddleVM CONDITIONAL EXCEPTION", ex);
                return null;
            }
        }



    }
}
