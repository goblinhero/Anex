using System;
using Anex.Api.Database;
using Anex.Api.Database.Mappings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("POSTGRESQLCONNSTR_CONNECTION_ANEX_DB");

if (!string.IsNullOrEmpty(connectionString))
{
    SessionHelper.Initialize(connectionString, typeof(LedgerTagMapping));
}
else
{
    SessionHelper.Initialize("Server=localhost;Database=Anex_DB;Port=5432;User Id=postgres;Password=mysecretpassword;",typeof(LedgerTagMapping));
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ISessionHelper, SessionHelper>();
builder.Services.AddDateOnlyTimeOnlyStringConverters();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
