using GraphQLWithEFCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<ApplicationDbContext>(
    options =>
        options
            .UseNpgsql(builder.Configuration.GetConnectionString("ApplicationDB"))
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, LogLevel.Information));

builder.Services
    .AddGraphQLServer()
    .AddTypes()
    .AddFiltering()
    .AddSorting()
    // .AddProjections()
    .RegisterDbContext<ApplicationDbContext>();

var app = builder.Build();

app.UseStaticFiles();

app.MapGraphQL();

app.Run();