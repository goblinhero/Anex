using Anex.DBMigration.Extensions;
using FluentMigrator;

namespace Anex.DBMigration;

[Migration(2023112701, "Adding EconomicTransaction table")]
public class Migration_20231127_01_EconomicTransaction : Migration
{
    public override void Up()
    {
        this.CreateEntityTable(TableConstants.EconomicTransactionTable);
    }

    public override void Down()
    {
        Delete.Table(TableConstants.EconomicTransactionTable);
    }
}