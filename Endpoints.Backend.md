UserAccounts
-----
- `https://localhost:7134/api/user/register`
- `https://localhost:7134/api/user/addrole`
- get token request (login)
  - `https://localhost:7134/auth/login2`
- get token request for comparison (psuedo user authentication)
  - `https://localhost:7134/auth/logindouble`




| Syntax    | Description |
| --------- | ----------- |
| Header    | Title       |
| Paragraph | Text        |


`https:/
/localhost:7134`

--------

[ ] User Accounts & DexHolder
-------
|  ?     | Description                      | HTML   | Definition               | Request Body | Response Body        |
| ---    | ----------------------           | ----   | ------------------------ | ------------ | -------------------- |
|  y     | RA - Get All Users List          | GET    | `api/user/admin/all`     |   None       |  UserListVM Array |
|   y    | R1 - Get One User Info           | GET    | `/auth/logindouble`      |              | User object      |
|   y    | R1 - Get One User Dex            | GET    | `api/user/dex/{id}`      |    None      |                  |
|   y    | R1 - Get Token/Login             | POST   | `auth/login2`            |              |                  |
|   y?   | R1 - Get Token/Login (user auth) | POST   | `auth/logindouble`       |              |                  |
|  y     | C  - Register User               | POST   | `api/user/register`      |  form-data | UserDexHolder Object |
|        |    - Add Role to User            | POST   | `api/user/addrole`       |   form-data | added role message   |
|    y   | U - Update user info             | PUT    | ``                       |  form-data |               |
|    y   | U - Update DexHolder             | PUT    | ``                       | form-data | None |
|    y   | D - Delete User                  | DELETE | `api/user/delete/{id}`      | None         | None |



--------

[ ] Person `api/people`
-------
|  ?     | Description                      | HTML   | Definition                                                     | Request Body           | Response Body      |
| -------| -------------------------------- | ------ | -----------------------------------------------------------    | -------                | ----------------   |
| [HACK]  y | ~~RA - Read People List~~ [^1]   | ~~GET~~ |    (See: Get User Dex)                                        |   --                   |    --              |
|   y    | R1 - Read Person Details         | GET    | `/retrieveViewModel` `/retrieveRequestpath/{input}/{criteria}` | PersonRequest bodyData | PersonDetail       |
|    y   | C  - Create Person w Email       | POST   | `/forms/Create`                                                | NewPersonVM formData   | Person Object      |
|   y    | U - Update Person                | PUT    | `/forms/update/{personId}`                                     | EditPersonFullVM form-data | None           |
|    y   | D - Delete Person                | DELETE | `/delete/{input}/{criteria}`                                   |    None               | None                |


[^1]: workaround by reading array from DexHolder


--------

[ ] Relations (M:N) `api/people`
-------
|  ?     | Description                      | HTML   | Definition               | Request Body | Response Body        |
| ------ | -------------------------------- | ------ |  ---------------------------- | ------------- | ------------- |
|        | RA - Get All Relations (People)  | GET    | `/api/people/retrieveRelations/specific` | RelationRequest  | RelationVM Array   |
|        | RA - Get All Relations (User)    | GET    | `api/people/retrieveRelations/User/{id}` | None               |  RelationVM Array |
|        | RA - Get All Relations           | GET    | `api/people/retrieveRelations/all`       | None                |  RelationVM Array |
|        | R1 - Get Single Relation         | GET    | `api/people/retrieveRelations/one`       | RelationRequest     |  RelationVM |
|        | C - Create Relation              | POST   | `relations/create`                       | RelationReq formdata |  RelationVM | 
|        | U - Update Relation              | PUT    | `/relations/update`                      | Relationship-RequestUpdate form-data | None |
|        | D - Delete Relation              | DELETE | `api/people/relations/delete`            |      | None |

--------


[ ] Entry Items `api/entry`
-------
|  ?     | Description                      | HTML   | Definition                           | Request Body | Response Body        |
| ------ | -------------------------------- | ------ | -----------------------------        | -------------  |  ------------- |
|  NO    | R1 - Read Entry Details  Id      | GET    |        [XXX] Works but wont use   for frontent    | EntryItem Object      |
|  NO    | RA - Read Entries by User        | GET    | ``      [XXX] Works but wont use  for frontent    |       [XXX]     | Array of EntryItem |
|  NO    | RA - Read EntryItem List         | GET    | ``      [XXX]  Works but wont use  for frontent   |     [XXX]       | Array of EntryItem |
|        | R1 - Read DTO Entry Details  Id  | GET    | ` /transfer/one/{Int64 id}          ` |                 | EntryItem Object      |
|        | RA - Read DTO Entry User Person  | GET    | `/transfer/person/{input}/{criteria}` |                 | EntryItem Object      |
|        | RA - Read DTO Entries by User    | GET    | `/transfer/user/{input}`              |                 | Array of EntryItem |
|        | RA - Read DTO EntryItem List     | GET    | ` /transfer/all                     ` |                 | Array of EntryItem |
|        | C  - Create EntryItem            | POST   | `/create`                                   |  EntryItemVM form-data |  EntryItem Object |
|        | U  - Update EntryItem            | PUT    | `/put`                                |  EntryItemVM form-data | None |
|        | D  - Delete EntryItem            | DELETE | ``                                   |                 | None |

--------


[ ] Social Medias `api/SocialMedia`
-------
|  ?     | Description                        | HTML   | Definition                            | Request Body | Response Body        |
| ------ | --------------------------------   | ------ | ------------------------------        | -------------  |  ------------- |
|        | R1 - Read SocialMedia Details      | GET    | `/transfer/one/{Int64 id}          `  |                 |  SocialMediaDTO Object |
|        | RA - Read DTO S.M List User person | GET    | `/transfer/person/{input}/{criteria}` |                 |  Array of SocialMediaDTO |
|        | RA - Read DTO S.M List User        | GET    | `/transfer/user/{input}`              |                 |  Array of SocialMediaDTO |
|        | RA - Read DTO S.M List All         | GET    | `/transfer/all                     `  |                 |  Array of SocialMediaDTO |
|        | C  - Create SocialMedia            | POST   | `/create`                             |  SocialMediaVM form-data |  SocialMedia Object |
|        | U  - Update SocialMedia            | PUT    | `/put`                                |  SocialMediaVM form-data | None |
|        | D  - Delete SocialMedia            | DELETE | `/delete/{Int64 id}                `  |                | None |


--------


### <sub>(`api/SocialMedia/ContactInfo`)</sub>
## [ ] ContactInfo `..SocialMedia/ContactInfo`

| ?      | Description                        | HTML   | Definition                            | Request Body            | Response Body        |
| ------ | ---------------------------------- | ------ | ------------------------------------- | ----------------------- | -------------------- |
|        | R1 - Read ContactInfo NoteText     | GET    | `/note/Get/{input}/{criteria}     `  |                         | SocialMedia Object   |
|        | U  - Update SocialMedia            | PUT    | `note/Put/{input}/{criteria} `       | string newText form-data | None                 |








https://gist.github.com/Myndex/5140d6fe98519bb15c503c490e713233





