# WebApi2_Owin_OAuthAccessTokenAuthentication
A WebApi2 app built on OWIN middleware that demonstrates how to implement OAuth JWT's (Access), plus several ways to generate claims on the fly.

Developed with VS2015 Community, requires Fiddler or POSTMAN to test. 

The migrations configuration and seed method are available for you to create your own database, just change the database connection.

---
####Techs

| No.        | Description  |
| -----------|-------------|
| 1 | ASP.NET Web Api 2 |
| 2 | ASP.NET IDentity 2.1 |
| 3 | Entity Framework 6.1.3 |
| 4 | OWIN |
| 5 | Json Web Access Tokens |
| 6 | C# |
| 7 | Linq |
| 8 | Code First |

---

####Features

|Feature| Description  |
|-------|--------------|
|Claims | Implementation of a custom claims factory using the 'ClaimsIdentityFactory' class for creating claims |
|Claims | Implementation of 'AuthorizationFilterAttribute' that checks if a user has the required claim |
|Claims | Provision of an extended claims service 'ExtendedClaimsProvider' used for creating claims |
|User Management| Implemented with 'UserManager'|
|Roles Management| Implemented with 'RoleManager'|
|Password Policy| Implemented with 'PasswordValidator'|
|User Name Policy | Implemented with 'UserValidator'|
|Email confirmation| Implemented with 'IIdentityMessageService' and Gmail|
|Authorization Server| Authorization server configured with 'OAuthAuthorizationServerOptions' |
|Resource Server| Resource server authentication configured with 'JwtBearerAuthenticationOptions' |
|Bearer Access Tokens| Implemented with 'ISecureDataFormat' and OAuth Json Web Tokens (JWT) |

---

####Controller Methods

|Controller|Methods|
|----------|-------|
|Accounts|Register, Change Password, Login, Delete User, Assign Claims, Remove Claims, Get User By Id, Get User By Name |
|Claims| Unpacking claims in the JWT and returning them |
|Roles| Get Role By Id, Get All Roles, Create Role, Delete Role, Manage Users in a Role|
|Orders| 'RefundOrder' which requires the user to have the 'IncidentResolvers' claim. 'GetOrder' which requires the user to have the 'FTE' claim (Full Time Employee)|

---

####To Test

Run the client assembly. This will test 3 endpoints, there are more endpoints that can be tested, for example: The OrdersController contains an action that allows a user to refund money on an order, however, the user must have the "IncidentResolvers" claim and for that they need to have joined more than 3 months previously so any new users should not be able to perform this action.

Just checkout the controllers...

---

#####Resources

| Title  | Author | Publisher |
| -----------|-------------|-----------|
| [IDentity Management in ASP.NET](https://www.youtube.com/watch?v=A8Cfc62xdMo) | Brock Allen |Tech Talk|
| [ASP.NET Identity 2.1 with ASP.NET Web API 2.2 (Accounts Management)](http://bitoftech.net/2015/01/21/asp-net-identity-2-with-asp-net-web-api-2-accounts-management/) | Taiseer Joudeh| Bit Of Technology |
| [Pro ASP.NET Web API Security](http://www.apress.com/9781430257820) | Badrinarayanan Lakshmiraghavan |Apress|



