using BlazorGraphQL;
using BlazorGraphQL.Components;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();


builder.Services.AddDbContextPool<ApplicationDbContext>(
    options =>
        options
            .UseNpgsql(builder.Configuration.GetConnectionString("ApplicationDB"))
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging()
            .LogTo(Console.WriteLine, LogLevel.Information));

builder.Services
    .AddGraphQLServer()
    .AddBlazorGraphQLTypes() // AddTypes
    .AddFiltering()
    .AddSorting()
    // .AddProjections()
    .RegisterDbContext<ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapGraphQL();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(BlazorGraphQL.Client._Imports).Assembly);

app.Run();