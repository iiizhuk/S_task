using Models;
using System.Linq.Expressions;

namespace FunctionalFitting.Tests
{
    public class LinearFittingModelAlgorithmTests
    {
        private LinearFittingModelAlgorithm _sut;

        [OneTimeSetUp]
        public void OnOneTimeSetup()
        {
            _sut = new LinearFittingModelAlgorithm();
        }

        [TestCase(FittingMode.Linear, true)]
        [TestCase(FittingMode.Exponential, false)]
        [TestCase(FittingMode.Power, false)]
        public void CanExceptLinear(FittingMode regressionMode, bool expectedResult)
        {
            Assert.That(_sut.CanAccept(regressionMode), Is.EqualTo(expectedResult));
        }

        [Test]
        public void Cannot_Resolve_ForEmptyData()
        {
            Assert.Throws<ArgumentException>(() => _sut.Calculate(Array.Empty<double>(), Array.Empty<double>()));
        }

        [Test]
        public void Cannot_Resolve_ForOnePoint()
        {
            Assert.Throws<ArgumentException>(() => _sut.Calculate(new double[] { 1.0 }, new double[] { 2.0 }));
        }
        [TestCaseSource(typeof(TestDataCollection), nameof(TestDataCollection.Linear))]
        public void Can_Resolve_Exactly((double[] xValue, double[] yValues, string formula) data)
        {
            var result = _sut.Calculate(data.xValue, data.yValues);
            
            Assert.That(result.IsValid, Is.True, "Algorithm should produce the correct result.");
            Assert.That(result.FormulaDescription, Is.EqualTo(data.formula));
        }
    }
}