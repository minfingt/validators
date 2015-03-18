using Minfin.Validators;
using NUnit.Framework;
using System;

namespace Minfin.Validators.Tests
{
    [TestFixture]
    public class NitValidatorTests
    {

        [Test]
        public void IsValid_WithNullNit_ThrowsArgumentNullException()
        {

            //Arrange
            string nit = null;
            NitValidator validator = new NitValidator();

            //Act
            TestDelegate actionToExecute = () => validator.IsValid(nit);

            //Assert
            Assert.That(actionToExecute, Throws.TypeOf<ArgumentNullException>().With.Message.ContainsSubstring("nit"));

        }

        [TestCase("         ")]
        [TestCase("       - ")]
        [TestCase("-")]
        [TestCase("a-k")]
        [TestCase("3526316-")]
        [TestCase("35263164-8")]
        [TestCase("35263164-K")]
        public void IsValid_WithInvalidFormat_ThrowsFormatException(string nit)
        {

            //Arrange
            NitValidator validator = new NitValidator();

            //Act
            TestDelegate actionToExecute = () => validator.IsValid(nit);

            //Assert
            Assert.That(actionToExecute, Throws.TypeOf<FormatException>().With.Message.ContainsSubstring("#######K"));

        }

        [TestCase("2725010", true)]
        [TestCase("1137530", true)]
        [TestCase("1162691", true)]
        [TestCase("1175262", true)]
        [TestCase("11872683", true)]
        [TestCase("11898534", true)]
        [TestCase("35263164", true)]
        [TestCase("12052655", true)]
        [TestCase("12049956", true)]
        [TestCase("12060097", true)]
        [TestCase("1009117", true)]
        [TestCase("900028", true)]
        [TestCase("13300318", true)]
        [TestCase("988842K", true)]
        [TestCase("959762K", true)]
        [TestCase("847785K", true)]
        [TestCase("835782K", true)]
        [TestCase("828069K", true)]
        [TestCase("123456", false)]
        [TestCase("859078", false)]
        public void IsValid_WithValidFormat_CheckResults(string nit, bool expectedResult)
        {

            //Arrange
            NitValidator validator = new NitValidator();

            //Act
            bool result = validator.IsValid(nit);

            //Assert
            Assert.AreEqual(expectedResult, result);

        }


    }
}
