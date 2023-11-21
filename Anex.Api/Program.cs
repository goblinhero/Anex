using Anex.Api.Database;
using Anex.Api.Database.Mappings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("POSTGRESQLCONNSTR_CONNECTION_ANEX_DB");

if (!string.IsNullOrEmpty(connectionString))
{
    SessionHelper.Initialize(true, connectionString, typeof(LedgerTagMapping));
}

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
