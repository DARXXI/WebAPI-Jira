using AdminPanel.Web.hubs;
using JiraTeams.Repositories;
using JiraTeams.Repositories.Interfaces;
using Microsoft.AspNetCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Net.Http;
//using SignalRApp;

//HttpClient client = new HttpClient();
var builder = WebApplication.CreateBuilder(args);

//{

//    Hook hook = new Hook { };
//    // создаем JsonContent
//    JsonContent content = JsonContent.Create(hook);
//    // отправляем запрос
//    using var response = await client.PostAsync("https://jira.arsis.ru/rest/webhooks/1.0/webhook", content);
//    Hook h2 = await response.Content.ReadFromJsonAsync<Hook>();
//    Console.WriteLine(h2.url, h2.name);

//    //var content = new FormUrlEncodedContent(values);

//    //var response = await client.PostAsync("https://jira.arsis.ru/rest/webhooks/1.0/setWebhook?url=https://2c87-31-148-138-233.eu.ngrok.io", content);

//    //var responseString = await response.Content.ReadAsStringAsync();


//    response.EnsureSuccessStatusCode();
//    string responseBody = await response.Content.ReadAsStringAsync();
//    // Above three lines can be replaced with new helper method below
//    // string responseBody = await client.GetStringAsync(uri);

//    Console.WriteLine(responseBody);
//}
//catch (HttpRequestException e)
//{
//    Console.WriteLine("\nException Caught!");
//    Console.WriteLine("Message :{0} ", e.Message);
//}

// Add services to the container.
//builder.Services.AddSignalR();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IJiraRepository, JiraRepository>();

//var hubConnection = new signalR.HubConnectionBuilder()
//                .withUrl("/chat")
//                .build();

var app = builder.Build();

//app.MapHub<ChatHub>("/listening");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
