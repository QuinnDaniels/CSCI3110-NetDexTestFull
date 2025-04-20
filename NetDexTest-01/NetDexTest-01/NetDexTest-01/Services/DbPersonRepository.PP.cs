namespace NetDexTest_01.Services
{
    // TODO: do this!
    /// <summary>
    /// interacts with Extensions of Person (Ci, Fn, Rc)
    /// </summary>
    public partial class DbPersonRepository
    {
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
        public bool FindMatch(Person p1, Person p2, string desc)
        {
            bool outTf = _db.PersonPerson
                            .Any(pp => pp.PersonParent == p1
                                    && pp.PersonChild == p2
                                    && pp.RelationshipDescription == desc);
            return outTf;
        }

    }
}
