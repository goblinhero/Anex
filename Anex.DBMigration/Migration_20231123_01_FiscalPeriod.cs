using Anex.DBMigration.Extensions;
using FluentMigrator;

namespace Anex.DBMigration;

[Migration(2023112301, "Adding FiscalPeriod table")]
public class Migration_20231123_01_FiscalPeriod : Migration
{
    private string _fiscalPeriodTable = "fiscalperiod";

    public override void Up()
    {
        this.CreateEntityTable(_fiscalPeriodTable)
            .WithColumn("startdate").AsDate().NotNullable()
            .WithColumn("enddate").AsDate().NotNullable();
    }

    public override void Down()
    {
        this.DeleteEntityTable(_fiscalPeriodTable);
    }
}