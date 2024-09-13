using API.Services.Concretes;
using API.Services.Interfaces;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<Secrets>(builder.Configuration.GetSection("Secrets"));

builder.Services.AddSingleton<ISecrets>(provider =>
    provider.GetRequiredService<IOptions<Secrets>>().Value);

builder.Services.AddSingleton<IAssistantsClient, AssistantsClient>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
