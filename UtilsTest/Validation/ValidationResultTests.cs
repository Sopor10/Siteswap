using System;
using NUnit.Framework;
using Utils.Validation;

namespace UtilsTest.Validation
{
    public class ValidationResultTests
    {
        [Test]
        public void ValidationResult_AddFehler_HasErrorShouldReturnTrue()
        {
            ValidationResult sut = new ValidationResult();
            sut.Add(new ValidationMessage(String.Empty, ValidationLevel.Error));
           
            Assert.That(sut.HasErrors,Is.EqualTo(true));
        }
        
        [Test]
        public void ValidationResult_AddWarning_HasErrorShouldReturnFalse()
        {
            ValidationResult sut = new ValidationResult();
            sut.Add(new ValidationMessage(String.Empty, ValidationLevel.Warning));

            Assert.That(sut.HasErrors,Is.EqualTo(false));
        }
        
        [Test]
        public void ValidationResult_AddInfo_HasErrorShouldReturnFalse()
        {
            ValidationResult sut = new ValidationResult();
            sut.Add(new ValidationMessage(String.Empty, ValidationLevel.Info));

            Assert.That(sut.HasErrors,Is.EqualTo(false));
        }
        
        [Test]
        public void ValidationResult_Merge_ShouldCopyMessages()
        {
            
            
            ValidationResult other = new ValidationResult();
            other.Add(new ValidationMessage("newMessage", ValidationLevel.Error));
            
            ValidationResult sut = new ValidationResult();
            sut.Add(new ValidationMessage("oldMessage", ValidationLevel.Info));
            
            sut.Merge(other);

            Assert.That(sut.HasErrors,Is.EqualTo(true));
        }
    }
}