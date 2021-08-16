# Identity Provider API
.Net WebApi 2 Restful Api that provide user's registration, authentication and authorization.  
## How to run the Api
- Fork or download the API repo.
- Download all dependencies using the following instructions: [restore nuged packages](https://docs.microsoft.com/en-us/nuget/consume-packages/package-restore#restore-packages-manually-using-visual-studio).
- Build the Solution
- Run the Identity.Server Project
## User stories
-  As a user, I want to register a user using a rest API so that I can login to that API to get a jwt token.
-  As a user, I want to lo gin to a rest API and receive back a jwt token so that I can use that token to access secure endpoints.
-  As an admin user, I want to create, read, update and delete users from a rest API so that I can manage the users' information.
-  As an admin user, I want to create, read, update and delete user's roles so that I can manage the user's roles information.
-  As an admin user, I want to assign roles to users so that I grant access to role-specific secured endpoints.
-  As a user, I want to log out from a rest API and receive back a jwt token so that I can use that token to access secure endpoints.
## How to login and logout
### Log In
-  Send a POST request to the endpoint `/api/Account/Login`, with the following header parameters:
    -  `Content-Type : 'Content-Type: application/x-www-form-urlencoded'
    -  `grant_type=password`
    -  `username=admin`
    -  `password=Password@123`
- Javascript example:
``` 
var data = "";

var xhr = new XMLHttpRequest();
xhr.withCredentials = true;

xhr.addEventListener("readystatechange", function() {
  if(this.readyState === 4) {
    console.log(this.responseText);
  }
});

xhr.open("POST", "https://localhost:44319/api/Account/Logout");
xhr.setRequestHeader("Authorization", "Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJuYW1laWQiOiI1OGFhMjk0Ni1jOGYxLTQyZGItYWY0NS0wOGFlMWI4ZDA5MmEiLCJ1bmlxdWVfbmFtZSI6ImFkbWluIiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS9hY2Nlc3Njb250cm9sc2VydmljZS8yMDEwLzA3L2NsYWltcy9pZGVudGl0eXByb3ZpZGVyIjoiQVNQLk5FVCBJZGVudGl0eSIsIkFzcE5ldC5JZGVudGl0eS5TZWN1cml0eVN0YW1wIjoiNWNhM2Y0MTEtYmUwMS00ZTQzLWJjZjctYTM0NDlmMTFjYzk4Iiwicm9sZSI6IkFkbWluaXN0cmF0b3IiLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0IiwiYXVkIjoiZTZiMGY5M2I2MDI1NDQwODlkMzQzNmZhYmM1YjRhYjAiLCJleHAiOjE2MjkxMzkyOTIsIm5iZiI6MTYyOTEzNTY5Mn0.kZyRaBt5ad5ugH2dDXXrZ8kWaz7rGHd2ZnKZrT0CxsU");

xhr.send(data);
```
-  Swagger example:
    -  Open the API swagger url in a browser by example `https://localhost:44319//Swagger`
    -  Click on the `Auth` link.
    -  Click on the `api/Account/Login` link.
    -  Fill in the grant_type, username, and password inputs.
    -  Click on the 'Try it out!' button.
### Log Out
-  Send a POST request to the endpoint `/api/Account/Logout`, with the following header parameters:
    -  `"Authorization" : "bearer XXXX"'` where XXX is the jwt token
- Javascript example:
``` 
var data = "grant_type=password&username=admin&password=Password%40123";

var xhr = new XMLHttpRequest();
xhr.withCredentials = true;

xhr.addEventListener("readystatechange", function() {
  if(this.readyState === 4) {
    console.log(this.responseText);
  }
});

xhr.open("POST", "https://localhost:44319/api/Account/Login");
xhr.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");

xhr.send(data);
```
-  Swagger example:
    -  Open the API swagger url in a browser by example `https://localhost:44319//Swagger`
    -  Click on the `Account` link.
    -  Click on the `api/Account/Logout` link.
    -  Fill the `Authorization` parameter input with the string `bearer XXX` where XXX is the jwt token
    -  Click on the 'Try it out!' button.
## How to manage users and roles

## Features

## Future features

