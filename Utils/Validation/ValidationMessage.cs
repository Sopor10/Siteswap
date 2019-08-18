using System;

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
        public static ValidationMessage Fehler(string message)
        {
            return new ValidationMessage(message,ValidationLevel.Error);
        }

        public ValidationMessage(Exception exception)
        {
            this.message = exception.ToString();
            this.level = ValidationLevel.Error;
        }

        public bool IsErrorMessage => level == ValidationLevel.Error;
    }
}