using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services
    .AddMyApiClient()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}/graphql"));

await builder.Build().RunAsync();