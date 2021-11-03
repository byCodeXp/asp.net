# Identity.Jwt.Authorization

### Packages

Microsoft.EntityFrameworkCore:

https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/5.0.11

Microsoft.AspNetCore.Identity.EntityFrameworkCore:

https://www.nuget.org/packages/Microsoft.AspNetCore.Identity.EntityFrameworkCore

Microsoft.AspNetCore.Authentication.JwtBearer:

https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer

### Configure in Startup.cs

Register identity in services:

```csharp
services.AddIdentity<User, IdentityRole>()
        .AddEntityFrameworkStores<DataContext>()
        .AddDefaultTokenProviders();
```

Add authentication scheme:

```csharp
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfiguration.Secret)),
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireExpirationTime = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});
```

Add `app.UseAuthentication()` between `app.UseRouting()` and `app.UseAuthorization()`.

```csharp
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
```
