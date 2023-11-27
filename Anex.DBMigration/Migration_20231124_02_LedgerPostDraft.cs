using Anex.DBMigration.Extensions;
using FluentMigrator;

namespace Anex.DBMigration;

[Migration(2023112402, "Adding LedgerPostDraft table")]
public class Migration_20231124_02_LedgerPostDraft : Migration
{
    public override void Up()
    {
        this.CreateEntityTable(TableConstants.LedgerPostDraftTable)
            .WithColumn("fiscaldate").AsDate().NotNullable()
            .WithColumn("vouchernumber").AsInt32().Nullable()
            .WithColumn("amount").AsDecimal(15, 2).NotNullable()
            .WithColumn("ledgertagid").AsInt64().Nullable()
            .WithColumn("contratagid").AsInt64().Nullable()
            .WithColumn("ledgerdraftid").AsInt64().NotNullable();

        this.CreateForeignKey(TableConstants.LedgerPostDraftTable, TableConstants.LedgerDraftTable);
        this.CreateForeignKey(TableConstants.LedgerPostDraftTable, TableConstants.LedgerTagTable);
        this.CreateForeignKey(TableConstants.LedgerPostDraftTable, TableConstants.LedgerTagTable, "contratag");
    }

    public override void Down()
    {
        this.DeleteEntityTable(TableConstants.LedgerPostDraftTable);
    }
}