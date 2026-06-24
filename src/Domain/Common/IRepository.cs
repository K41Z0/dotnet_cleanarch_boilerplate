namespace Domain.Common;

public interface IRepository<T, in TFilter>
    where T : Entity
    where TFilter : Filter
{
    Task<Result<T>> GetAsync(string id, CancellationToken cancellationToken = default);
    Task<Result<Page<T>>> SearchAsync(TFilter filter, CancellationToken cancellationToken = default);
}
