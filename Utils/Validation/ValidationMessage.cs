namespace Utils.Validation
{
    public class ValidationMessage
    {
        private string message;
        private ValidationLevel level;

        public string Message => message;
        public ValidationLevel Level => level;

        public ValidationMessage(string message, ValidationLevel level)
        {
            this.message = message;
            this.level = level;
        }

        public bool IsErrorMessage => level == ValidationLevel.Error;
    }
}