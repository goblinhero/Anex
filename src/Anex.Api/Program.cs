using Anex.Api.Database;
using Anex.Api.Database.Mappings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

//SessionHelper.Initialize(false, @"Server=.\SQLEXPRESS;initial catalog=Testers;Integrated Security=true", typeof(LedgerTagMapping));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ISessionHelper, SessionHelper>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
