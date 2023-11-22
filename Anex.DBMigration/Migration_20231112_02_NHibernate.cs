using FluentMigrator;
using FluentMigrator.Builders.Insert;


namespace Anex.DBMigration;

[Migration(2023111202, "Adding NHibernate specific table for hilo algorithm")]
public class Migration_20231112_02_NHibernate : Migration
{
    private const string _hiloTable = "nhibernate_ids";

    public override void Up()
    {
        Create.Table(_hiloTable)
            .WithColumn("next_hi").AsInt32().NotNullable()
            .WithColumn("entity").AsString(255);
    }

    public override void Down()
    {
        Delete.Table(_hiloTable);
    }
}