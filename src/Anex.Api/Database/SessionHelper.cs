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

    public static void Initialize(bool createDatabase, string connectionString, params Type[] mappingTypes)
    {
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
                dbcp.ConnectionString = connectionString;
                dbcp.Driver<SqlClientDriver>();
                dbcp.Dialect<MsSql2012Dialect>();
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
        if (createDatabase)
            new SchemaExport(configuration).Execute(true, true, false);
        _sessionFactory = configuration.BuildSessionFactory();
    }

    public async Task<QueryResult<T>> TryExecuteQuery<T>(IExecutableQuery<T> query)
    {
        if (_sessionFactory == null) throw new Exception("Sessionfactory has not been initialized. Should be initialized at app startup using static method .Initialize()");
        using (var session = _sessionFactory.OpenSession())
        using (var tx = session.BeginTransaction())
        {
            var result = await query.TryExecute(session);
            return result;
        }
    }
}
