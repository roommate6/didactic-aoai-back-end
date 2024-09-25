using API;
using API.Services.Concretes;
using API.Services.Interfaces;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

var keyVaultURI = new Uri(
    $"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"
);

builder.Configuration.AddAzureKeyVault(
    keyVaultURI,
    new DefaultAzureCredential(),
    new CustomSecretManager("didacticapi")
);

builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "CORS",
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:3000")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowAnyOrigin();
                                  });
            });

builder.Services.Configure<Secrets>(builder.Configuration.GetSection("Secrets"));
builder.Services.Configure<AppConfiguration>(builder.Configuration.GetSection("AppConfiguration"));

builder.Services.AddSingleton<ISecrets>(provider =>
    provider.GetRequiredService<IOptions<Secrets>>().Value);
builder.Services.AddSingleton<IAppConfiguration>(provider =>
    provider.GetRequiredService<IOptions<AppConfiguration>>().Value);

builder.Services.AddSingleton<IAssistantsClient, AssistantsClient>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors("CORS");

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
