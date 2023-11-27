using Anex.DBMigration.Extensions;
using FluentMigrator;

namespace Anex.DBMigration;

[Migration(2023112401, "Adding LedgerDraft table")]
public class Migration_20231124_01_LedgerDraft : Migration
{
    public override void Up()
    {
        this.CreateEntityTable(TableConstants.LedgerDraftTable)
            .WithColumn("description").AsString(255).NotNullable()
            .WithColumn("fiscalperiodid").AsInt64().Nullable();

        this.CreateForeignKey(TableConstants.LedgerDraftTable, TableConstants.FiscalPeriodTable);
    }

    public override void Down()
    {
        this.DeleteEntityTable(TableConstants.LedgerDraftTable);
    }
}