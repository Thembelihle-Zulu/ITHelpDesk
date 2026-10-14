using System.Text;
using AuthProject.Data;
using SharedLibrary.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharedLibrary.Data;
using SharedLibrary.Model;
using Swashbuckle.AspNetCore.Filters;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);
Env.Load();
// configuring the database connection
var connectionString = Environment.GetEnvironmentVariable("HelpDeskConnectionString") ??  builder.Configuration.GetConnectionString("DefaultConnection")  ;

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, FakeEmailSender>();
builder.Services.AddControllers();


//loading the values stored on the system variables
var key = builder.Configuration["Jwt:Key"];
var username=builder.Configuration["admin"] ?? "testadmin";
var email = builder.Configuration["AdminEmail"] ?? "testadmin@example.com";
var password = builder.Configuration["superpassword"] ?? "Test@1234" ;

//injecting the dependency injection for authentication
builder.Services.AddAuthentication(options =>
{
    //the following sets Bearer(OAuth2-style tokens) as the default way your API will:
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;     //Authenticate requests     
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;        //Challenge unauthorized requests
})
    //The app is told to validate the JWT token that was issued 
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };
    });
builder.Services.AddAuthorization();

  // This adds the identity API endpoints 
builder.Services.AddEndpointsApiExplorer();

//configuring swagger 
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddTransient<ICustomEmailSender, FakeEmailSender>();


//Adding the Cross-Origin Resource sharing, to allow web pages that are from different origins to make requests to the api(the frontend and backend are both running on the local
//but they are running on different ports,which may appear as different origins
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
var app = builder.Build();

app.UseCors("AllowAll");
/*
"Since we are outside a regular HTTP request, we create a temporary service scope using CreateScope().
This gives us a ServiceProvider, which is our way of accessing any registered services � like UserManager, RoleManager, or DbContext.
We then call GetRequiredService<T>() to manually get an instance of the services we want to use inside this startup logic.
All of this happens inside a using block so that everything is cleaned up properly afterward."
*/
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var config = builder.Configuration;
    await SeedData.CreateRoles(services, username, password,email);
}
    if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();
app.MapIdentityApi<ApplicationUser>();
app.Run();
