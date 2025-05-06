using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using NetDexTest_01.Models.Entities;
using NetDexTest_01.Models.ViewModels;

using System.Diagnostics;
using System.Linq.Expressions;

namespace NetDexTest_01.Services
{
    // TODO: do this!
    /// <summary>
    /// interacts with Extensions of Person (Ci, Fn, Rc)
    /// </summary>
    public partial class DbPersonRepository
    {
        public async Task<int> AddPersonPersonAsync(PersonPerson ppIn)
        {
            await Console.Out.WriteLineAsync("\n\n\n--------- AddPersonAsync -----------\n\n");
            await Console.Out.WriteLineAsync($"\n parent\t{ppIn.PersonParent.Nickname} \n");
            await Console.Out.WriteLineAsync($"\n child\t{ppIn.PersonChild.Nickname} \n");
            await Console.Out.WriteLineAsync($"\n PersonPerson:\t{ppIn.RelationshipDescription}");
            await Console.Out.WriteLineAsync($"\n PersonPerson:\t{ppIn.PersonParent.Nickname} -> {ppIn.PersonChild.Nickname}");

            int returner = -1;
            //TODO double check this
            ppIn.PersonParent.PersonParents.Add(ppIn);
            ppIn.PersonChild.PersonChildren.Add(ppIn);
            await Console.Out.WriteLineAsync("\n\n\n--------------------\n\n");
            returner = await SaveChangesAsync();
            await Console.Out.WriteLineAsync("\n\n\n---attempted to add. check results-------\n\n");

            await Console.Out.WriteLineAsync("\n\n\n--------------------\n\n");
            return returner;
        }


        public async Task AddPersonPersonAsync(Person parent, Person child, PersonPerson ppIn)
        {
            await Console.Out.WriteLineAsync("\n\n\n--------- AddPersonAsync -----------\n\n");
            await Console.Out.WriteLineAsync($"\n parent\t{parent.Nickname} \n");
            await Console.Out.WriteLineAsync($"\n child\t{child.Nickname} \n");
            await Console.Out.WriteLineAsync($"\n PersonPerson:\t{ppIn.RelationshipDescription}");
            await Console.Out.WriteLineAsync($"\n PersonPerson:\t{ppIn.PersonParent.Nickname} -> {ppIn.PersonChild.Nickname}");


            parent.PersonParents.Add(ppIn);
            child.PersonChildren.Add(ppIn);
            await Console.Out.WriteLineAsync("\n\n\n--------------------\n\n");
            await SaveChangesAsync();
            await Console.Out.WriteLineAsync("\n\n\n---attempted to add. check results-------\n\n");

            await Console.Out.WriteLineAsync("\n\n\n--------------------\n\n");

        }
        public async Task<bool> AddPersonPersonCheckAsync(Person parent, Person child, PersonPerson ppIn)
        {
            bool noMatch = !FindMatch(ppIn);
            if (noMatch)
            {
                await AddPersonPersonAsync(parent, child, ppIn);
            }
            return noMatch;
        }
        public async Task<bool> AddPersonPersonCheckAsync(Person parent, Person child, PersonPerson ppIn, string desc)
        {
            bool noMatch = !FindMatch(ppIn, desc);
            if (noMatch)
            {
                ppIn!.RelationshipDescription = desc;
                await AddPersonPersonAsync(parent, child, ppIn);
            }
            return noMatch;
        }
        public async Task<bool> AddPersonPersonCheckAsync(PersonPerson ppIn, string desc)
        {
            PersonPerson ppInNew = new PersonPerson(ppIn, desc);

            bool noMatch1 = !FindMatch(ppInNew);
            bool noMatch2 = !FindMatch(ppIn.PersonParent, ppIn.PersonChild, desc);
            if (noMatch2) // && noMatch1)
            {
                await AddPersonPersonAsync(ppIn.PersonParent, ppIn.PersonChild, ppInNew);
                return true;
            }
            else return false;
        }

        /*------------------------------------------------------------*/



        public void AddPersonPerson(Person parent, Person child, PersonPerson ppIn)
        {


            Console.WriteLine("\n\n\n--------- AddPerson -----------\n\n");
            Console.WriteLine($"\n parent\t{parent.Nickname} \n");
            Console.WriteLine($"\n child\t{child.Nickname} \n");
            Console.WriteLine($"\n PersonPerson:\t{ppIn.RelationshipDescription}");
            Console.WriteLine($"\n PersonPerson:\t{ppIn.PersonParent.Nickname} -> {ppIn.PersonChild.Nickname}");

            parent.PersonParents.Add(ppIn);
            child.PersonChildren.Add(ppIn);
            Console.WriteLine("\n\n\n--------------------\n\n");

        }

        public bool AddPersonPersonCheck(Person parent, Person child, PersonPerson ppIn)
        {
            bool noMatch = !FindMatch(ppIn);
            if (noMatch)
            {
                AddPersonPerson(parent, child, ppIn);
            }
            return noMatch;
        }

        public bool AddPersonPersonCheck(Person parent, Person child, PersonPerson ppIn, string desc)
        {
            bool noMatch = !FindMatch(ppIn, desc);
            if (noMatch)
            {
                ppIn!.RelationshipDescription = desc;
                AddPersonPerson(parent, child, ppIn);
            }
            return noMatch;
        }


        public bool AddPersonPersonCheck(PersonPerson ppIn, string desc)
        {
            PersonPerson ppInNew = new PersonPerson(ppIn, desc);

            bool noMatch1 = !FindMatch(ppInNew);
            bool noMatch2 = !FindMatch(ppIn.PersonParent, ppIn.PersonChild, desc);
            if (noMatch2) // && noMatch1)
            {
                AddPersonPerson(ppIn.PersonParent, ppIn.PersonChild, ppInNew);
                return true;
            }
            else return false;
        }






#region CREATE METHOD

        public async Task<int> AddPersonPersonForViewModel(string input, string nickname1, string nickname2, string? desc)
        {
            Person? p1 = null;
            Person? p2 = null;

            p1 = await GetPersonByNickName(PropertyField.username, input, nickname1);
            if (p1 == null)
            {
                p1 = await GetPersonByNickName(PropertyField.email, input, nickname1);
                if (p1 == null)
                {
                    p1 = await GetPersonByNickName(PropertyField.id, input, nickname1);
           
                }
            }

            p2 = await GetPersonByNickName(PropertyField.username, input, nickname2);
            if (p2 == null)
            {
                p2 = await GetPersonByNickName(PropertyField.email, input, nickname2);
                if (p2 == null)
                {
                    p2 = await GetPersonByNickName(PropertyField.id, input, nickname2);
                }
            }

            if (p1 != null && p2 != null) 
            {
                    bool noMatch2 = !FindMatch(p1, p2, desc);
                    if (noMatch2)
                    {
                        PersonPerson ppInNew = new PersonPerson(p1, p2, desc);
                        try
                        {
                            return await AddPersonPersonAsync(ppInNew);

                        }
                        catch (Exception ex)
                        {

                            await Console.Out.WriteLineAsync($"\n\n--------------\nCould not add relation to database\n-----------\n{ex.Message}\n\n---------------");
                            return -1;    
                        }
                        
                        //return true;
                    }
                    //else return false;
            }
            return -1;
        }

#endregion


        public async Task<ICollection<RelationshipVM>> GetAllRelationshipsAsync()
        {
            var peoplepeople = await _db.PersonPerson
                    .Include(pp => pp.PersonParent)
                        .ThenInclude(pa => pa.FullName)
                    .Include(pp => pp.PersonParent)
                        .ThenInclude(pa => pa.DexHolder)
                            .ThenInclude(dh => dh.ApplicationUser)
                    .Include(pp => pp.PersonChild)
                        .ThenInclude(pc => pc.FullName)
                    .Include(pp => pp.PersonChild)
                        .ThenInclude(pc => pc.DexHolder)
                            .ThenInclude(dh => dh.ApplicationUser)
                .ToListAsync();
            
            List<RelationshipVM> relationshipVMs = new List<RelationshipVM>();

            peoplepeople.ForEach(async p =>
            {
                if(p.PersonParent.DexHolder.ApplicationUser.Email == p.PersonChild.DexHolder.ApplicationUser.Email)
                {
                    RelationshipVM rel = new RelationshipVM()
                    {
                        AppEmail = p.PersonParent.DexHolder.ApplicationUser.Email,
                        Id = p.Id,
                        AppUsername = p.PersonParent.DexHolder.ApplicationUserName,
                        PersonParentId = p.PersonParentId,
                        ParentNickname = p.PersonParent.Nickname,
                        RelationshipDescription = p.RelationshipDescription,
                        PersonChildId = p.PersonChildId,
                        ChildNickname = p.PersonChild.Nickname,
                        LastUpdated = p.LastUpdated
                    };
                    relationshipVMs.Add(rel);
                    
                }
                else
                {
                    await Console.Out.WriteLineAsync("\n\n\n------EMAIL MISMATCH---------\n\n\n");
                }

            } );

            return relationshipVMs;
        }

        public async Task<ICollection<RelationshipVM>?> GetAllRelationshipsByUserAsync(string input)
        {
            ApplicationUser? user = null;
            user = await _userRepo.GetByEmailAsync(input);

            if (user == null)
            {
                user = await _userRepo.GetByUsernameAsync(input);
                if (user == null)
                {
                    user = await _userRepo.GetByIdAsync(input);
                }
            }

            if (user != null)
            { 
                var peoplepeople = await _db.PersonPerson
                        .Include(pp => pp.PersonParent)
                            .ThenInclude(pa => pa.FullName)
                        .Include(pp => pp.PersonParent)
                            .ThenInclude(pa => pa.DexHolder)
                                .ThenInclude(dh => dh.ApplicationUser)
                        .Include(pp => pp.PersonChild)
                            .ThenInclude(pc => pc.FullName)
                        .Include(pp => pp.PersonChild)
                            .ThenInclude(pc => pc.DexHolder)
                                .ThenInclude(dh => dh.ApplicationUser)
                        .Where(pp => (pp.PersonParent.DexHolder.ApplicationUser == user)
                                    && (pp.PersonChild.DexHolder.ApplicationUser == user))
                    .ToListAsync();

                List<RelationshipVM> relationshipVMs = new List<RelationshipVM>();

                peoplepeople.ForEach(async p =>
                {
                    if (p.PersonParent.DexHolder.ApplicationUser.Email == p.PersonChild.DexHolder.ApplicationUser.Email)
                    {
                        RelationshipVM rel = new RelationshipVM()
                        {
                            AppEmail = p.PersonParent.DexHolder.ApplicationUser.Email,
                            Id = p.Id,
                            AppUsername = p.PersonParent.DexHolder.ApplicationUserName,
                            PersonParentId = p.PersonParentId,
                            ParentNickname = p.PersonParent.Nickname,
                            RelationshipDescription = p.RelationshipDescription,
                            PersonChildId = p.PersonChildId,
                            ChildNickname = p.PersonChild.Nickname,
                            LastUpdated = p.LastUpdated
                        };
                        relationshipVMs.Add(rel);

                    }
                    else
                    {
                        await Console.Out.WriteLineAsync("\n\n\n------EMAIL MISMATCH---------\n\n\n");
                    }
                });
                return relationshipVMs;
            }
            else
            {
                return null;
            }
        }





        public async Task<RelationshipVM?> GetOneRelationshipWithRequestAsync(RelationshipRequest relation)
        {
            var relationshipVMs = await GetAllRelationshipsByUserAsync(relation.input);

            if (relationshipVMs != null)
            {
                RelationshipVM? relationshipOut = null;
                IEnumerable<RelationshipVM>? relList = null;
                //relationshipOut = relationshipVMs
                //    .FirstOrDefault(rvm => rvm.ParentNickname == relation.nicknameOne
                //            && rvm.ChildNickname == relation.nicknameTwo
                //            && rvm.RelationshipDescription == relation.description);
                bool failure = false;





                relList = relationshipVMs.Where(rvm => rvm.ParentNickname == relation.nicknameOne);

                if (relList == null || !relList.Any()) failure = true;

                if (failure != true) relList = relList.Where(rvm => rvm.ChildNickname == relation.nicknameTwo);

                if (failure != true && relList == null || !relList.Any()) failure = true;

                if(failure != true) relationshipOut = relList.FirstOrDefault(rvm => rvm.RelationshipDescription == relation.description);


                if (relationshipOut != null)
                {
                    return relationshipOut;
                }
                else
                {
                    await Console.Out.WriteLineAsync("\n\n\nERROR: relationship not found, double check criteria!!!\n\n");
                    return null;
                }
            }
            else
            {
                await Console.Out.WriteLineAsync($"\n\n\nERROR: relationships not found for the selected user input, [{relation.input}] \n\n");
                return null;
            }

        }



        //TODO - add if nickname1 == null branches
        public async Task<ICollection<RelationshipVM>?> GetAllRelationshipsWithPeopleRequestAsync(RelationshipRequest relation)
        {
            var relationshipVMs = await GetAllRelationshipsByUserAsync(relation.input);

            if (relationshipVMs != null)
            {
                List<RelationshipVM>? relationshipsOut = null;


                // NOTE - Thank you Tarnoff for teaching truth tables
                if (relation.nicknameOne != null && relation.nicknameTwo == null && relation.description == null)
                {
                    relationshipsOut = relationshipVMs
                        .Where(rvm => rvm.ParentNickname == relation.nicknameOne
                            //&& rvm.RelationshipDescription == relation.description
                            //&& rvm.ChildNickname == relation.nicknameTwo
                            ).ToList();


                }
                else if (relation.nicknameOne != null && relation.nicknameTwo != null && relation.description == null)
                {
                    relationshipsOut = relationshipVMs
                        .Where(rvm => rvm.ParentNickname == relation.nicknameOne
                                //&& rvm.RelationshipDescription == relation.description
                                && rvm.ChildNickname == relation.nicknameTwo).ToList();
                }
                else if (relation.nicknameOne != null && relation.nicknameTwo == null && relation.description != null)
                {
                    relationshipsOut = relationshipVMs
                        .Where(rvm => rvm.ParentNickname == relation.nicknameOne
                            && rvm.RelationshipDescription == relation.description
                            //&& rvm.ChildNickname == relation.nicknameTwo
                            ).ToList();


                }
                else if (relation.nicknameOne != null && relation.nicknameTwo != null && relation.description != null)
                {
                    relationshipsOut = relationshipVMs
                        .Where(rvm => rvm.ParentNickname == relation.nicknameOne
                                && rvm.RelationshipDescription == relation.description
                                && rvm.ChildNickname == relation.nicknameTwo).ToList();
                }
                else if (relation.nicknameOne == null && relation.nicknameTwo == null && relation.description == null)
                {
                    relationshipsOut = relationshipVMs
                        //.Where(rvm => rvm.ParentNickname == relation.nicknameOne
                        //    //&& rvm.RelationshipDescription == relation.description
                        //    //&& rvm.ChildNickname == relation.nicknameTwo
                        //    )
                        .ToList();


                }
                else if (relation.nicknameOne == null && relation.nicknameTwo != null && relation.description == null)
                {
                    relationshipsOut = relationshipVMs
                        .Where(rvm => //vm.ParentNickname == relation.nicknameOne
                                //&& rvm.RelationshipDescription == relation.description
                                //&&
                                rvm.ChildNickname == relation.nicknameTwo).ToList();
                }
                else if (relation.nicknameOne == null && relation.nicknameTwo == null && relation.description != null)
                {
                    relationshipsOut = relationshipVMs
                        .Where(rvm => //rvm.ParentNickname == relation.nicknameOne
                            //&&
                            rvm.RelationshipDescription == relation.description
                            //&& rvm.ChildNickname == relation.nicknameTwo
                            ).ToList();


                }
                else if (relation.nicknameOne == null && relation.nicknameTwo != null && relation.description != null)
                {
                    relationshipsOut = relationshipVMs
                        .Where(rvm => //rvm.ParentNickname == relation.nicknameOne
                                //&& 
                                rvm.RelationshipDescription == relation.description
                                && rvm.ChildNickname == relation.nicknameTwo).ToList();
                }





                if (relationshipsOut != null)
                {
                    return relationshipsOut;
                }
                else
                {
                    await Console.Out.WriteLineAsync("\n\n\nERROR: relationships not found!!!\n\n");
                    return null;
                }
            }
            else
            {
                await Console.Out.WriteLineAsync($"\n\n\nERROR: relationships not found for the selected user input, [{relation.input}] \n\n");
                return null;
            }

        }





        public bool FindMatch(PersonPerson ppIn)
        {
            string desc = string.Empty;
            if (ppIn.RelationshipDescription != null && ppIn.RelationshipDescription != string.Empty)
            {
                desc = ppIn.RelationshipDescription;
            }
            bool outTf = FindMatch(ppIn, desc);
            return outTf;

        }
        public bool FindMatch(PersonPerson ppIn, string desc)
        {
            Person p1 = ppIn.PersonParent;
            Person p2 = ppIn.PersonChild;

            bool outTf = FindMatch(p1, p2, desc);
            return outTf;

        }
        public bool FindMatch(Person p1, Person p2)
        {
            string desc = string.Empty;
            bool outTf = FindMatch(p1, p2, desc);
            return outTf;

        }
        public bool FindMatch(Person p1, Person p2, string? desc)
        {
            bool outTf = _db.PersonPerson
                            .Any(pp => pp.PersonParent == p1
                                    && pp.PersonChild == p2
                                    && pp.RelationshipDescription == desc);
            return outTf;
        }






        public async Task<bool> UpdateRelationshipBoolAsync(RelationshipRequestUpdate request)
        {
            var finder = request.getExistingRequestInstance();
            var relationVM = await GetOneRelationshipWithRequestAsync(finder);

            if (relationVM == null) return false;

            var relation = await _db.PersonPerson
            .Include(pp => pp.PersonParent)
            .Include(pp => pp.PersonChild)
            .ThenInclude(pc => pc.DexHolder)
            .FirstOrDefaultAsync(pp => pp.PersonParent.Nickname == relationVM.ParentNickname
                && pp.PersonChild.Nickname == relationVM.ChildNickname
                && pp.RelationshipDescription == relationVM.RelationshipDescription
                && pp.PersonParent.DexHolder.ApplicationUserName == relationVM.AppUsername
                );

            if (relation == null) return false;

            string input = request.input;
            string nickname1 = request.nicknameOne;
            string newNickname2 = request.nickname2;
            string? desc = request.newDescription;
            bool wasDeleted = false;
            try
            {
                if (!(await DeleteRelationshipBoolAsync(relation)))
                {
                    await Console.Out.WriteLineAsync($"\n\nAn error occurred while trying to modify the database\n\tERROR: Could not remove the relation from the database!");
                    return false;
                }
                else 
                {
                    await Console.Out.WriteLineAsync($"\n\nModified the database. Removed Relation from database.\n\tSetting flag to true");
                    wasDeleted = true;
                }
                
                if ((await AddPersonPersonForViewModel(input, nickname1, newNickname2, desc)) < 0)
                { 
                    if (wasDeleted)
                    {
                        await Console.Out.WriteLineAsync($"\n\nSecond add failed. Attempting to re-add the relation to the database.");
                        if(!(await AddPersonPersonCheckAsync(relation, relation.RelationshipDescription))) throw new NotImplementedException("Could not re-add the relation to the database.");
                        await Console.Out.WriteLineAsync($"\n\tAttempt to re-add the relation to the database succeeded!");
                    }
                    return false;
                }


                var newRelationship = await _db.PersonPerson
                    .Include(pp => pp.PersonParent)
                    .Include(pp => pp.PersonChild)
                    .FirstOrDefaultAsync(pp => pp.PersonParent.Nickname == nickname1
                        && pp.PersonChild.Nickname == newNickname2
                        && pp.RelationshipDescription == desc
                        );
                if (newRelationship == null) return false;
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync($"\n\nAn error occurred while trying to modify the database\n");
                await Console.Out.WriteLineAsync($"\n\n{ex.Message}\n");
            }

            //relation.RelationshipDescription = request.description;
            // relation.RelationshipType = request.RelationshipType;
            // relation.RelationshipNote = request.RelationshipNote;

            return true;
        }

        public async Task<bool> UpdateRelationshipBoolAsync(RelationshipRequest request)
        {
            var relation = await _db.PersonPerson
                .Include(pp => pp.PersonParent)
                .Include(pp => pp.PersonChild)
                
                .FirstOrDefaultAsync(pp => pp.PersonParent.Nickname == request.nicknameOne
                    && pp.PersonChild.Nickname == request.nicknameTwo
                    //&& pp.RelationshipDescription == request.description
                    );

            if (relation == null) return false;

            relation.RelationshipDescription = request.description;
            //relation.RelationshipType = request.RelationshipType;
            //relation.RelationshipNote = request.RelationshipNote;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRelationshipBoolAsync(RelationshipRequest request)
        {
            var relation = await _db.PersonPerson
                .Include(pp => pp.PersonParent)
                .Include(pp => pp.PersonChild)

                .FirstOrDefaultAsync(pp => pp.PersonParent.Nickname == request.nicknameOne
                    && pp.PersonChild.Nickname == request.nicknameTwo
                    //&& pp.RelationshipDescription == request.description
                    );

            if (relation == null) return false;

            _db.PersonPerson.Remove(relation);
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteRelationshipBoolAsync(PersonPerson request)
        {
            var relation = await _db.PersonPerson
                .Include(pp => pp.PersonParent)
                .Include(pp => pp.PersonChild)

                .FirstOrDefaultAsync(pp => pp.PersonChildId == request.PersonChildId
                    && pp.PersonParentId == request.PersonParentId
                    && pp.RelationshipDescription == request.RelationshipDescription
                    );

            if (relation == null) return false;

            _db.PersonPerson.Remove(relation);
            await _db.SaveChangesAsync();
            return true;
        }


        // public async Task<RelationshipVM?> UpdateRelationshipVmAsync(RelationshipRequest request)
        // {
        //     var relation = await _db.PersonPerson
        //         .Include(pp => pp.PersonParent)
        //         .Include(pp => pp.PersonChild)

        //         .FirstOrDefaultAsync(pp => pp.PersonParent.Nickname == request.nicknameOne
        //             && pp.PersonChild.Nickname == request.nicknameTwo
        //             //&& pp.RelationshipDescription == request.description
        //             );

        //     if (relation == null) return null;

        //     relation.RelationshipDescription = request.description;
        //     //relation.RelationshipType = request.RelationshipType;
        //     //relation.RelationshipNote = request.RelationshipNote;

        //     await _db.SaveChangesAsync();

        //     var output = await _db.PersonPerson
        //         .Include(pp => pp.PersonParent)
        //         .Include(pp => pp.PersonChild)
        //         .FirstOrDefaultAsync(pp => pp.PersonParent.Nickname == request.nicknameOne
        //             && pp.PersonChild.Nickname == request.nicknameTwo
        //         //&& pp.RelationshipDescription == request.description
        //         );




        //     return true;
        // }

        // public async Task<RelationshipVM?> DeleteRelationshipVmAsync(RelationshipRequest request)
        // {
        //     var relation = await _db.PersonPerson
        //         .Include(pp => pp.PersonParent)
        //         .Include(pp => pp.PersonChild)

        //         .FirstOrDefaultAsync(pp => pp.PersonParent.Nickname == request.nicknameOne
        //             && pp.PersonChild.Nickname == request.nicknameTwo
        //             //&& pp.RelationshipDescription == request.description
        //             );

        //     if (relation == null) return null;

        //     _db.PersonPerson.Remove(relation);
        //     await _db.SaveChangesAsync();
        //     return true;
        // }








    }
}
