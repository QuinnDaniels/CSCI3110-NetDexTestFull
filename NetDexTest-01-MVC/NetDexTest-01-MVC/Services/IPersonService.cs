using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetDexTest_01_MVC.Models;
using NetDexTest_01_MVC.Models.ViewModels;
using NetDexTest_01_MVC.Models.Entities;
using NetDexTest_01_MVC.Models.Authentication;


namespace NetDexTest_01_MVC.Services
{
    public interface IPersonService
    {
        Task<PeopleResponse> CreatePersonAsync(Person person);
        Task<PeopleResponse> CreatePersonAsync(NewPersonVM person);
        Task<DexHolderMiddleVM?> GetDexHolderMiddleVMAsync(string input);
    }
}