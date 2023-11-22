using FluentMigrator;

namespace Anex.DBMigration;

[Migration(2023111201, "Cleaning up from initial tests")]
public class Migration_20231112_01_CleanUp : Migration
{
    public override void Up()
    {
        //These are tables temporarily created by using nhibernate schemaexport for early testing purpose
        foreach (var table in new[]{"hibernate_unique_key","ledgertag"})
        {
            if (Schema.Table(table).Exists())
            {
                Delete.Table(table);
            }
        }
    }

    public override void Down()
    {
        //These tables should not be recreated as they are created in the following steps
    }
}