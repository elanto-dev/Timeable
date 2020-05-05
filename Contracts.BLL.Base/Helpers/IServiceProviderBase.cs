namespace Contracts.BLL.Base.Helpers
{
    public interface IServiceProviderBase
    {
        TService GetService<TService>();
    }
}
