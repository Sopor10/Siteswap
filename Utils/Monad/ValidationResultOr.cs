using System;
using Utils.Validation;

namespace Utils.Monad
{
    public struct ValidationResultOr<TRight> 
    {
        private readonly bool stammtAusNullZuweisung;
        private Either<ValidationResult, TRight> either;
        public static implicit operator ValidationResultOr<TRight>(TRight right) => new ValidationResultOr<TRight>(right);

        public static implicit operator ValidationResultOr<TRight>(ValidationResult validationResult) => new ValidationResultOr<TRight>(validationResult);

        public static implicit operator ValidationResultOr<TRight>(ValidationMessage message) => new ValidationResultOr<TRight>(new ValidationResult(message));
        
        public static implicit operator ValidationResultOr<TRight>(Either<ValidationResult,TRight> either) => new ValidationResultOr<TRight>(either);
        public static ValidationResultOr<TResult> Some<TResult>(TResult result)
        {
            if (result == null)
            {
                return new ValidationResultOr<TResult>(new ValidationResult(new ValidationMessage($"Der Typ {typeof(TResult)} darf nicht null sein",
                    ValidationLevel.Error)), true);
            }
            return new ValidationResultOr<TResult>(result);
        } 
        
        public static ValidationResultOr<TResult> Validation<TResult>(ValidationResult result, bool stammtAusNullZuweisung = false)
        {
            if (result == null)
            {
                return new ValidationResultOr<TResult>(new ValidationResult(new ValidationMessage($"Das Validationresult darf nicht null sein",
                    ValidationLevel.Error)), true);
            }
            return new ValidationResultOr<TResult>(result,stammtAusNullZuweisung);
        } 
            
        private ValidationResultOr(ValidationResult validationResult, bool stammtAusNullZuweisung = false)
        {
            either = new Either<ValidationResult, TRight>(validationResult);
            this.stammtAusNullZuweisung = stammtAusNullZuweisung;
        }

        public ValidationResultOr( TRight right) 
        {
            either = new Either<ValidationResult, TRight>(right);
            stammtAusNullZuweisung = false;
        }

        private ValidationResultOr(Either<ValidationResult,TRight> either)
        {
            this.either = either;
            stammtAusNullZuweisung = !either.IsLeft && either.GetRightUnsafe() != null;
        }

        public ValidationResult ValidationResult => either.GetLeftUnsafe();

        public bool StammtAusNullZuweisung => stammtAusNullZuweisung;

        public ValidationResultOr<TResult> Try<TResult>(Func<TRight, TResult> func) 
        {
            if (either.IsLeft)
            {
                return Validation<TResult>(ValidationResult, stammtAusNullZuweisung);
            }
            
            if (func == null)
            {
                return new ValidationResultOr<TResult>(new ValidationResult(ValidationMessage.Fehler("Hier darf kein null stehen")));
            }
            
            try
            {
                TResult resultFromFunction = func.Invoke(either.GetRightUnsafe());

                return Some(resultFromFunction);
            }
            catch (Exception e)
            {
                return Validation<TResult>(new ValidationResult( new ValidationMessage(e)));
            }
        }
       
        public ValidationResultOr<TResult> Try<TResult>(Func<TRight, ValidationResultOr<TResult>> func)
        {
            if (func == null)
            {
                return Validation<TResult>(new ValidationResult(ValidationMessage.Fehler("Hier darf kein null stehen")));
            }
            
            if (either.IsLeft)
            {
                return Validation<TResult>(either.GetLeftUnsafe(),stammtAusNullZuweisung);
            }

            try
            {
                ValidationResultOr<TResult> result = func.Invoke(either.GetRightUnsafe());

                if (result.either.IsLeft || result.either.GetRightUnsafe() != null)
                {
                    return result;
                }
                var validationMessage = new ValidationMessage($"{typeof(TResult)} darf nicht null sein.", ValidationLevel.Error);
                return Validation<TResult>(new ValidationResult(validationMessage),true);

            }
            catch (Exception e)
            {
                var result = new ValidationMessage(e);
                return new ValidationResultOr<TResult>(new ValidationResult(result));
            }
        }

        public bool HasErrors()
        {
            return either.IsLeft&& either.GetLeftUnsafe().HasErrors();
        }

        public void ThrowIfInvalid()
        {
            either.DoLeft(x => x.ThrowIfInvalid());
        }

        public TRight GetValueOrThrow()
        {
            ThrowIfInvalid();
            return either.GetRightUnsafe();
        }
    }
}