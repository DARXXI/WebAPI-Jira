using AdminPanel.Web.hubs;
using JiraTeams.Repositories;
using JiraTeams.Repositories.Interfaces;
//using SignalRApp;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IJiraRepository, JiraRepository>();

//var hubConnection = new signalR.HubConnectionBuilder()
//                .withUrl("/chat")
//                .build();

var app = builder.Build();

app.MapHub<ChatHub>("/listening");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
