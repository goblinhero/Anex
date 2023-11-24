using Anex.DBMigration.Extensions;
using FluentMigrator;

namespace Anex.DBMigration;

[Migration(2023112401, "Adding LedgerDraft table")]
public class Migration_20231124_01_LedgerDraft : Migration
{
    private string _ledgerDraftTable = "ledgerdraft";
    private string _fiscalPeriodTable = "fiscalperiod";
    public override void Up()
    {
        this.CreateEntityTable(_ledgerDraftTable)
            .WithColumn("description").AsString(255).NotNullable()
            .WithColumn("fiscalperiodid").AsInt64().Nullable();

        this.CreateForeignKey(_ledgerDraftTable, _fiscalPeriodTable);
    }

    public override void Down()
    {
        this.DeleteEntityTable(_ledgerDraftTable);
    }
}