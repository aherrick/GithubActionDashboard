using Blazored.LocalStorage;
using GithubActionDashboard.Components;
using GithubActionDashboard.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<GitHubService>();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();