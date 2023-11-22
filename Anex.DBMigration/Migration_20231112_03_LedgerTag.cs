using Anex.DBMigration.Extensions;
using FluentMigrator;

namespace Anex.DBMigration;

[Migration(2023111203, "Adding LedgerTag table")]
public class Migration_20231112_03_LedgerTag : Migration
{
    private const string _ledgerTagTable = "ledgertag";

    public override void Up()
    {
        this.CreateEntityTable(_ledgerTagTable)
            .WithColumn("description").AsString(255).NotNullable();
    }

    public override void Down()
    {
        this.DeleteEntityTable(_ledgerTagTable);
    }
}