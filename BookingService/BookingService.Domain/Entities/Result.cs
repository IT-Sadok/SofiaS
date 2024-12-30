namespace BookingService.Domain.Entities
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }

        public static Result Success() => new() { IsSuccess = true };
        public static Result Failure(string errorMessage) => new() { IsSuccess = false, ErrorMessage = errorMessage };
    }

    public class Result<T> : Result
    {
        public T? Value { get; private set;}

        public static Result<T> Success(T value) => new Result<T> { IsSuccess = true, Value = value };
        public new Result<T> Failure(string errorMessage) => new Result<T> { IsSuccess = false, ErrorMessage = errorMessage };

    }
}