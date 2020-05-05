namespace Contracts.DAL.Base.Mappers
{
    public interface IBaseDalMapper
    {
        TOutObject Map<TOutObject>(object inObject)
            where TOutObject : class;
    }
}
