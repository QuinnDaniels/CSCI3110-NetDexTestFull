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
|        | R1 - Get One User Info           | GET    | `/auth/logindouble`      |              | User object      |
|   y    | R1 - Get One User Dex            | GET    | `api/user/dex/{id}`      |    None      |                  |
|        | R1 - Get Token/Login             | POST   | `auth/login2`            |              |                  |
|        | R1 - Get Token/Login (user auth) | POST   | `auth/logindouble`       |              |                  |
|  y     | C  - Register User               | POST   | `api/user/register`      |  form-data | UserDexHolder Object |
|        |    - Add Role to User            | POST   | `api/user/addrole`       |   form-data | added role message   |
| [TODO] | U - Update user info             | PUT    | ``                       |  form-data |               |
| [TODO] | U - Update DexHolder             | PUT    | ``                       | form-data | None |
| [TODO] | D - Delete User                  | DELETE | `api/user/dex/{id}`      | None         | None |



--------

[ ] Person
-------
|  ?     | Description                      | HTML   | Definition               | Request Body | Response Body        |
| -------| -------------------------------- | ------ | ----------------------------   | ------- | ---------------- |
| [HACK] | ~~RA - Read People List~~ [^1]   | ~~GET~~ |    (See: Get User Dex)        |         |                  |
|        | R1 - Read Person Details         | GET    | `api/people/retrieveViewModel` |         |                  |
|        | C  - Create Person w Email       | POST   | `api/people/forms/Create`      | NewPersonVM formData | Person Object |
| [TODO] | U - Update Person                | PUT    | ``                             | Person form-data | None  |
|        | D - Delete Person                | DELETE | `api/people/retrieveViewModel` |             | None  |


[^1]: workaround by reading array from DexHolder


--------

[ ] Relations (M:N)
-------
|  ?     | Description                      | HTML   | Definition               | Request Body | Response Body        |
| ------ | -------------------------------- | ------ |  ---------------------------- | ------------- | ------------- |
|        | RA - Get All Relations (People)  | GET    | `/api/people/retrieveRelations/specific` | RelationRequest  | RelationVM Array   |
|        | RA - Get All Relations (User)    | GET    | `api/people/retrieveRelations/User/{id}` | None               |  RelationVM Array |
|        | RA - Get All Relations           | GET    | `api/people/retrieveRelations/all`       | None               |  RelationVM Array |
|        | R1 - Get Single Relation         | GET    | `api/people/retrieveRelations/one`       | RelationRequest    |  RelationVM |
| [TODO] | C - Create Relation              | POST   | ``                                       | Relation formdata    |  RelationVM | 
| [TODO] | U - Update Relation              | PUT    | ``                                       | form-data | None |
| [TODO] | D - Delete Relation              | DELETE | `api/people/retrieveRelations/one`       |      | None |

--------


[ ] Entry Items
-------
|  ?     | Description                      | HTML   | Definition               | Request Body | Response Body        |
| ------ | -------------------------------- | ------ | ----------------------------- | -------------  |  ------------- |
| [TODO] | RA - Read EntryItem List         | GET    | ``                            |                 | Array of EntryItem |
| [TODO] | R1 - Read EntryItem Details      | GET    | ``                            |                 | EntryItem Object      |
| [TODO] | C  - Create EntryItem            | POST   | ``                            |  EntryItem form-data |  EntryItem Object |
| [TODO] | U  - Update EntryItem            | PUT    | ``                            |  EntryItem form-data | None |
| [TODO] | D  - Delete EntryItem            | DELETE | ``                            |                 | None |

--------


[ ] Social Medias
-------
|  ?     | Description                      | HTML   | Definition               | Request Body | Response Body        |
| ------ | -------------------------------- | ------ | ------------------------------ | -------------  |  ------------- |
| [TODO] | RA - Read SocialMedia List       | GET    | ``                             |                 |  Array of SocialMedia |
| [TODO] | R1 - Read SocialMedia Details    | GET    | ``                             |                 |  SocialMedia Object |
| [TODO] | C  - Create SocialMedia          | POST   | ``                             |  SocialMedia form-data |  SocialMedia Object |
| [TODO] | U  - Update SocialMedia          | PUT    | ``                             |  SocialMedia form-data | None |
| [TODO] | D  - Delete SocialMedia          | DELETE | ``                             |                | None |



