using Anex.DBMigration.Extensions;
using FluentMigrator;

namespace Anex.DBMigration;

[Migration(2023112301, "Adding FiscalPeriod table")]
public class Migration_20231123_01_FiscalPeriod : Migration
{
    public override void Up()
    {
        this.CreateEntityTable(TableConstants.FiscalPeriodTable)
            .WithColumn("startdate").AsDate().NotNullable()
            .WithColumn("enddate").AsDate().NotNullable();
    }

    public override void Down()
    {
        this.DeleteEntityTable(TableConstants.FiscalPeriodTable);
    }
}