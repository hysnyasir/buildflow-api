using BuildFlow.SharedKernel;

namespace BuildFlow.SharedKernel.Pagination;

/// <summary>
/// Base class for paginated query parameters.
/// Derive from this in Application query records that return paged results.
/// </summary>
public abstract class PagedRequest
{
    private int _pageSize = AppConstants.Pagination.DefaultPageSize;
    private int _pageNumber = 1;

    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > AppConstants.Pagination.MaxPageSize
            ? AppConstants.Pagination.MaxPageSize
            : value < 1 ? 1 : value;
    }

    public int Skip => (PageNumber - 1) * PageSize;
}
