namespace Anex.Api.Utilities;

public interface IStrategy<TCriteria>
{
    bool IsApplicable(TCriteria criteria);
    void Execute(TCriteria criteria);
}
public interface IStrategy<TCriteria, TResult>
{
    bool IsApplicable(TCriteria criteria);
    TResult Execute(TCriteria criteria);
}