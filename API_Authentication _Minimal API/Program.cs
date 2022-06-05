using API_Authentication__Minimal_API.Models;
using API_Authentication__Minimal_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateActor = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["jwt:issuer"],
            ValidAudience = builder.Configuration["jwt:audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"]))
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddSingleton<IMovovieService, MovieService>();
builder.Services.AddSingleton<IUserService, UserService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme="Bearer",
        BearerFormat="JWT",
        In=ParameterLocation.Header,
        Name="Authorization",
        Description="Bearer Authentication with JWT Token",
        Type=SecuritySchemeType.Http
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference=new OpenApiReference
                {
                    Id="Bearer",
                    Type=ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();


app.MapGet("/", () => "hello world").ExcludeFromDescription();
app.MapPost("/login", (UserLogin user, IUserService service) => Login(user, service));

app.MapPost("/create",
[Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme,Roles ="Administrator")]
(Movie movie, IMovovieService service) => Create(movie, service))
    .Accepts<Movie>("application/json")
    .Produces<Movie>(statusCode:200,contentType:"application/json");
app.MapGet("/get",
 [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Standard")]
(int id, IMovovieService service) => Get(id, service))
    .Produces<Movie>();
app.MapGet("/list", (IMovovieService service) => List(service));
app.MapPut("/update",
 [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(Movie movie, IMovovieService service) => Update(movie, service))
    .Accepts<Movie>("application/json");
app.MapDelete("/delete",
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
(int id, IMovovieService service) => Delete(id, service));
IResult Login(UserLogin user, IUserService service)
{
    if(!string.IsNullOrEmpty(user.Username)&&
        !string.IsNullOrEmpty(user.Password))
    {
        var loggedUser = service.Get(user);
        if (user is null) return Results.NotFound("user not found");
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, loggedUser.Username),
            new Claim(ClaimTypes.Email, loggedUser.Password),
            new Claim(ClaimTypes.GivenName, loggedUser.GivenName),
            new Claim(ClaimTypes.Surname, loggedUser.Surname),
            new Claim(ClaimTypes.Role, loggedUser.Role),
        };
        var token = new JwtSecurityToken(
            issuer: builder.Configuration["jwt:issuer"],
            audience: builder.Configuration["jwt:audience"],
            claims:claims,
            expires:DateTime.UtcNow.AddDays(60),
            notBefore:DateTime.UtcNow,
            signingCredentials:new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwt:key"])),
                SecurityAlgorithms.HmacSha256)
            );
        var tokenString=new JwtSecurityTokenHandler().WriteToken(token);
        return Results.Ok(tokenString);
    }
    return Results.NotFound("user us invalid");
}
IResult Create(Movie movie, IMovovieService service)
{
    var result=service.Create(movie);
    return Results.Ok(result);
}
IResult Get(int id, IMovovieService service)
{
    var result = service.Get(id);
    if (result is null) return Results.NotFound("Movie not found");
    return Results.Ok(result);
}
IResult List(IMovovieService service)
{
    var result = service.List();
    return Results.Ok(result);
}
IResult Update(Movie newMovie, IMovovieService service)
{
    var updatedMovie = service.Update(newMovie);
    if (updatedMovie is null) return Results.NotFound("movie not found");
    return Results.Ok(updatedMovie);
}
IResult Delete(int id, IMovovieService service)
{
    var result = service.Delete(id);
    if (!result) return Results.BadRequest("something went wrong");
    return Results.Ok(result);
}
app.Run();

