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
            ValidationResultOr<Test> sut = new Test();
            var result = sut.Try(x => x.ReturnsValidationResultWithError("falscher String"));
            
            Assert.That(result.HasErrors(),Is.EqualTo(true));
            Assert.That(result.ValidationResult.Messages.Count, Is.EqualTo(1));
            Assert.That(result.ValidationResult.Messages.Single().Message, Is.EqualTo("falscher String war der falsche Input"));
        }
        
        [Test]
        public void WennFehlerfreiValidationResult_TestfunktionReturnsError_ShouldNotExcecute2Funktion()
        {
            ValidationResultOr<Test> sut = new Test();
            var result = sut.Try(x => x.ReturnsValidationResultWithError("falscher String"));
            
            result.Try(x =>
            {
                Assert.Fail($"Diese Zeile hätte nicht ausgeführt werden dürfen, da vorher ein Fehler in {nameof(Test.ReturnsValidationResultWithError)} aufgetreten ist.");
                return true;
            });
        }

        [Test]
        public void WennFehlerfreiValidationResult_TestfunktionThrowsException_ShouldHaveError()
        {
            ValidationResultOr<Test> sut = new Test();
            var result = sut.Try(x => x.ThrowsExceptionEitherValidationResult());
            
            Assert.That(result.HasErrors(),Is.EqualTo(true));
            Assert.That(result.ValidationResult.Messages.Count, Is.EqualTo(1));
        }
        
        [Test]
        public void WennFehlerfreiValidationResult_TestfunktionTrhowsException_ShouldNotExcecute2Funktion()
        {
            ValidationResultOr<Test> sut = new Test();
            var result = sut.Try(x => x.ThrowsExceptionEitherValidationResult());
            
            result.Try(x =>
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
            ValidationResultOr<Test> sut = new Test();
            var result = sut.Try(x => x.ReturnsNull());
            
            Assert.That(result.HasErrors(),Is.EqualTo(true));
            Assert.That(result.IsNull,Is.EqualTo(true));
            Assert.That(result.ValidationResult.Messages.Count, Is.EqualTo(1));
            Assert.True(result.ValidationResult.Messages.Single().Message.Contains("hat null zurückgegeben"));
        }
        
        [Test]
        public void WennFehlerfreiExceptionMethod_TestfunktionReturnsNull_ShouldNotExcecute2Funktion()
        {
            ValidationResultOr<Test> sut = new Test();
            ValidationResultOr<string> result = sut.Try(x => x.ReturnsNull());
            
            var result2 = result.Try(x =>
            {
                Assert.Fail($"Diese Zeile hätte nicht ausgeführt werden dürfen, da vorher ein Fehler in {nameof(Test.ReturnsNull)} aufgetreten ist.");
                return true;
            });
            Assert.That(result.IsNull,Is.EqualTo(true));
            Assert.That(result2.IsNull,Is.EqualTo(true));
        }

        [Test]
        public void WennFehlerfreiExceptionMethod_TestfunktionThrowsException_ShouldHaveError()
        {
            ValidationResultOr<Test> sut = new Test();
            var result = sut.Try(x => x.ThrowsException());
            
            Assert.That(result.HasErrors(),Is.EqualTo(true));
            Assert.That(result.ValidationResult.Messages.Count, Is.EqualTo(1));
        }
        
        [Test]
        public void WennFehlerfreiExceptionMethod_TestfunktionTrhowsException_ShouldNotExcecute2Funktion()
        {
            ValidationResultOr<Test> sut = new Test();
            var result = sut.Try(x => x.ThrowsException());
            
            result.Try(x =>
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

            var result = test.Try(x => 5 * x).GetValueOrThrow();
        }

        [Test]
        public void UsageTest()
        {
            ValidationResultOr<Buch> buch = new Buch(seiten:5);

            ValidationResultOr<Buch> result = buch.Try(x => x.UmblaetternValidationResult());
            ValidationResultOr<Buch> result2 = buch.Try(x => x.UmblaetternNormal());
            buch.Try(x => x.UmblaetternNormal())
                .Try(x => x.UmblaetternNormal())
                .Try(x => x.UmblaetternValidationResult())
                .Try(x => x.UmblaetternNormal())
                .Try(x => x.UmblaetternNormal())
                .Try(x => x.UmblaetternNormal())
                .Try(x => x.UmblaetternNormal())
                .Try(x => Assert.Fail());
        }
        
        [Test]
        public void UsageTest2()
        {
            ValidationResultOr<Buch> buch = new Buch(seiten:5);
            var result = buch.Try(x => x.UmblaetternValidationResult());
            var result2 = result.Try(x => x.UmblaetternNormal());
            var result3 = result2.Try(x => x.UmblaetternValidationMessage());
        }
    }
}