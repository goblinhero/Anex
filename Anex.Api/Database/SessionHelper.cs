using NHibernate.Event;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using NHibernate;
using System.Reflection;
using NHibernate.Type;
using NHibernate.Cfg;
using NHibernate.Driver;
using NHibernate.Dialect;
using Anex.Api.Database.Listeners;
using System;
using System.Linq;
using System.Threading.Tasks;
using Anex.Api.Database.Commands;
using Anex.Api.Database.Queries;
using Anex.DBMigration;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;

namespace Anex.Api.Database;

public class SessionHelper : ISessionHelper
{
    private static ISessionFactory? _sessionFactory;
    private static string? _connectionString;
    public static void Initialize(string connectionString, params Type[] mappingTypes)
    {
        _connectionString = connectionString;
        var modelMapper = new ModelMapper();
        var mappings = mappingTypes
            .Select(Assembly.GetAssembly)
            .Distinct()
            .SelectMany(a =>
            {
                if (a == null) throw new Exception("Assembly is null. Should not be possible.");
                return a.GetExportedTypes();
            });
        modelMapper.AddMappings(mappings);
        modelMapper.BeforeMapManyToOne += (_, propertyPath, map) =>
            map.Column(propertyPath.LocalMember.Name + "Id");
        modelMapper.BeforeMapProperty += (_, member, customizer) =>
        {
            if (member.GetRootMember().MemberType == MemberTypes.Property &&
                ((PropertyInfo)member.GetRootMember()).PropertyType == typeof(DateTime))
                customizer.Type<UtcDateTimeType>();
        };
        var configuration = new Configuration()
            .DataBaseIntegration(dbcp =>
            {
                dbcp.ConnectionString = connectionString;
                dbcp.Driver<NpgsqlDriver>();
                dbcp.Dialect<PostgreSQL83Dialect>();
            });
        configuration.AddDeserializedMapping(modelMapper.CompileMappingForAllExplicitlyAddedEntities(), "mappings");
        configuration.EventListeners.PreInsertEventListeners = new IPreInsertEventListener[]
        {
            new SetCreationDateListener(),
            new CheckValidityListener()
        };
        configuration.EventListeners.PreUpdateEventListeners = new IPreUpdateEventListener[]
        {
            new CheckValidityListener(),
            new CheckTransactionalUpdateListener()
        };
        configuration.EventListeners.PreDeleteEventListeners = new IPreDeleteEventListener[]
        {
            new DisallowDeleteTransactionListener()
        };
        _sessionFactory = configuration.BuildSessionFactory();
    }

    public async Task<QueryResult<T>> TryExecuteQuery<T>(IExecutableQuery<T> query)
    {
        if (_sessionFactory == null) throw new Exception("Critical error, SessionFactory not initialized.");
        
        using (var session = _sessionFactory.OpenSession())
        using (var tx = session.BeginTransaction())
        {
            var result = await query.TryExecute(session);
            await tx.RollbackAsync();
            return result;
        }
    }

    public async Task<CommandResult> TryExecuteCommand(IExecutableCommand command)
    {
        if (_sessionFactory == null) throw new Exception("Critical error, SessionFactory not initialized.");
        
        using (var session = _sessionFactory.OpenSession())
        using (var tx = session.BeginTransaction())
        {
            var result = await command.TryExecute(session);
            if (result.Success)
            {
                await tx.CommitAsync();
            }
            else
            {
                await tx.RollbackAsync();
            }            
            return result;
        }
    }

    public void MigrateDatabaseToNewest()
    {
        RunMigrationAction(r => r.MigrateUp());
    }

    public void MigrateDatabaseDownToVersion(long version)
    {
        RunMigrationAction(r => r.MigrateDown(version));
    }

    private static void RunMigrationAction(Action<IMigrationRunner> action)
    {
        using (var serviceProvider = CreateMigrationServices())
        using (var scope = serviceProvider.CreateScope()) //The scope ensures all instances are properly disposed 
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            action(runner);
        }
    }
    private static ServiceProvider CreateMigrationServices()
    {
        return new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres11_0()
                .WithGlobalConnectionString(_connectionString)
                .ScanIn(typeof(Migration_20231112_01_CleanUp).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);
    }
}
