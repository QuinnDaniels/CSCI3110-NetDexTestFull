using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;
using System.Security.Cryptography.Pkcs;
using NetDexTest_01_MVC.Models.Entities;

namespace NetDexTest_01_MVC.Models.ViewModels
{
    /// <summary>
    /// A view model that is used in <see cref="DbUserRepository.ReadAllApplicationUsersVMAsync" />. 
    /// </summary>
    /// <remarks>
    /// [user.username]
    /// [user.email]
    /// [user.accessFailCount]
    /// [user.DexHolder.Id]
    /// [user.DexHolder.FirstName]
    /// [user.DexHolder.LastName]
    /// </remarks>

    public class AdminUserVMResponseString
    {

        ///// <summary>
        ///// a factory method.<br />
        ///// 
        ///// - gets discussed in software engineering, design patterns.<br />
        ///// 
        ///// - all factory methods do is create things
        ///// </summary>
        ///// <returns></returns>
        //public Person GetPersonInstance()
        //{
        //    return new Person
        //    {
        //        Id = 0,
        //        DexHolderId = 0,
        //        Nickname = this.Nickname,   //this.Nickname is passed from constructor;  Nickname is what will be the property of the new person
        //        DateOfBirth = this.DateOfBirth,
        //        Gender = this.Gender,
        //        Pronouns = this.Pronouns,
        //        Rating = this.Rating,
        //        Favorite = this.Favorite
        //    };
        //}

        public HttpStatusCode Status { get; set; }
        public string? Message { get; set; } = "default message";
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public int? DexHolderId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? AccessFailedCount { get; set; }
        public int? PeopleCount { get; set; }
        public string? Roles { get; set; }
    }



    public class AdminUserRootResponse
    {
        public HttpStatusCode Status { get; set; }
        public string? Message { get; set; } = "default message";
        public List<AdminUserVM>? AdminUserVMs { get; set; }

    }
    public class AdminUserVMResponse
    {

        public HttpStatusCode Status { get; set; }
        public string? Message { get; set; } = "default message";
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public int? DexHolderId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? AccessFailedCount { get; set; }
        public int? PeopleCount { get; set; }
        public List<RoleVM>? Roles { get; set; }
    }








    public class AdminUserVM
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public int? DexHolderId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? AccessFailedCount { get; set; }
        public int? PeopleCount { get; set; }
        public List<RoleVM>? Roles { get; set; } //= new List<RoleVM>();

        public AdminUserVM() { }

        public AdminUserVM(AdminUserVMResponseString x)
        {
            this.Id = x.Id;
            this.UserName = x.UserName;
            this.Email = x.Email;
            this.DexHolderId = x.DexHolderId;
            this.FirstName = x.FirstName;
            this.LastName = x.LastName;
            this.AccessFailedCount = x.AccessFailedCount;
            this.PeopleCount = x.PeopleCount;

            string? feeder = x.Roles;
            if (feeder == null) feeder = "";
            string[] words = feeder.Split('+');

            List<string> rolesOut = [.. words];

            rolesOut.RemoveAt(rolesOut.IndexOf(rolesOut.Last()));
            List<RoleVM> rvm = new();
            foreach(string role in rolesOut)
            {
                rvm.Add(new RoleVM(role));
            }

            this.Roles = rvm;

        }
    }





    //public class UserAdminAllResponse
    //{
    //    public HttpStatusCode Status { get; set; }
    //    public string? Message { get; set; } = "default message";
    //    public string? Email { get; set; }
    //    public string? AccessToken { get; set; }
    //    public string? RefreshToken { get; set; }
    //    public string? Roles { get; set; }

    //}

}
