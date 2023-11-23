using System;
using System.Linq;
using System.Threading.Tasks;
using Anex.Api.Database.Queries;
using Anex.Api.Dto;
using Anex.Domain;
using NHibernate;

namespace Anex.Api.Database.Commands;

public class CreateFiscalPeriodCommand : BaseCreateCommand<FiscalPeriod>
{
    private readonly EditableFiscalPeriodDto _dto;

    public CreateFiscalPeriodCommand(EditableFiscalPeriodDto dto)
    {
        _dto = dto;
    }

    protected override async Task<QueryResult<FiscalPeriod>> CreateEntity(ISession session)
    {
        var existingPeriods = await session
            .QueryOver<FiscalPeriod>()
            .ListAsync();

        var overlappingPeriods = existingPeriods
            .Where(fp => fp.EndDate >= _dto.StartDate)
            .Where(fp => fp.StartDate <= _dto.EndDate)
            .ToList();

        if (overlappingPeriods.Any())
        {
            return new QueryResult<FiscalPeriod>($"Fiscal periods are not allowed to overlap. {_dto.StartDate.ToShortDateString()} - {_dto.EndDate.ToShortDateString()} overlaps with the following periods:{Environment.NewLine}{string.Join(Environment.NewLine, overlappingPeriods.Select(fp => fp.ToString()))}");
        }
        
        var priorPeriod = existingPeriods
            .Where(fp => fp.StartDate <= _dto.StartDate)
            .MaxBy(fp => fp.StartDate);
        if (priorPeriod != null && priorPeriod.EndDate.AddDays(1) != _dto.StartDate)
        {
            return new QueryResult<FiscalPeriod>($"The next fiscal period after {priorPeriod} should start on {priorPeriod.EndDate.AddDays(1).ToShortDateString()}");
        }

        var laterPeriod = existingPeriods
            .Where(fp => fp.StartDate >= _dto.EndDate)
            .MinBy(fp => fp.StartDate);
        if (laterPeriod != null && laterPeriod.StartDate.AddDays(-1) != _dto.EndDate)
        {
            return new QueryResult<FiscalPeriod>($"The prior fiscal period after {laterPeriod} should end on {laterPeriod.StartDate.AddDays(-1).ToShortDateString()}");
        }
        
        var fiscalPeriod = FiscalPeriod.Create(_dto.StartDate, _dto.EndDate);
        return new QueryResult<FiscalPeriod>(fiscalPeriod);
    }
}