using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Internal.Commands;
using Utils.Monad;
using Utils.Validation;

namespace UtilsTest.Monad
{
    public class Test
    {
        public ValidationResultOr<bool> ThrowsExceptionEitherValidationResult()
        {
            throw new System.NotImplementedException();
        }
        public ValidationResultOr<string> ReturnsValidationResultWithError(string input)
        {
            return ValidationResultOr<string>.Validation<string>(new ValidationResult(new ValidationMessage($"{input} war der falsche Input",ValidationLevel.Error)));
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
        public List<Seite> Seites { get; set; }
        public int aktuelleSeite { get; set; }

        public Buch(int seiten)
        {
            Seites = new List<Seite>(seiten);
//            Seites.SelectMany()
        }

        public ValidationResultOr<Buch> Umblaettern()
        {
            if (aktuelleSeite < Seites.Count)
            {
                aktuelleSeite++;
                return this;
            }

            return new ValidationResult(new ValidationMessage("Das Buch ist zuende.",ValidationLevel.Error));
        }
        
        public Buch UmblaetternInt()
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