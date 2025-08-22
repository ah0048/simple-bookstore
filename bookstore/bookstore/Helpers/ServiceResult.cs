namespace bookstore.Helpers
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string SuccessMessage { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public List<string> ValidationErrors { get; set; } = new List<string>();

        public static ServiceResult CreateSuccess(string successMessage)
        {
            return new ServiceResult { Success = true, SuccessMessage = successMessage };
        }

        public static ServiceResult CreateError(string errorMessage)
        {
            return new ServiceResult { Success = false, ErrorMessage = errorMessage };
        }

        public static ServiceResult CreateValidationError(string validationError)
        {
            return new ServiceResult
            {
                Success = false,
                ValidationErrors = new List<string> { validationError }
            };
        }

        public static ServiceResult CreateValidationErrors(List<string> validationErrors)
        {
            return new ServiceResult
            {
                Success = false,
                ValidationErrors = validationErrors
            };
        }
    }

    public class ServiceResult<T>: ServiceResult
    {
        public T? Data { get; set; }
        public static ServiceResult<T> CreateSuccess(T data)
        {
            return new ServiceResult<T>
            {
                Success = true,
                Data = data
            };
        }

        public new static ServiceResult<T> CreateError(string errorMessage)
        {
            return new ServiceResult<T>
            {
                Success = false,
                ErrorMessage = errorMessage
            };
        }
        public new static ServiceResult<T> CreateValidationError(string validationError)
        {
            return new ServiceResult<T>
            {
                Success = false,
                ValidationErrors = new List<string> { validationError }
            };
        }

        public new static ServiceResult<T> CreateValidationErrors(List<string> validationErrors)
        {
            return new ServiceResult<T>
            {
                Success = false,
                ValidationErrors = validationErrors
            };
        }
    }
}
