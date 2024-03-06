using System.Globalization;
using System.Linq.Expressions;
using System.Text;

namespace Common.Tests
{
    public class TextExpressionBodyVisitorTests
    {
        private StringBuilder _builder;
        private TextExpressionBodyVisitor _sut;

        [SetUp]
        public void Setup()
        {
            _builder = new StringBuilder();
            _sut = new TextExpressionBodyVisitor(_builder, new ExpressionToTextParameters(x => x.ToString(CultureInfo.InvariantCulture)));
        }

        [Test]
        public void ComplexFunction()
        {
            Expression<Func<double, double>> expression = x => 
                10.9 * Math.Exp(45.456 * Math.Pow(3, x) + 3 * x * Math.Exp(45.456 * x));

            _sut.Visit(expression.Body);

            var actual = _builder.ToString();
            Assert.That(actual, Is.EqualTo("10.9 * exp(45.456 * (3 ^ x) + 3 * x * exp(45.456 * x))"));
        }   
        
        [Test]
        public void LinearFunction()
        {
            Expression<Func<double, double>> expression = x => 10.3 * x + 45;

            _sut.Visit(expression.Body);

            var actual = _builder.ToString();
            Assert.That(actual, Is.EqualTo("10.3 * x + 45"));
        }

        [Test]
        public void PowerFunction()
        {
            Expression<Func<double, double>> expression = x => 10 * Math.Pow(3, x) + 45;

            _sut.Visit(expression.Body);

            var actual = _builder.ToString();
            Assert.That(actual, Is.EqualTo("10 * (3 ^ x) + 45"));
        }

        [Test]
        public void ExponentialFunction()
        {
            Expression<Func<double, double>> expression = x => 10.9 * Math.Exp(45.456 * x);

            _sut.Visit(expression.Body);

            var actual = _builder.ToString();
            Assert.That(actual, Is.EqualTo("10.9 * exp(45.456 * x)"));
        }
    }
}