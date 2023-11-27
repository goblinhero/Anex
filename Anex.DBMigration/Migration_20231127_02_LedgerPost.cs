using Anex.DBMigration.Extensions;
using FluentMigrator;

namespace Anex.DBMigration;

[Migration(2023112702, "Adding LedgerPost table")]
public class Migration_20231127_02_LedgerPost : Migration
{

    public override void Up()
    {
        this.CreateEntityTable(TableConstants.LedgerPostTable)
            .WithColumn("fiscaldate").AsDate().NotNullable()
            .WithColumn("vouchernumber").AsInt32().Nullable()
            .WithColumn("amount").AsDecimal(15, 2).NotNullable()
            .WithColumn("ledgertagid").AsInt64().NotNullable()
            .WithColumn("economictransactionid").AsInt64().NotNullable();
        this.CreateForeignKey(TableConstants.LedgerPostTable, TableConstants.LedgerTagTable);
        this.CreateForeignKey(TableConstants.LedgerPostTable, TableConstants.EconomicTransactionTable);
    }

    public override void Down()
    {
        Delete.Table(TableConstants.LedgerPostTable);
    }
}