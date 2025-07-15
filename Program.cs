
using pefi.githublistener.Services;
using Octokit.Webhooks;
using Octokit.Webhooks.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Logging.AddConsole();

builder.Services.AddHttpClient<ServiceManagerClient>((sp,c) => {
    var baseAddress = builder.Configuration.GetSection("ServiceManager").GetValue<string>("baseurl") ?? "";
    c.BaseAddress = new Uri(baseAddress); 
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<WebhookEventProcessor, ProcessRegistryPackageWebhookProcessor>();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "allow-all",
        policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyOrigin();
            policy.AllowAnyMethod();
        });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();

app.UseCors();
app.MapGitHubWebhooks("service-manager/newpackage");

app.Run();


