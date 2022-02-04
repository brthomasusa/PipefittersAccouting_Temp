#pragma warning disable CS8600
#pragma warning disable CS8604
#pragma warning disable CS8625

using System;
using Xunit;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.UnitTests.ValueObjects.Common
{
    public class ValueObjectTests
    {
        [Fact]
        public void ShouldReturn_Valid_NonDefault_Guid()
        {
            Guid entityID = Guid.NewGuid();
            var result = EntityGuidID.Create(entityID);

            Assert.IsType<EntityGuidID>(result);
            Assert.Equal(entityID, result);
        }

        [Fact]
        public void ShouldRaiseError_Invalid_EntityID_DefaultGuid()
        {
            Guid entityID = new Guid();
            Action action = () => EntityGuidID.Create(entityID);

            var caughtException = Assert.Throws<ArgumentNullException>(action);

            Assert.Equal("The entity Id is required; it can not be a default Guid.", caughtException.ParamName);
        }

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

        [Fact]
        public void ShouldReturn_Valid_TelephoneNumber()
        {
            string phone = "972-555-5555";

            var result = PhoneNumber.Create(phone);

            Assert.IsType<PhoneNumber>(result);
            Assert.Equal(phone, result);
        }

        [Fact]
        public void ShouldRaiseError_Invalid_TelephoneNumber()
        {
            string phone = "0972-555-5555";

            Action action = () => PhoneNumber.Create(phone);

            var caughtException = Assert.Throws<ArgumentException>(action);

            Assert.Contains("Invalid PhoneNumber number!", caughtException.Message);
        }

        [Fact]
        public void ShouldRaiseError_Invalid_TelephoneNumberIsNull()
        {
            Action action = () => PhoneNumber.Create(null);

            var caughtException = Assert.Throws<ArgumentNullException>(action);

            Assert.Contains("The PhoneNumber number is required.", caughtException.Message);
        }

        [Fact]
        public void ShouldReturn_Valid_Address_WithoutSecondAddressLine()
        {
            string line1 = "123 Main Street";
            string line2 = null;
            string city = "Anywhereville";
            string stateCode = "TX";
            string zipcode = "75216";

            var result = Address.Create(line1, line2, city, stateCode, zipcode);

            Assert.IsType<Address>(result);
            Assert.Equal(line1, result.AddressLine1);
            Assert.Null(result.AddressLine2);
            Assert.Equal(city, result.City);
            Assert.Equal(stateCode, result.StateCode);
            Assert.Equal(zipcode, result.Zipcode);
        }

        [Fact]
        public void ShouldRaiseError_Invalid_StateCodeDoesNotExist()
        {
            string line1 = "123 Main Street";
            string line2 = null;
            string city = "Anywhereville";
            string stateCode = "IX";
            string zipcode = "75216";

            Action action = () => Address.Create(line1, line2, city, stateCode, zipcode);

            var caughtException = Assert.Throws<ArgumentException>(action);

            Assert.Contains("Invalid state code!", caughtException.Message);
        }

        [Fact]
        public void ShouldRaiseError_Invalid_StateCodeIsNull()
        {
            string line1 = "123 Main Street";
            string line2 = null;
            string city = "Anywhereville";
            string stateCode = null;
            string zipcode = "75216";

            Action action = () => Address.Create(line1, line2, city, stateCode, zipcode);

            var caughtException = Assert.Throws<ArgumentNullException>(action);

            Assert.Contains("A 2-digit state code is required.", caughtException.Message);
        }

        [Fact]
        public void ShouldRaiseError_Invalid_ZipcodeIsNull()
        {
            string line1 = "123 Main Street";
            string line2 = null;
            string city = "Anywhereville";
            string stateCode = "TX";
            string zipcode = null;

            Action action = () => Address.Create(line1, line2, city, stateCode, zipcode);

            var caughtException = Assert.Throws<ArgumentNullException>(action);

            Assert.Contains("A zip code is required.", caughtException.Message);
        }

        [Fact]
        public void ShouldRaiseError_Invalid_Zipcode()
        {
            string line1 = "123 Main Street";
            string line2 = null;
            string city = "Anywhereville";
            string stateCode = "TX";
            string zipcode = "752136";

            Action action = () => Address.Create(line1, line2, city, stateCode, zipcode);

            var caughtException = Assert.Throws<ArgumentException>(action);

            Assert.Contains("Invalid zip code!", caughtException.Message);
        }

    }
}