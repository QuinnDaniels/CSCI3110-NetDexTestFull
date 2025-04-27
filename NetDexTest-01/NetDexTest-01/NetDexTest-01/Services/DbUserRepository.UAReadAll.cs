using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Models.ViewModels;
using NetDexTest_01.Services;
using System.Data;
using static NetDexTest_01.Constants.Authorization;

namespace NetDexTest_01.Services
{




    //public class AdminRolesVM
    //{
    //    public string Id { get; set; }
    //    public string RoleName { get; set; }
    //}




    // assisted partially with chatGPT
    public partial class DbUserRepository : IUserRepository
    {
        public async Task<ICollection<AdminUserVM>> ReadAllApplicationUsersVMAsync()
        {

             var userlist = await _db.Users
                    .Include(u => u.DexHolder)
                        .ThenInclude(dh => dh.People)
                    .ToListAsync();

            /*
            //_db.UserRoles
            //    .Include(ur => ur.)

            //var query =
            //    from role in _db.Roles
            //    join userRole in _db.UserRoles on userRole.RoleName equals role.RoleName into ur
            //    from userRole in ur.DefaultIfEmpty()
            //    select new
            //    {
            //        RoleName = role.RoleName,
            //        IsInRole = userRole != null
            //    };
            //var roles = context.Roles
            //    .SelectMany(r => context.UserRoles.Where(ur => ur.RoleName == r.RoleName
            //     && ur.UserName == userName).DefaultIfEmpty(),
            //     (r, ur) => new
            //     {
            //         RoleName = r.RoleName,
            //         IsInRole = ur != null
            //     }).ToList();
             */


            var rolelist = await _db.Roles.ToListAsync();
            var userrolelist = await _db.UserRoles.ToListAsync();
            /*
            //IEnumerable<UserUserRolesVM> queryUrR =
            //    from userRole in _db.UserRoles.ToList()
            //    join role in _db.Roles.ToList() on userRole.RoleId equals role.Id into r
            //    from role in r.DefaultIfEmpty()
            //    select new UserUserRolesVM()
            //    {
            //        UserId = userRole.UserId,
            //        RoleId = r != null ? role.Id : "<No Role Id>",
            //        RoleName = role?.Name ?? "<No Role Name>"
            //        //r != null ? role.Name : "<No Role Name>"
            //    };
            //.ToListAsync();
             */

            // left join reference - https://stackoverflow.com/a/54715309
            // linq reference - https://learn.microsoft.com/en-us/dotnet/csharp/linq/standard-query-operators/join-operations
            // complex linq reference - https://learn.microsoft.com/en-us/ef/core/querying/complex-query-operators
            // linq with multiple left joins reference - https://stackoverflow.com/a/10145808


            //set up a query that manually joins User -> UserRoles -> Roles
            // because Users, UserRoles, and Roles do not contain navigation properties
            // (and I do not want to add them),
            // this query operates off of a join that contains a subquery
            var joinresult =
                         from appUser in userlist
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
                         on appUser.Id equals usrSub.userId//.DefaultIfEmpty() // into grouping
                         //from usrSub in grouping.DefaultIfEmpty()
                         select new
                         {
                             Id = usrSub.userId,
                             UserName = appUser.UserName,
                             RoleId = usrSub != null ? usrSub.roleId : "<No RoleId>",
                             RoleName = usrSub != null ? usrSub.roleName : "<No Role>"
                         };
            // END var result

            int errorCounter = -900;
            int errorCounter2 = -4040;
            List<AdminUserVM> UsersWithRoles = new List<AdminUserVM>();
            // loop through established list of users
            userlist.ForEach( async x =>
            {
                try
                {
                    // create a new VM object, and add properties from ApplicationUser to the new VM
                    // List<RoleVM> Roles is created automatically by default and is empty, but it will get populated later
                    _tools.Writer($"creating VM for: {x?.UserName ?? "<x Username Error in generation!>"}");
                    AdminUserVM userAdminModel = new AdminUserVM()
                    {
                        Id = x.Id,
                        UserName = x?.UserName ?? "ERROR-UserName",
                        Email = x?.Email ?? "ERROR-Email",
                        DexHolderId = x?.DexHolder?.Id ?? errorCounter--,
                        FirstName = x?.DexHolder?.FirstName ?? "---",
                        LastName = x?.DexHolder?.LastName ?? "---",
                        AccessFailedCount = x.AccessFailedCount,
                        PeopleCount = x?.DexHolder?.PeopleCount ?? errorCounter2--
                };
                 _tools.Writer($"VM : {x?.UserName ?? "Error!"}\nDH: {x?.DexHolder?.Id ?? errorCounter - 1}");

                /*
                //var returner = from udh in usrlist
                //               from ur in result.Where(r => r.Id == udh.Id).DefaultIfEmpty()
                //               select new
                //               {
                //                   udh,
                //                   ur // instead of this, prob iterate result
                //               }
                 */
                /*---- in a LINQ foreach loop for each user (x) in usrlist ------*/

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
                var task = Task.Run(() =>  joinresult.ToList().ForEach(p =>
                {
                    if (p.Id == x.Id)
                    {
                        RoleVM rvm = new()
                        {
                            RoleName = p.RoleName
                        };
                        rolesForUser.Add(rvm);
                    }
                }));

                // add the list of RoleVMs back to the AdminUserVM object
                userAdminModel.Roles = rolesForUser;
                /*-------------- end LINQ foreach-----------------*/
                /*
                //List<AdminUserVM> rolesByUser = 
                //var query = 
                //    from role in _db.Roles
                //    join userRole in _db.UserRoles on role.Id equals userRole.RoleId into ur
                //    join user in _db.Users on UserId equals user.Id into u
                //    from userRole in ur.DefaultIfEmpty()
                //    from user in u.DefaultIfEmpty()
                //    select new
                //    {


                //        RoleName = role.RoleName,
                //        IsInRole = userRole != null
                //    };
                 */

                // add the AdminUserVM object to the list that was prepped earlier
                UsersWithRoles.Add(userAdminModel);
            }
            catch (Exception ex)
            {
                await _tools.ConOut("UserAdminVM LOOP EXCEPTION", ex);
            }
            });


            UsersWithRoles.OrderBy(u => u.DexHolderId);

            return UsersWithRoles;

        }

        public async Task<ICollection<ApplicationUser>> ReadAllUserDexPeopleAsync()
        {
            return await _db.Users
                .Include(u => u.DexHolder)
                    .ThenInclude(dh => dh.People)
                .ToListAsync();
        }

    }

    // https://stackoverflow.com/a/52586464

    /// <summary>
    /// https://stackoverflow.com/a/13503860
    /// </summary>
    internal static class MyExtensions
    {
        internal static IEnumerable<TResult> FullOuterGroupJoin<TA, TB, TKey, TResult>(
            this IEnumerable<TA> a,
            IEnumerable<TB> b,
            Func<TA, TKey> selectKeyA,
            Func<TB, TKey> selectKeyB,
            Func<IEnumerable<TA>, IEnumerable<TB>, TKey, TResult> projection,
            IEqualityComparer<TKey> cmp = null)
        {
            cmp = cmp ?? EqualityComparer<TKey>.Default;
            var alookup = a.ToLookup(selectKeyA, cmp);
            var blookup = b.ToLookup(selectKeyB, cmp);

            var keys = new HashSet<TKey>(alookup.Select(p => p.Key), cmp);
            keys.UnionWith(blookup.Select(p => p.Key));

            var join = from key in keys
                       let xa = alookup[key]
                       let xb = blookup[key]
                       select projection(xa, xb, key);

            return join;
        }

        internal static IEnumerable<TResult> FullOuterJoin<TA, TB, TKey, TResult>(
            this IEnumerable<TA> a,
            IEnumerable<TB> b,
            Func<TA, TKey> selectKeyA,
            Func<TB, TKey> selectKeyB,
            Func<TA, TB, TKey, TResult> projection,
            TA defaultA = default(TA),
            TB defaultB = default(TB),
            IEqualityComparer<TKey> cmp = null)
        {
            cmp = cmp ?? EqualityComparer<TKey>.Default;
            var alookup = a.ToLookup(selectKeyA, cmp);
            var blookup = b.ToLookup(selectKeyB, cmp);

            var keys = new HashSet<TKey>(alookup.Select(p => p.Key), cmp);
            keys.UnionWith(blookup.Select(p => p.Key));

            var join = from key in keys
                       from xa in alookup[key].DefaultIfEmpty(defaultA)
                       from xb in blookup[key].DefaultIfEmpty(defaultB)
                       select projection(xa, xb, key);

            return join;
        }
    }
}
