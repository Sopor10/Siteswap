using System;
using System.Collections.Generic;
using Utils.Monad;
using Utils.Validation;

namespace UtilsTest.Monad
{
    public class Test
    {
        public ValidationResultOr<bool> ThrowsExceptionEitherValidationResult()
        {
            throw new NotImplementedException();
        }
        public ValidationResultOr<string> ReturnsValidationResultWithError(string input)
        {
            return new ValidationResult(new ValidationMessage($"{input} war der falsche Input",ValidationLevel.Error));
        }
            
        public string ReturnsNull()
        {
            return null;
        }
            
        public bool ThrowsException()
        {
            throw new System.NotImplementedException();
        }
    }

    public class Buch
    {
        public Guid Id { get; set; }
        public List<Seite> Seites { get; }
        public int aktuelleSeite { get; set; }

        public Buch(int seiten)
        {
            Seites = new List<Seite>(seiten);
        }

        public ValidationResultOr<Buch> UmblaetternValidationResult()
        {
            if (aktuelleSeite < Seites.Count)
            {
                aktuelleSeite++;
                return this;
            }

            return new ValidationResult(new ValidationMessage("Das Buch ist zuende.",ValidationLevel.Error));
        }
        
        public ValidationResultOr<Buch> UmblaetternValidationMessage()
        {
            if (aktuelleSeite < Seites.Count)
            {
                aktuelleSeite++;
                return this;
            }

            return new ValidationMessage("Das Buch ist zuende.",ValidationLevel.Error);
        }
        
        public Buch UmblaetternNormal()
        {
            if (aktuelleSeite < Seites.Count)
            {
                aktuelleSeite++;
                return this;
            }

            throw new InvalidOperationException("Das Buch ist zuende.");
        }
    }

    public class Seite
    {
    }
}