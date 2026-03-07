namespace ECM.Domain.Common
{
    public class OperationResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }

        public static OperationResult Ok(string message = "Operación exitosa")
        {
            return new OperationResult { Success = true, Message = message };
        }

        public static OperationResult Fail(string message)
        {
            return new OperationResult { Success = false, Message = message };
        }
    }
    
    public class OperationResult<T> : OperationResult
    {
        public T? Data { get; set; }
        
        public static OperationResult<T> Ok(T data, string message = "Operación exitosa")
        {
            return new OperationResult<T> { Success = true, Message = message, Data = data };
        }

        public new static OperationResult<T> Fail(string message)
        {
            return new OperationResult<T> { Success = false, Message = message, Data = default };
        }
    }
}