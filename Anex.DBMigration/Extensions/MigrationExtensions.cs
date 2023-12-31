using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace Anex.DBMigration.Extensions;

public static class MigrationExtensions
{
    public static void CreateForeignKey(this Migration migration, string fromTable, string toTable, string? property = null)
    {
        var foreignColumn = $"{property ?? toTable}id";
        migration.Create.ForeignKey($"fk_{fromTable}_{toTable}_{foreignColumn}")
            .FromTable(fromTable).ForeignColumn(foreignColumn)
            .ToTable(toTable).PrimaryColumn("id");
    }
    
    public static ICreateTableColumnOptionOrWithColumnSyntax CreateEntityTable(this Migration migration, string tableName)
    {
        migration.Insert.IntoTable("nhibernate_ids").Row(new { entity = tableName, next_hi = 1 });
        return migration.Create.Table(tableName)
            .WithColumn("id").AsInt64().PrimaryKey($"pk_{tableName}")
            .WithColumn("version").AsInt32().NotNullable()
            .WithColumn("created").AsDateTime2().NotNullable();
    }

    public static void DeleteEntityTable(this Migration migration, string tableName)
    {
        RawSql.Insert($"DELETE FROM nhibernate_ids WHERE entity = {tableName}");
        migration.Delete.Table(tableName);
    }
}