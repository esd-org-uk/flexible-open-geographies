namespace Esd.FlexibleOpenGeographies
{
    public interface IUnitOfWorkWithResult<T>
    {
        T ExecuteWithResult();
    }
}
