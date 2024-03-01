namespace UserManagement.WebAPI.DTO;

/// <summary>
/// Represents a paginated result containing a collection of items of type <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">The type of items contained in the paginated result.</typeparam>
public class PaginatedResult<T>
{
    /// <summary>
    /// Gets or sets the collection of items in the paginated result.
    /// </summary>
    public IEnumerable<T> Items { get; set; } = new List<T>();

    /// <summary>
    /// Gets or sets the total number of records across all pages.
    /// </summary>
    public int TotalRecords { get; set; }

    /// <summary>
    /// Gets or sets the total number of pages.
    /// </summary>
    public int TotalPages { get; set; }
}


