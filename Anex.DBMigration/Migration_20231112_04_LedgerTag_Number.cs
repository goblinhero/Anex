using FluentMigrator;

namespace Anex.DBMigration;

[Migration(2023111204, "Adding Number to LedgerTag")]
public class Migration_20231112_04_LedgerTag_Number : Migration
{
    private string _newColumn = "number";

    public override void Up()
    {
        Alter.Table(TableConstants.LedgerTagTable)
            .AddColumn(_newColumn).AsInt32().Nullable();
    }

    public override void Down()
    {
        Delete.Column(_newColumn).FromTable(TableConstants.LedgerTagTable);
    }
}