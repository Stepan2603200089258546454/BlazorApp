namespace BlazorApp.Client.Models
{
    public class Result
    {
        public bool Success { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;

        public static Result OnSuccess()
        {
            return new Result()
            {
                Success = true,
                ErrorMessage = string.Empty,
            };
        }
        public static Result OnError(string message)
        {
            return new Result()
            {
                Success = false,
                ErrorMessage = message,
            };
        }
        public static Result OnError(Exception ex)
        {
            return new Result()
            {
                Success = false,
                ErrorMessage = ex.Message,
            };
        }
        public static Result OnCanceled(OperationCanceledException ex)
        {
            return new Result()
            {
                Success = false,
                ErrorMessage = ex.Message,
            };
        }
    }
    public class Result<T> : Result
    {
        public T? Value { get; set; }

        public static Result<T> OnSuccess(T? value)
        {
            return new Result<T>()
            {
                Success = true,
                ErrorMessage = string.Empty,
                Value = value,
            };
        }
        public static new Result<T> OnError(Exception ex)
        {
            return new Result<T>()
            {
                Success = false,
                ErrorMessage = ex.Message,
                Value = default,
            };
        }
        public static new Result<T> OnCanceled(OperationCanceledException ex)
        {
            return new Result<T>()
            {
                Success = false,
                ErrorMessage = ex.Message,
                Value = default,
            };
        }
    }
}
