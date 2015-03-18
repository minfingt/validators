using Minfin.Validators;
using NUnit.Framework;
using System;

namespace Minfin.Validators.Tests
{
    [TestFixture]
    public class CuiValidatorTests
    {

        [Test]
        public void IsValid_WithNullCui_ThrowsArgumentNullException()
        {

            //Arrange
            var validator = new CuiValidator();

            //Act
            TestDelegate actionToExecute = () => validator.IsValid(null);

            //Assert
            Assert.That(actionToExecute, Throws.InstanceOf<ArgumentNullException>().And.Message.ContainsSubstring("cui"));

        }

        [TestCase("             ")]
        [TestCase("")]
        [TestCase("123")]
        [TestCase("123456789012a")]
        [TestCase("12345.1335468")]
        public void IsValid_WithInvalidFormat_ThrowsFormatException(string nit)
        {

            //Arrange
            var validator = new CuiValidator();

            //Act
            TestDelegate actionToExecute = () => validator.IsValid(nit);

            //Assert
            Assert.That(actionToExecute, Throws.TypeOf<FormatException>());

        }

        [TestCase("1587564440904", true)]
        [TestCase("2415751810000", false)]
        [TestCase("1580352240101", true)]
        public void IsValid_WithValidFormat_CheckResults(string cui, bool expectedResult)
        {

            //Arrange
            CuiValidator validator = new CuiValidator();

            //Act
            bool result = validator.IsValid(cui);

            //Assert
            Assert.AreEqual(expectedResult, result);

        }

    }
}
