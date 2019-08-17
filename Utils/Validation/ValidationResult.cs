using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils.Validation
{
    public sealed class ValidationResult
    {
        private readonly List<ValidationMessage> validationMessages;

        public ValidationResult()
        {
            validationMessages = new List<ValidationMessage>();
        }
        
        public List<ValidationMessage> Messages => validationMessages;

        public void Add(ValidationMessage validationMessage)
        {
            validationMessages.Add(validationMessage);
        }

        public bool HasErrors()
        {
            return validationMessages.Any(x => x.IsErrorMessage);
        }

        public void Merge(ValidationResult other)
        {
            foreach (var message in other.validationMessages)
            {
                Add(message);
            }
        }

        public void ThrowIfInvalid()
        {
            if (!HasErrors()) return;
            string result = string.Empty;
            foreach (var message in Messages)
            {
                result += message.Message + "\n";
            }
            throw new InvalidOperationException(result);
        }
    }
}