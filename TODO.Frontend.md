# [CSCI3110-NetDexTestFull](/README.md)
 A repository that contains the  ASP.NET Web API backend project and the MVC frontend application. A pokedex for the people that I meet, revisited!

[_go to BackEnd_](./TODO.Backend.md) 

[_home_](/README.md)
## NetDexTest-01-MVC
### FRONTEND TO-DO LIST


**TODO**



----------------

Useful tutorial: [.NET MVC App Calling Web API for Authentication](https://memorycrypt.hashnode.dev/net-mvc-app-calling-web-api-for-authentication#heading-2-add-web-api-urls-in-appsettingsjson)



----------------


List all users
-----

[user.username] [user.email] [user.accessFailCount] [user.DexHolder.Id] [user.DexHolder.FirstName] [user.DexHolder.LastName]

Edit User VMModel
-----
[user.username] [user.email] [user.Password]->(oldPassword) [(confirmPassword)]


Delete User
------



https://stackoverflow.com/a/46121119


https://stackoverflow.com/a/59835938



[URI Parameters binding](https://learn.microsoft.com/en-us/aspnet/web-api/overview/formats-and-model-binding/parameter-binding-in-aspnet-web-api)
```
public class GeoPoint
{
    public double Latitude { get; set; } 
    public double Longitude { get; set; }
}

public ValuesController : ApiController
{
    public HttpResponseMessage Get([FromUri] GeoPoint location) { ... }
}
```

^^ could be especially useful for PersonPerson




- `/dex/{input?}`
- `/dex/{input!}/Edit`
- `/dex/{input!}/Details`

 ~~eventually: {localCount} (?) I mean, we can use DexHolderListVM -> List<PersonListDexVM> on the backend and since we're already passing email, I mean it should work. but. later...~~ 
|
V
- `/dex/{input!}/p/{nickname}` (detail)


- `/dex/{input!}/p/{nickname}/Edit` 

- `/dex/{input!}/p/{nickname}/rec` (list)
- `/dex/{input!}/p/{nickname}/rec/{id}`
    - TODO: viewmodel

- `/dex/{input!}/p/{nickname}/cinfo` (list)
- `/dex/{input!}/p/{nickname}/cinfo/{id}`
     - TODO: viewmodel


- `GET /dex/{input!}/pplist`
     - list
     - `[ppId][ppNickname][related to][pcNickname][pcId][email]`

- `GET /dex/{input!}/pplist/?parent=<ppId>&child=<pcId>`

- `POST /dex/{input!}/pplist (FromForm)`
     - include drop down menus
- `PUT /dex/{input!}/pplist (FromForm)`
     - just include the string


