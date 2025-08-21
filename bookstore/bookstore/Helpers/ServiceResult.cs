namespace bookstore.Helpers
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public List<string> ValidationErrors { get; set; } = new List<string>();

        public static ServiceResult CreateSuccess()
        {
            return new ServiceResult { Success = true };
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
}
