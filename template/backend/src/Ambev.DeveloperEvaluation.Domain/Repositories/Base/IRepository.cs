namespace Ambev.DeveloperEvaluation.Domain.Repositories.Base;

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public int TotalItems { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
}


public interface IRepository<T> where T : class
{
    Task<PagedResult<T>> GetAllAsync(int page, int size, string? order, CancellationToken ct);
}
