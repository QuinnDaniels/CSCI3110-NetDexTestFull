using NetDexTest_01.Models.Entities;
using NetDexTest_01.Models.ViewModels;

namespace NetDexTest_01.Services
{

    /// <summary>
    /// Interface that defines the person repository.
    /// </summary>
    /// <remarks>
    /// 
    /// <code>IUserRepository.cs</code> has child files:
    /// 
    /// <list type="bullet">
    /// <listheader>
    /// <!--Contains children(partial) files:-->
    /// </listheader>
    /// <!--                -->
    /// <item>
    /// <term> IPersonRepository.ReadOne.cs </term>
    /// <description> ReadOne for DexHolder </description>
    /// </item>
    /// /// <!---->
    /// <item>
    /// <term> IPersonRepository.Create.cs </term>
    /// <description> Create - Primary working Create and Batch Creation methods. </description>
    /// </item></list>
    /// <!---->
    /// </remarks>
    public partial interface IPersonRepository
    {
        // create just a person // --> Create

        // create a person with ContactInfo, FullName, RecordCollector

        // create a P & Ci,Fn,Rc using Authorixation

        /// <summary>
        /// Returns a List of All People records, based on certain arguments. Has many overloads.
        /// </summary>
        /// <remarks>
        /// <para>
        /// NOTICE: Empty Parameter method returns all People REGARDLESS of
        /// any associated DexHolder! As such, it should only be called by
        /// Admin or Moderator methods.
        /// </para>
        /// <para> For All People by a SINGLE SPECIFIED USER, use and Overload </para>
        /// </remarks>
        /// <returns>A List of Person objects</returns>
        Task<ICollection<Person>> ReadAllPeopleAsync();
        /// <inheritdoc cref="ReadAllPeopleAsync()"/>
        Task<ICollection<Person>> ReadAllPeopleAsync(DexHolder dexHolder);
        /// <inheritdoc cref="ReadAllPeopleAsync()"/>
        Task<ICollection<Person>> ReadAllPeopleAsync(ApplicationUser user);
        /// <remarks>
        /// Uses a single string argument. Does a sequential check to find a DexHolder
        /// in the order of UserName -> UserId -> UserEmail
        /// </remarks>
        /// <inheritdoc cref="ReadAllPeopleAsync()"/>
        Task<ICollection<Person>?> ReadAllPeopleAsync(string inputString);
        /// <remarks>
        /// Directly uses DexHolderId to find a DexHolder to search using
        /// </remarks>
        /// <inheritdoc cref="ReadAllPeopleAsync()"/>
        Task<ICollection<Person>> ReadAllPeopleAsync(int dexHolderId);







        Task SaveChangesAsync();

        /// <inheritdoc cref="FindMatch(Person, Person, string)"/>
        bool FindMatch(PersonPerson ppIn);
        /// <inheritdoc cref="FindMatch(Person, Person, string)"></inheritdoc>
        bool FindMatch(PersonPerson ppIn, string desc);
        /// <inheritdoc cref="FindMatch(Person, Person, string)"></inheritdoc>
        bool FindMatch(Person p1, Person p2);
        /// <summary>
        /// Tool/Helper method made to be used by Initializer. Searches for a PersonPerson using .Any()
        /// </summary>
        /// <param name="ppIn">PersonPerson input. Will be stripped of RelationshipDescriptor if desc is not specified</param>
        /// <param name="p1">PersonParent</param>
        /// <param name="p2">PersonChild</param>
        /// <param name="desc">String that describes the relationship between Parent and Child</param>
        /// <returns>returns true if a match is found, false if not</returns>
        bool FindMatch(Person p1, Person p2, string desc);


        /// <summary>
        /// Helper/Tool method that cuts down on redundant code while adding PersonPerson to Parent and Child
        /// </summary>
        /// <remarks>
        /// <para>
        /// Does not contain checks for ppIn.RelationshipDescriptor!
        /// </para>
        /// <code>Must be followed with _personRepo.SaveChangesAsync !</code>
        /// </remarks>
        /// <param name="parent"></param>
        /// <param name="child"></param>
        /// <param name="ppIn"></param>
        /// <param name="desc"></param>
        void AddPersonPerson(Person parent, Person child, PersonPerson ppIn);
        
        /// <remarks>
        /// Like <seealso cref="AddPersonPerson(Person, Person, PersonPerson)" /> but includes a check if it exists
        /// </remarks>
        /// <inheritdoc cref="AddPersonPerson(Person, Person, PersonPerson)"></inheritdoc>
        bool AddPersonPersonCheck(Person parent, Person child, PersonPerson ppIn);
        /// <remarks>
        /// Like <seealso cref="AddPersonPersonCheck(Person, Person, PersonPerson)" /> but allows you to change the description
        /// </remarks>
        /// <inheritdoc cref="AddPersonPerson(Person, Person, PersonPerson)"></inheritdoc>
        bool AddPersonPersonCheck(Person parent, Person child, PersonPerson ppIn, string desc);

        /// <remarks>
        /// This one should work.... 4-17-2025 @ 12:16 PM
        /// </remarks>
        /// <inheritdoc cref="AddPersonPerson(Person, Person, PersonPerson)"></inheritdoc>

        bool AddPersonPersonCheck(PersonPerson ppIn, string desc);






        /*--------------------------------*/
        /// <inheritdoc cref="AddPersonPerson(Person, Person, PersonPerson)"></inheritdoc>
        Task AddPersonPersonAsync(Person parent, Person child, PersonPerson ppIn);
        /// <inheritdoc cref="AddPersonPerson(Person, Person, PersonPerson)"></inheritdoc>
        Task AddPersonPersonAsync(PersonPerson ppIn);
        /// <inheritdoc cref="AddPersonPersonCheck(Person, Person, PersonPerson)"></inheritdoc>
        Task<bool> AddPersonPersonCheckAsync(Person parent, Person child, PersonPerson ppIn);
        /// <inheritdoc cref="AddPersonPersonCheck(Person, Person, PersonPerson, string)"></inheritdoc>
        Task<bool> AddPersonPersonCheckAsync(Person parent, Person child, PersonPerson ppIn, string desc);
        /// <inheritdoc cref="AddPersonPersonCheck(PersonPerson, string)"></inheritdoc>
        Task<bool> AddPersonPersonCheckAsync(PersonPerson ppIn, string desc);


        Task AddPersonPersonForViewModel(string input, string nickname1, string nickname2, string desc);
        /*--------------------------------*/



        Task<ICollection<RelationshipVM>> GetAllRelationshipsAsync();
        Task<ICollection<RelationshipVM>?> GetAllRelationshipsByUserAsync(string input);
        Task<RelationshipVM?> GetOneRelationshipWithRequestAsync(RelationshipRequest relation);
        Task<ICollection<RelationshipVM>?> GetAllRelationshipsWithPeopleRequestAsync(RelationshipRequest relation);

        Task<Person?> GetOneByUserInputAsync(string input, string criteria);




        
        Task<bool> UpdateRelationshipBoolAsync(RelationshipRequest request);
        Task<bool> DeleteRelationshipBoolAsync(RelationshipRequest request);
        // Task<RelationshipVM> UpdateRelationshipVmAsync(RelationshipRequest request);
        // Task<RelationshipVM> DeleteRelationshipVmAsync(RelationshipRequest request);

        // Task<bool> UpdateRelationshipBoolAsync(RelationshipRequestExtend request);


        // read just a person // --> ReadOne

        // read all persons

        // read all persons by username

        // read all persons by userid


        // update/edit


        // delete a person
    }
}
