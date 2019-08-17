using System.Linq;
using NUnit.Framework;
using Utils.Monad;
using Utils.Validation;

namespace UtilsTest.Monad
{
    public class ValidateEitherTests
    {
       
        [Test]
        public void WennFehlerfreiValidationMessage_TestfunktionReturnsError_ShouldHaveError()
        {
            ValidateEither<Test> sut = new ValidateEither<Test>(new Test());
            var result = sut.Try(x => x.Testfunktion("falscher String"));
            
            Assert.That(result.HasErrors(),Is.EqualTo(true));
            Assert.That(result.ValidationResult.Messages.Count, Is.EqualTo(1));
            Assert.That(result.ValidationResult.Messages.Single().Message, Is.EqualTo("falscher String war der falsche Input"));
        }
        
        [Test]
        public void WennFehlerfreiValidationMessage_TestfunktionReturnsError_ShouldNotExcecute2Funktion()
        {
            ValidateEither<Test> sut = new ValidateEither<Test>(new Test());
            var result = sut.Try(x => x.Testfunktion("falscher String"));
            
            result.Try<bool>(x =>
            {
                Assert.Fail($"Diese Zeile hätte nicht ausgeführt werden dürfen, da vorher ein Fehler in {nameof(Test.Testfunktion)} aufgetreten ist.");
                return true;
            });
        }

        [Test]
        public void WennFehlerfreiValidationMessage_TestfunktionReturnsNull_ShouldHaveError()
        {
            ValidateEither<Test> sut = new ValidateEither<Test>(new Test());
            var result = sut.Try(x => x.ReturnsNull());
            
            Assert.That(result.HasErrors(),Is.EqualTo(true));
            Assert.That(result.ValidationResult.Messages.Count, Is.EqualTo(1));
            Assert.True(result.ValidationResult.Messages.Single().Message.Contains("System.String in Funktion Utils.Monad.Either`2["));
        }
        
        [Test]
        public void WennFehlerfreiValidationMessage_TestfunktionReturnsNull_ShouldNotExcecute2Funktion()
        {
            ValidateEither<Test> sut = new ValidateEither<Test>(new Test());
            var result = sut.Try(x => x.ReturnsNull());
            
            result.Try<bool>(x =>
            {
                Assert.Fail($"Diese Zeile hätte nicht ausgeführt werden dürfen, da vorher ein Fehler in {nameof(Test.Testfunktion)} aufgetreten ist.");
                return true;
            });
        }

        [Test]
        public void WennFehlerfreiValidationMessage_TestfunktionThrowsException_ShouldHaveError()
        {
            ValidateEither<Test> sut = new ValidateEither<Test>(new Test());
            var result = sut.Try(x => x.ThrowsException());
            
            Assert.That(result.HasErrors(),Is.EqualTo(true));
            Assert.That(result.ValidationResult.Messages.Count, Is.EqualTo(1));
            Assert.True(result.ValidationResult.Messages.Single().Message.Contains("System.String in Funktion Utils.Monad.Either`2["));
        }
        
        [Test]
        public void WennFehlerfreiValidationMessage_TestfunktionTrhowsException_ShouldNotExcecute2Funktion()
        {
            ValidateEither<Test> sut = new ValidateEither<Test>(new Test());
            var result = sut.Try(x => x.ThrowsException());
            
            result.Try<bool>(x =>
            {
                Assert.Fail($"Diese Zeile hätte nicht ausgeführt werden dürfen, da vorher ein Fehler in {nameof(Test.Testfunktion)} aufgetreten ist.");
                return true;
            });
        }
        [Test]
        public void WennFehlerfreiValidationMessage_ErsteFunktionMitInfoZweiteFunktionMitFehler_ValidationResultShouldContainBothMessages()
        {
            ValidateEither<Test> sut = new ValidateEither<Test>(new Test());
            var result = sut.Try(x => x.ReturnsInfo());
            
            result.Try<bool>(x =>
            {
                Assert.Fail($"Diese Zeile hätte nicht ausgeführt werden dürfen, da vorher ein Fehler in {nameof(Test.ReturnsInfo)} aufgetreten ist.");
                return true;
            });
        }

        [Test]
        public void UsageFeelTest3()
        {
            ValidateEither<Test> sut = new ValidateEither<Test>(new Test());
            var result = sut.Try(x => x.Testfunktion("test"));
            
            Assert.That(sut.HasErrors(),Is.EqualTo(false));
        }

        public class Test
        {
            public Either<ValidationMessage, string> Testfunktion(string input)
            {
                if (input == "test")
                {
                    return "success";
                }

                return new ValidationMessage($"{input} war der falsche Input",ValidationLevel.Error);
            }
            
            public Either<ValidationMessage, string> ReturnsInfo()
            {
                return new ValidationMessage($"info",ValidationLevel.Info);
            }
            
            public Either<ValidationMessage, string> ReturnsNull()
            {
                return null;
            }

            public Either<ValidationMessage,bool> ThrowsException()
            {
                throw new System.NotImplementedException();
            }
        }
    }
}