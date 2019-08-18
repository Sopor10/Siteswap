using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Utils.Monad;

namespace UtilsTest.Monad
{
    public class ValidateEitherTests
    {


        #region Function Returns Either<ValidationResult,TResult>

        [Test]
        public void WennFehlerfreiValidationResult_TestfunktionReturnsError_ShouldHaveError()
        {
            ValidationResultOr<Test> sut = new ValidationResultOr<Test>(new Test());
            var result = sut.Try(x => x.ReturnsValidationResultWithError("falscher String"));
            
            Assert.That(result.HasErrors(),Is.EqualTo(true));
            Assert.That(result.ValidationResult.Messages.Count, Is.EqualTo(1));
            Assert.That(result.ValidationResult.Messages.Single().Message, Is.EqualTo("falscher String war der falsche Input"));
        }
        
        [Test]
        public void WennFehlerfreiValidationResult_TestfunktionReturnsError_ShouldNotExcecute2Funktion()
        {
            ValidationResultOr<Test> sut = new ValidationResultOr<Test>(new Test());
            var result = sut.Try(x => x.ReturnsValidationResultWithError("falscher String"));
            
            result.Try<bool>(x =>
            {
                Assert.Fail($"Diese Zeile hätte nicht ausgeführt werden dürfen, da vorher ein Fehler in {nameof(Test.ReturnsValidationResultWithError)} aufgetreten ist.");
                return true;
            });
        }

        [Test]
        public void WennFehlerfreiValidationResult_TestfunktionReturnsNull_ShouldHaveError()
        {
            ValidationResultOr<Test> sut = new ValidationResultOr<Test>(new Test());
            var result = sut.Try(x => x.ReturnsNullEitherValidationResult());
            
            Assert.That(result.HasErrors(),Is.EqualTo(true));
            Assert.That(result.ValidationResult.Messages.Count, Is.EqualTo(1));
            Assert.True(result.ValidationResult.Messages.Single().Message.Contains("darf nicht null sein"));
        }
        
        [Test]
        public void WennFehlerfreiValidationResult_TestfunktionReturnsNull_ShouldNotExcecute2Funktion()
        {
            ValidationResultOr<Test> sut = new ValidationResultOr<Test>(new Test());
            var result = sut.Try(x => x.ReturnsNullEitherValidationResult());
            
            result.Try<bool>(x =>
            {
                Assert.Fail($"Diese Zeile hätte nicht ausgeführt werden dürfen, da vorher ein Fehler in {nameof(Test.ReturnsNullEitherValidationResult)} aufgetreten ist.");
                return true;
            });
        }

        [Test]
        public void WennFehlerfreiValidationResult_TestfunktionThrowsException_ShouldHaveError()
        {
            ValidationResultOr<Test> sut = new ValidationResultOr<Test>(new Test());
            var result = sut.Try(x => x.ThrowsExceptionEitherValidationResult());
            
            Assert.That(result.HasErrors(),Is.EqualTo(true));
            Assert.That(result.ValidationResult.Messages.Count, Is.EqualTo(1));
        }
        
        [Test]
        public void WennFehlerfreiValidationResult_TestfunktionTrhowsException_ShouldNotExcecute2Funktion()
        {
            ValidationResultOr<Test> sut = new ValidationResultOr<Test>(new Test());
            var result = sut.Try(x => x.ThrowsExceptionEitherValidationResult());
            
            result.Try<bool>(x =>
            {
                Assert.Fail($"Diese Zeile hätte nicht ausgeführt werden dürfen, da vorher ein Fehler in {nameof(Test.ThrowsExceptionEitherValidationResult)} aufgetreten ist.");
                return true;
            });
        }
        #endregion

        #region Normal Function
        
        [Test]
        public void WennFehlerfreiExceptionMethod_TestfunktionReturnsNull_ShouldHaveError()
        {
            ValidationResultOr<Test> sut = new ValidationResultOr<Test>(new Test());
            var result = sut.Select(x => x.ReturnsNull());
            
            Assert.That(result.HasErrors(),Is.EqualTo(true));
            Assert.That(result.ValidationResult.Messages.Count, Is.EqualTo(1));
            Assert.True(result.ValidationResult.Messages.Single().Message.Contains("nicht null"));
        }
        
        [Test]
        public void WennFehlerfreiExceptionMethod_TestfunktionReturnsNull_ShouldNotExcecute2Funktion()
        {
            ValidationResultOr<Test> sut = new ValidationResultOr<Test>(new Test());
            ValidationResultOr<string> result = sut.Select(x => x.ReturnsNull());
            
            result.Select<bool>(x =>
            {
                Assert.Fail($"Diese Zeile hätte nicht ausgeführt werden dürfen, da vorher ein Fehler in {nameof(Test.ReturnsNull)} aufgetreten ist.");
                return true;
            });
        }

        [Test]
        public void WennFehlerfreiExceptionMethod_TestfunktionThrowsException_ShouldHaveError()
        {
            ValidationResultOr<Test> sut = new ValidationResultOr<Test>(new Test());
            var result = sut.Select(x => x.ThrowsException());
            
            Assert.That(result.HasErrors(),Is.EqualTo(true));
            Assert.That(result.ValidationResult.Messages.Count, Is.EqualTo(1));
        }
        
        [Test]
        public void WennFehlerfreiExceptionMethod_TestfunktionTrhowsException_ShouldNotExcecute2Funktion()
        {
            ValidationResultOr<Test> sut = new ValidationResultOr<Test>(new Test());
            var result = sut.Select(x => x.ThrowsException());
            
            result.Select<bool>(x =>
            {
                Assert.Fail($"Diese Zeile hätte nicht ausgeführt werden dürfen, da vorher ein Fehler in {nameof(Test.ThrowsException)} aufgetreten ist.");
                return true;
            });
        }
        #endregion


        [Test]
        public void UsageFeel()
        {
            ValidationResultOr<int> test = 5;

            var result = test.Select(x => 5 * x).GetValueOrThrow();
        }

        [Test]
        public void METHOD()
        {
            ValidationResultOr<Buch> buch = new Buch(seiten:5);

            ValidationResultOr<int> result = buch.Try(x => x.Umblaettern());
            
        }
       
    }
}