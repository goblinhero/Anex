using Anex.DBMigration.Extensions;
using FluentMigrator;

namespace Anex.DBMigration;

[Migration(2023112402, "Adding LedgerPostDraft table")]
public class Migration_20231124_02_LedgerPostDraft : Migration
{
    private string _ledgerPostDraftTable = "ledgerpostdraft";
    private string _ledgerDraftTable = "ledgerdraft";
    private string _ledgerTagTable = "ledgertag";

    public override void Up()
    {
        this.CreateEntityTable(_ledgerPostDraftTable)
            .WithColumn("fiscaldate").AsDate().NotNullable()
            .WithColumn("vouchernumber").AsInt32().Nullable()
            .WithColumn("amount").AsDecimal(15, 2).NotNullable()
            .WithColumn("ledgertagid").AsInt64().Nullable()
            .WithColumn("contratagid").AsInt64().Nullable()
            .WithColumn("ledgerdraftid").AsInt64().NotNullable();

        this.CreateForeignKey(_ledgerPostDraftTable, _ledgerDraftTable);
        this.CreateForeignKey(_ledgerPostDraftTable, _ledgerTagTable);
        this.CreateForeignKey(_ledgerPostDraftTable, _ledgerTagTable, "contratag");
    }

    public override void Down()
    {
        this.DeleteEntityTable(_ledgerPostDraftTable);
    }
}