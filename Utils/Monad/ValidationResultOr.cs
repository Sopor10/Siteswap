using System;
using Utils.Validation;

namespace Utils.Monad
{
    public struct ValidationResultOr<TRight> 
    {
        private Either<ValidationResult, TRight> either;
        
        public static implicit operator ValidationResultOr<TRight>(TRight right) => new ValidationResultOr<TRight>(right);

        public static implicit operator ValidationResultOr<TRight>(ValidationResult validationResult) => new ValidationResultOr<TRight>(validationResult);

        public static implicit operator ValidationResultOr<TRight>(ValidationMessage message) => new ValidationResultOr<TRight>(new ValidationResult(message));
        
        public static implicit operator ValidationResultOr<TRight>(Either<ValidationResult,TRight> either) => new ValidationResultOr<TRight>(either);
          
        private ValidationResultOr(ValidationResult validationResult, bool stammtAusNullZuweisung = false)
        {
            either = new Either<ValidationResult, TRight>(validationResult);
            this.IsNull = stammtAusNullZuweisung;
        }

        private ValidationResultOr( TRight right) 
        {
            either = new Either<ValidationResult, TRight>(right);
            IsNull = false;
        }

        private ValidationResultOr(Either<ValidationResult,TRight> either)
        {
            this.either = either;
            IsNull = !either.IsLeft && either.GetRightUnsafe() != null;
        }

        public ValidationResult ValidationResult => either.GetLeftUnsafe();

        public bool IsNull { get; }

        public ValidationResultOr<TResult> Try<TResult>(Func<TRight, TResult> func) 
        {
            if (either.IsLeft)
            {
                return new ValidationResultOr<TResult>(ValidationResult, IsNull);
            }
            
            if (func == null)
            {
                return new ValidationResultOr<TResult>(new ValidationResult(ValidationMessage.Fehler("Hier darf kein null stehen")));
            }
            
            try
            {
                TResult resultFromFunction = func.Invoke(either.GetRightUnsafe());

                if (resultFromFunction == null)
                {
                    return new ValidationResultOr<TResult>(new ValidationResult(new ValidationMessage($"Die Funktion {func.Method} hat null zurückgegeben",ValidationLevel.Error)),true);
                }

                return resultFromFunction;
            }
            catch (Exception e)
            {
                return new ValidationResult( new ValidationMessage(e));
            }
        }
       
        public ValidationResultOr<TResult> Try<TResult>(Func<TRight, ValidationResultOr<TResult>> func)
        {
            if (either.IsLeft)
            {
                return new ValidationResultOr<TResult>(either.GetLeftUnsafe(),IsNull);
            }
            
            if (func == null)
            {
                return new ValidationResultOr<TResult>(new ValidationResult(ValidationMessage.Fehler("Hier darf kein null stehen")));
            }
            
            try
            {
                ValidationResultOr<TResult> result = func.Invoke(either.GetRightUnsafe());

                if (result.either.IsLeft || result.either.GetRightUnsafe() != null)
                {
                    return result;
                }
                var validationMessage = new ValidationMessage($"{typeof(TResult)} darf nicht null sein.", ValidationLevel.Error);
                return new ValidationResultOr<TResult>(new ValidationResult(validationMessage),true);

            }
            catch (Exception e)
            {
                var result = new ValidationMessage(e);
                return new ValidationResultOr<TResult>(new ValidationResult(result));
            }
        }

        public void Try(Action<TRight> func)
        {
            if (either.IsLeft)
            {
                return;
            }

            func?.Invoke(either.GetRightUnsafe());
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