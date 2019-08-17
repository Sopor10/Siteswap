using System;
using Utils.Validation;

namespace Utils.Monad
{
    public class ValidateEither<TRight> 
    {
        private readonly ValidationResult validationResult;
        private readonly TRight right;
        private readonly bool hasValue;

        private ValidateEither(ValidationResult validationResult)
        {
            this.validationResult = validationResult;
            hasValue = false;
        }

        public ValidateEither( TRight right)
        {
            this.right = right;
            hasValue = true;
            validationResult = new ValidationResult();
        }

        private ValidateEither(ValidationResult result, TRight right)
        {
            validationResult = result;
            this.right = right;
            hasValue = true;
        }
        
        public static implicit operator ValidateEither<TRight>(TRight right) => new ValidateEither<TRight>(right);
        public ValidationResult ValidationResult => validationResult;

        public ValidateEither<TResult> WennFehlerfrei<TResult>(Func<TRight, TResult> func)
       {
           if (!hasValue || validationResult.HasErrors())
           {
               return new ValidateEither<TResult>(validationResult);
           }
           try
           {
               var result = func.Invoke(right);
               if (result != null)
               {
                   return new ValidateEither<TResult>(validationResult, result);
               }
               validationResult.Add(new ValidationMessage($"{typeof(TResult)} darf nicht null sein.",ValidationLevel.Error));
               return new ValidateEither<TResult>(validationResult);
           }
           catch (Exception e)
           {
               validationResult.Add(new ValidationMessage(e.Message,ValidationLevel.Error));
           }
           return new ValidateEither<TResult>(validationResult);
       }
        
        public ValidateEither<TResult> Try<TResult>(Func<TRight, Either<ValidationMessage,TResult>> func)
        {
            var newValidationResult = new ValidationResult();
            newValidationResult.Merge(validationResult);
            
            if (!hasValue)
            {
                return new ValidateEither<TResult>(newValidationResult);
            }

            Either<ValidationMessage, TResult> result = func.Invoke(right);
            if (result == null)
            {
                result = new ValidationMessage($"{typeof(TResult)} in Funktion {func.Method} darf nicht null sein",
                    ValidationLevel.Error);
            }

            if (result.IsLeft)
            {
                newValidationResult.Add(result.GetLeftUnsafe());
            }
            else
            {
                var rightUnsafe = result.GetRightUnsafe();
                if (rightUnsafe != null)
                {
                    return new ValidateEither<TResult>(newValidationResult, rightUnsafe);
                }
                validationResult.Add(new ValidationMessage($"{typeof(TResult)} darf nicht null sein.",ValidationLevel.Error));
            }
            return new ValidateEither<TResult>(newValidationResult);
        }

        public bool HasErrors()
        {
            return validationResult.HasErrors();
        }

        public void ThrowIfInvalid()
        {
            validationResult.ThrowIfInvalid();
        }

        public TRight GetValue()
        {
            ThrowIfInvalid();
            return right;
        }
    }
}