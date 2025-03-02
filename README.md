## ASP.NET CORE 9 JWT auth example with SPA frontend

#### Setup user secrets

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=construction_estimator;Username=postgres;Password=postgres;Include Error Detail=True"
```

Install necessary libraries

```shell
nuget install Microsoft.AspNetCore.Authentication.JwtBearer
nuget install Microsoft.AspNetCore.Identity
nuget install Microsoft.EntityFrameworkCore
nuget install Microsoft.EntityFrameworkCore.Relational
nuget install Microsoft.AspNetCore.Identity.EntityFrameworkCore
nuget install Microsoft.AspNetCore.Authentication.Cookies
```

Add user with custom ID

Add AppContext

Add serviceCollectionExtensions

Install swagger

```shell
nuget install Swashbuckle.AspNetCore.SwaggerGen
nuget install Swashbuckle.AspNetCore.SwaggerUI
```


Add JwtSettings, add ServiceCollectionExtensions

Add Web/Auth
Add CustomNoOpEmailSender
Add TokenService
Add ServiceCollectionExtensions

Start postgres

```shell
docker compose up
```

Run migrations

```shell
dotnet ef database update
```

Add GetMySelfFeature
ADd serviceCollectionExtensions

Add AuthenticationController

How to use in SPA:

