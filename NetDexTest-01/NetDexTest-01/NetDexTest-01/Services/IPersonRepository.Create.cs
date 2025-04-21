namespace NetDexTest_01.Services
{
    // TODO: do this!
    /// <summary>
    /// interacts with Extensions of Person (Ci, Fn, Rc)
    /// </summary>
    public partial interface IPersonRepository
    {
        void CreatePersonAsync();
        Task<Person?> CreatePersonAsync(ApplicationUser user, string personNickname);
        Task<Person?> CreatePersonAsync(DexHolder dex, string personNickname);
        Task<Person?> CreatePersonAsync(PropertyField pType, string inputProperty, string personNickname);
        Task<Person?> CreatePersonAsync(PropertyField pType, string inputProperty, Person person);
    }
}
