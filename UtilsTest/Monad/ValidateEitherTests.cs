using System;
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
                return ValidationResultOr<bool>.Some<bool>(true);
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
                return ValidationResultOr<bool>.Some(true);
            });
        }
        #endregion

        #region Normal Function
        
        [Test]
        public void WennFehlerfreiExceptionMethod_TestfunktionReturnsNull_ShouldHaveError()
        {
            ValidationResultOr<Test> sut = new ValidationResultOr<Test>(new Test());
            var result = sut.Try(x => x.ReturnsNull());
            
            Assert.That(result.HasErrors(),Is.EqualTo(true));
            Assert.That(result.StammtAusNullZuweisung,Is.EqualTo(true));
            Assert.That(result.ValidationResult.Messages.Count, Is.EqualTo(1));
            Assert.True(result.ValidationResult.Messages.Single().Message.Contains("nicht null"));
        }
        
        [Test]
        public void WennFehlerfreiExceptionMethod_TestfunktionReturnsNull_ShouldNotExcecute2Funktion()
        {
            ValidationResultOr<Test> sut = new ValidationResultOr<Test>(new Test());
            ValidationResultOr<string> result = sut.Try(x => x.ReturnsNull());
            
            var result2 = result.Try<bool>(x =>
            {
                Assert.Fail($"Diese Zeile hätte nicht ausgeführt werden dürfen, da vorher ein Fehler in {nameof(Test.ReturnsNull)} aufgetreten ist.");
                return true;
            });
            Assert.That(result.StammtAusNullZuweisung,Is.EqualTo(true));
            Assert.That(result2.StammtAusNullZuweisung,Is.EqualTo(true));
        }

        [Test]
        public void WennFehlerfreiExceptionMethod_TestfunktionThrowsException_ShouldHaveError()
        {
            ValidationResultOr<Test> sut = new ValidationResultOr<Test>(new Test());
            var result = sut.Try(x => x.ThrowsException());
            
            Assert.That(result.HasErrors(),Is.EqualTo(true));
            Assert.That(result.ValidationResult.Messages.Count, Is.EqualTo(1));
        }
        
        [Test]
        public void WennFehlerfreiExceptionMethod_TestfunktionTrhowsException_ShouldNotExcecute2Funktion()
        {
            ValidationResultOr<Test> sut = new ValidationResultOr<Test>(new Test());
            var result = sut.Try(x => x.ThrowsException());
            
            result.Try<bool>(x =>
            {
                Assert.Fail($"Diese Zeile hätte nicht ausgeführt werden dürfen, da vorher ein Fehler in {nameof(Test.ThrowsException)} aufgetreten ist.");
                return true;
            });
        }
        #endregion


        [Test]
        public void UsageFeel()
        {
            ValidationResultOr<int> test = ValidationResultOr<int>.Some(5);

            var result = test.Try(x => 5 * x).GetValueOrThrow();
        }

        [Test]
        public void UsageTest()
        {
            ValidationResultOr<Buch> buch = ValidationResultOr<Buch>.Some(new Buch(seiten:5));

            ValidationResultOr<Buch> result = buch.Try(x => x.Umblaettern());
            ValidationResultOr<Buch> result2 = buch.Try(x => x.UmblaetternInt());
            var result3 = buch.Try(x => x.UmblaetternInt())
                .Try(x => x.UmblaetternInt())
                .Try(x => x.Umblaettern())
                .Try(x => x.UmblaetternInt())
                .Try(x => x.UmblaetternInt())
                .Try(x => x.UmblaetternInt())
                .Try(x => x.UmblaetternInt())
                .Try(x =>
                {
                    Assert.Fail();
                    return true;
                });
            
            
            Assert.That(result3.HasErrors);
            Assert.Throws<InvalidOperationException>(()=>result3.GetValueOrThrow());

        }
       
    }
}