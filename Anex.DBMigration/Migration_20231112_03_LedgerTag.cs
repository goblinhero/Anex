using Anex.DBMigration.Extensions;
using FluentMigrator;

namespace Anex.DBMigration;

[Migration(2023111203, "Adding LedgerTag table")]
public class Migration_20231112_03_LedgerTag : Migration
{
    public override void Up()
    {
        this.CreateEntityTable(TableConstants.LedgerTagTable)
            .WithColumn("description").AsString(255).NotNullable();
    }

    public override void Down()
    {
        this.DeleteEntityTable(TableConstants.LedgerTagTable);
    }
}