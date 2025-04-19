using NetDexTest_01.Models.Entities;

namespace NetDexTest_01.Services
{
    public interface IDebugRepository
    {

        Task<ICollection<Person>> ReadAllPeopleDebugAsync();
        Task<ICollection<PersonPerson>> ReadAllRelationsDebugAsync();
        Task<ICollection<ApplicationUser>> ReadAllUsersDebugAsync();
        Task<ICollection<DexHolder>> ReadAllDexHoldersDebugAsync();

        Task<ICollection<ApplicationUser>> ReadAbsolutelyAllDebugAsync();
    }
}
