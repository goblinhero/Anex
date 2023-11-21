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
using Anex.Api.Database.Queries;

namespace Anex.Api.Database;

public class SessionHelper : ISessionHelper
{
    private static ISessionFactory? _sessionFactory;
    private static string? _connectionString;
    private static bool? _createDatabase;
    private static Type[]? _mappingTypes;

    public static void Initialize(bool createDatabase, string connectionString, params Type[] mappingTypes)
    {
        _connectionString = connectionString;
        _createDatabase = createDatabase;
        _mappingTypes = mappingTypes;
    }

    private void Initialize()
    {
        if (_mappingTypes == null) throw new Exception("No mapping types");
        
        
        var modelMapper = new ModelMapper();
        var mappings = _mappingTypes
            .Select(Assembly.GetAssembly)
            .Distinct()
            .SelectMany(a =>
            {
                if (a == null) throw new Exception("Assembly is null. Should not be possible.");
                return a.GetExportedTypes();
            });
        modelMapper.AddMappings(mappings);
        modelMapper.BeforeMapManyToOne += (modelInspector, propertyPath, map) =>
            map.Column(propertyPath.LocalMember.Name + "Id");
        modelMapper.BeforeMapProperty += (inspector, member, customizer) =>
        {
            if (member.GetRootMember().MemberType == MemberTypes.Property &&
                ((PropertyInfo)member.GetRootMember()).PropertyType == typeof(DateTime))
                customizer.Type<UtcDateTimeType>();
        };
        var configuration = new Configuration()
            .DataBaseIntegration(dbcp =>
            {
                dbcp.ConnectionString = _connectionString;
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
        if (_createDatabase != null && _createDatabase.Value)
            new SchemaExport(configuration).Execute(true, true, false);
        _sessionFactory = configuration.BuildSessionFactory();
    }

    public async Task<QueryResult<T>> TryExecuteQuery<T>(IExecutableQuery<T> query)
    {
        if (_sessionFactory == null)
        {
            Initialize();
            return await TryExecuteQuery<T>(query);
        }
        
        using (var session = _sessionFactory.OpenSession())
        using (var tx = session.BeginTransaction())
        {
            var result = await query.TryExecute(session);
            return result;
        }
    }

    public string? GetConnectionString()
    {
        return _connectionString;
    }
}
