namespace TaskManagement.Domain.Base
{
    public class OperationResult<T>
    {
        public string? Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public dynamic? Data { get; set; }

        public OperationResult()
        {

        }
        public OperationResult(bool isSuccess, string message, dynamic? data = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            Data = data;
        }

        public static OperationResult<T> Success(string message, dynamic? data = null)
        {
            return new OperationResult<T>(true, message, data);
        }

        public static OperationResult<T> Failure(string message)
        {
            return new OperationResult<T>(false, message);
        }
    }
}
