namespace Biplov.Common.Core
{
    /// <summary>
    /// Every aggregate root can have its own repository for data persistence and data read.
    /// Further functionality can be added by the interfaces that inherit this interface
    /// </summary>
    /// <typeparam name="T">Aggregate root</typeparam>
    public interface IRepository<T> where T: Entity
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
