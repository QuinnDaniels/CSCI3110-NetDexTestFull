# [CSCI3110-NetDexTestFull](/README.md)
 A repository that contains the  ASP.NET Web API backend project and the MVC frontend application. A pokedex for the people that I meet, revisited!

[_go to FrontEnd_](./TODO.Frontend.md)


[_home_](/README.md)
## NetDexTest-01 (Web API)
### BACKEND **TO-DO** LIST


- [ ] AuthApiController

- [ ] AdminUserController
  - [ ] Get Users
  - [ ] Get Users(string id)
  - [ ] ? Insert User (ApplicationUser user)
  - [ ] Update User (string id, ApplicationUser user)
  - [ ] Delete User (string id)

- [ ] AdminDexHolderController
  - [ ] Get DexHolder
  - [ ] Get DexHolder(string id)
  - [ ] ? Insert DexHolder (DexHolder User)
  - [ ] Update DexHolder (string id, DexHolder dexHolder)
  - [ ] Delete DexHolder (string id)

- [ ] AdminPeopleController
  - [ ] GetAllPeople
  - [ ] GetAllPeople ()

- [ ] PeopleController
  - [ ] Get People ()
  - [ ] ~~Get People (int id)~~ <-- (wait until id is not just an int)
  - [ ] Get People (string nickname)
  - [ ] Insert Person (Person person)
  - [ ] Update Person
    - [ ] (string nickname, Person updatedPerson)
    - [ ] (int pId, Person updatedPerson)
  - [ ] Delete Person
    - [ ] (int pId)
    - [ ] (string pId)
  
- [ ] AdminRecordCollectorController
    - [ ] Get All Entries


- [ ] EntryItemsController
    - [ ] Get Entries
    - [ ] Get Entries ()


***Misc***

- [ ] scrape the index from CSCI3110BoardGameAPI for API Endpoints
- [ ] add Guid Id as Id
- [ ] change: Person Pk: Id -> (Id, DexHolderId)
