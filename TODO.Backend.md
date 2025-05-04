# [CSCI3110-NetDexTestFull](/README.md)
 A repository that contains the  ASP.NET Web API backend project and the MVC frontend application. A pokedex for the people that I meet, revisited!

[_go to FrontEnd_](./TODO.Frontend.md)


[_home_](/README.md)
## NetDexTest-01 (Web API)
### BACKEND **TO-DO** LIST


- [x] AuthApiController

- [ ] AdminUserController
  - [x] &#x2714;Get Users
  - [x] &#x2714;Get Users(string id)
  - [x] &#x2714;? Insert User (ApplicationUser user)
  - [ ] Update User (string id, ApplicationUser user)
  - [ ] Delete User (string id)

- [ ] AdminDexHolderController
  - [ ] Get DexHolder
  - [ ] Get DexHolder(string id)
  - ~~[ ] ? Insert DexHolder (DexHolder User)~~
  - [ ] Update DexHolder (string id, DexHolder dexHolder)
  - [ ] Delete DexHolder (string id)

- ~~[ ] AdminPeopleController~~
  - ~~[ ] GetAllPeople~~
  - ~~[ ] GetAllPeople ()~~

- [ ] PeopleController
  - [x] &#x2714;Get People ()
  - ~~[ ] Get People (int id)~~ <-- (wait until id is not just an int)
  - [x] &#x2714;Get People (string nickname)
  - [x] &#x2714;Insert Person (Person person)
  - [ ] Update Person
    - [ ] (string nickname, Person updatedPerson)
    - [ ] (int pId, Person updatedPerson)
  - [ ] Delete Person
    - [ ] (int pId)
    - [ ] (string pId)
  
- [ ] AdminRecordCollectorController
    - [x] &#x2714;Get All Entries


- ~~[ ] EntryItemsController~~
    - [x] &#x2714;Get Entries
    - [x] &#x2714;Get Entries ()


***Misc***

- [ ] scrape the index from CSCI3110BoardGameAPI for API Endpoints
- [ ] add Guid Id as Id
- [ ] change: Person Pk: Id -> (Id, DexHolderId)





- at some point add local counter to the DTOs for social media and contact info before they get sent to the frontend

/*------------ drafted statement for roles -+++++-*/
var result = from usr in db.users 
             join usrSub in (
                    from ur in db.UserRoles
                    from role in db.Roles
                         .Where(r => ur.RoleId == role.Id).DefaultIfEmpty ()
                    select new
                         {
                               userId = ur.UserId,
                               roleId ?= role.Id,
                               roleName ?= role.Name
                         })
                on usr.UserId equals usrSub.UserId into grouping
                from usrSub in grouping.DefaultIfEmpty()
	      select new 
                  {
                        Id = usr.Id,
                        UserName = usr.UserName,
                        RoleId ?= usrSub.RoleId ?? "<No Role>" ,
                        RoleName ?= usrSub.RoleName ?? "<No Role>" 
                   };

var returner = from udh in usrlist 
           from ur in result.Where(r => r.Id == udh.Id). DefaultIfEmpty()
           select new
           {
                udh, ur // instead of this, prob iterate result
           }

/*---- in a LINQ foreach loop for each user (x) in usrlist ------*/
List<RoleVM> rolesForUser = new();
result.ToList().ForEach(p => 
      if(p.Id == x.Id)
      {
           RoleVM rvm = new()
             {
                  RoleName = p.RoleName
             }
           rolesForUser.Add(rvm);
       }
)
x.RolesList = rolesForUser;
/*-------------- end LINQ foreach-----------------*/