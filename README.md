# CSCI3110-NetDexTestFull
 A repository that contains the  ASP.NET Web API backend project and the MVC frontend application. A pokedex for the people that I meet, revisited!


**TODO**
---------

- [Front End TODO List](./TODO.Frontend.md)
- [Back End TODO List](./TODO.Backend.md)



-----------------

- Add Person to Dbset
- CRRUD - Person
    - dbPersonRepository
    - ApiEndpoints 


**EOD 4-14-2025**

TODO:

add Services to Program.cs

- ~~IPersonRepository~~


- one to many???

---------------

Repository

----------
Person
- ~~Create~~
- ~~Read One~~
- ~~Read All (by user)~~
- Update
- Delete

RecordCollector, ContactInfo, FullName
- Update (only NoteText, tbh)

SocialMedia, Entry
- Create
- Read All
- Read One
- Update
- Delete

------------------

------------

ToJson? for quick results? or should I just scaffold Lists

--------
-------

IDataStorageService (Frontend)
-
- Use to get & set data to be used, such as:
  - UserId,
  - UserName,
  - password, 
  - JWToken information


-----
-----

Notes from lecture 4-15-25
-----

- views
- eager loading ( .Include() )
  - ReadAllAsync - for lists?
- explicit loading
  (after/when using .Find() )
    - ReadAsync - for single person
- CreateRecommendVM



------

-----


future:
---
- apparently Int64 can sometimes cause issues with JSON. should be fine for now but may want to change after project due