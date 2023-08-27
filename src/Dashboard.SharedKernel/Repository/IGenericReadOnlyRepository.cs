using Dashboard.SharedKernel.Specifications;

namespace Dashboard.SharedKernel.Repository;

public interface IGenericReadOnlyRepository<TEntity>
{
    Task<TEntity?> GetByIdAsync(object? id);
    Task<PagedResults<TEntity>> QueryAsync(QueryFilterSpecification query);
}