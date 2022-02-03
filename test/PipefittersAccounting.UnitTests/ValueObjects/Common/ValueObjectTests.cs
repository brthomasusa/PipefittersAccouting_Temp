#pragma warning disable CS8625

using System;
using Xunit;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.UnitTests.ValueObjects.Common
{
    public class ValueObjectTests
    {
        [Fact]
        public void ShouldReturn_Valid_PersonName()
        {
            var result = PersonName.Create("Doe", "Jon", "D");

            Assert.IsType<PersonName>(result);
            Assert.Equal("Doe", result.LastName);
        }

        [Fact]
        public void ShouldRaiseError_PersonName_LastName_Null()
        {
            Action action = () => PersonName.Create(null, "Jon", "D");

            var caughtException = Assert.Throws<ArgumentNullException>(action);

            Assert.Equal("A last name is required.", caughtException.ParamName);
        }

        [Fact]
        public void ShouldRaiseError_PersonName_LastName_ZeroLength()
        {
            Action action = () => PersonName.Create("", "Jon", "D");

            var caughtException = Assert.Throws<ArgumentNullException>(action);

            Assert.Equal("A last name is required.", caughtException.ParamName);
        }

        [Fact]
        public void ShouldRaiseError_PersonName_LastName_TooLong()
        {
            Action action = () => PersonName.Create("DoeRaeMeFoSoLaTeDoDoeRaeMeFoSoLaTeDo", "Jon", "D");

            var caughtException = Assert.Throws<ArgumentOutOfRangeException>(action);

            Assert.Equal("Maximum length of the last name is 25 characters.", caughtException.ParamName);
        }

        [Fact]
        public void ShouldRaiseError_PersonName_FirstName_Null()
        {
            Action action = () => PersonName.Create("Doe", null, "D");

            var caughtException = Assert.Throws<ArgumentNullException>(action);

            Assert.Equal("A first name is required.", caughtException.ParamName);
        }

        [Fact]
        public void ShouldRaiseError_PersonName_FirstName_ZeroLength()
        {
            Action action = () => PersonName.Create("Doe", "", "D");

            var caughtException = Assert.Throws<ArgumentNullException>(action);

            Assert.Equal("A first name is required.", caughtException.ParamName);
        }

        [Fact]
        public void ShouldRaiseError_PersonName_FirstName_TooLong()
        {
            Action action = () => PersonName.Create("Doe", "JonDoeRaeMeFoSoLaTeDoDoeRaeMeFoSoLaTeDo", "D");

            var caughtException = Assert.Throws<ArgumentOutOfRangeException>(action);

            Assert.Equal("Maximum length of the first name is 25 characters.", caughtException.ParamName);
        }

        [Fact]
        public void ShouldRaiseError_PersonName_MiddleInitial_TooLong()
        {
            Action action = () => PersonName.Create("Doe", "Jon", "Dee");

            var caughtException = Assert.Throws<ArgumentOutOfRangeException>(action);

            Assert.Equal("Maximum length of middle initial is 1 character.", caughtException.ParamName);
        }

        [Fact]
        public void ShouldReturn_Valid_OrganizationName()
        {
            var result = OrganizationName.Create("HelloWorld");

            Assert.IsType<OrganizationName>(result);
            Assert.Equal("HelloWorld", result);
        }

        [Fact]
        public void ShouldRaiseError_OrganizationName_TooLong()
        {
            Action action = () => OrganizationName.Create("DoeRaeMeFoSoLaTeDoDoeRaeMeFoSoLaTeDoDoeRaeMeFoSoLaTeDoDoeRaeMeFoSoLaTeDoDoeRaeMeFoSoLaTeDoDoeRaeMeFoSoLaTeDo");

            var caughtException = Assert.Throws<ArgumentOutOfRangeException>(action);

            Assert.Equal("Maximum length of the organization name is 50 characters.", caughtException.ParamName);
        }

        [Fact]
        public void ShouldRaiseError_OrganizationName_Null()
        {
            Action action = () => OrganizationName.Create(null);

            var caughtException = Assert.Throws<ArgumentNullException>(action);

            Assert.Equal("An organization name is required.", caughtException.ParamName);
        }

        [Fact]
        public void ShouldRaiseError_OrganizationName_ZeroLength()
        {
            Action action = () => OrganizationName.Create("");

            var caughtException = Assert.Throws<ArgumentNullException>(action);

            Assert.Equal("An organization name is required.", caughtException.ParamName);
        }
    }
}