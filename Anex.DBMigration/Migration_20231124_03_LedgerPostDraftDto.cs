using FluentMigrator;

namespace Anex.DBMigration;

[Migration(2023112403, "Adding LedgerPostDraftDto view")]
public class Migration_20231124_03_LedgerPostDraftDto : Migration
{
    public override void Up()
    {
        Execute.Sql(@"CREATE VIEW ledgerpostdraftdto AS 
                      SELECT lpd.id
                           , lpd.version
                           , lpd.created
                           , lpd.fiscaldate
                           , lpd.vouchernumber
                           , lpd.amount
                           , lpd.ledgertagid
                           , lt.number as ledgertagnumber
                           , lt.description as ledgertagdescription
                           , lpd.contratagid
                           , ct.number as contratagnumber
                           , ct.description as contratagdescription
                           , lpd.ledgerdraftid
                      FROM ledgerpostdraft lpd
                      LEFT JOIN ledgertag lt ON lt.id = lpd.ledgertagid
                      LEFT JOIN ledgertag ct ON ct.id = lpd.contratagid");
    }

    public override void Down()
    {
        Execute.Sql("DROP VIEW ledgerpostdraftdto");
    }
}