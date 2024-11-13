namespace Application.Core
{
    public class Result<T>
    {
        public bool IsSuccess { get; set; }
        public T Value { get; set; } = default!;
        public string Error { get; set; } = default!;
        public int? TotalCount { get; set; }
        public int? TotalPages { get; set; }
        public int? CurrentPage { get; set; }
        public int? PageSize { get; set; }
        public bool? HasPreviousPage { get; set; }
        public bool? HasNextPage { get; set; }

        public static Result<T> Success(
            T value,
            int? totalCount = null,
            int? totalPages = null,
            int? currentPage = null,
            int? pageSize = null,
            bool? hasPreviousPage = null,
            bool? hasNextPage = null
        ) =>
            new Result<T>
            {
                IsSuccess = true,
                Value = value,
                TotalCount = totalCount,
                TotalPages = totalPages,
                CurrentPage = currentPage,
                PageSize = pageSize,
                HasPreviousPage = hasPreviousPage,
                HasNextPage = hasNextPage
            };

        public static Result<T> Failure(string error) =>
            new Result<T> { IsSuccess = false, Error = error };
    }
}
