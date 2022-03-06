#pragma warning disable CS8600
#pragma warning disable CS8604
#pragma warning disable CS8625

using System;
using Xunit;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.Core.Financing.FinancierAggregate;

namespace PipefittersAccounting.UnitTests.ValueObjects.Financing
{
    public class FinancierAggregateTests
    {
        [Fact]
        public void ShouldReturn_Valid_Financier()
        {
            Financier financier = new Financier
            (
                FinancierAgent.Create(EntityGuidID.Create(Guid.NewGuid())),
                OrganizationName.Create("Testing"),
                PhoneNumber.Create("555-555-5555"),
                Address.Create("123 Main St", "Ste 1", "Somewhere", "OK", "87455"),
                PointOfContact.Create("Jon", "Doe", "O", "555-555-5555"),
                EntityGuidID.Create(Guid.NewGuid()),
                true
            );

            Assert.IsType<Financier>(financier);
        }

        [Fact]
        public void ShouldRaiseError_Null_Passed_In_Ctor()
        {
            // Set any of the input params below to null and run test (it will fail).
            // Should get specific error message describing missing parameter

            Action action = () => new Financier
            (
                null,
                OrganizationName.Create("Testing"),
                PhoneNumber.Create("555-555-5555"),
                Address.Create("123 Main St", "Ste 1", "Somewhere", "OK", "87455"),
                PointOfContact.Create("Jon", "Doe", "O", "555-555-5555"),
                EntityGuidID.Create(Guid.NewGuid()),
                true
            );

            var caughtException = Assert.Throws<ArgumentNullException>(action);
        }

        [Fact]
        public void ShouldUpdate_Financier_Name()
        {
            Financier financier = GetFinancier();
            Assert.Equal("Testing", financier.FinancierName);

            financier.UpdateFinancierName(OrganizationName.Create("HelloWorld"));

            Assert.Equal("HelloWorld", financier.FinancierName);
        }

        [Fact]
        public void ShouldUpdate_Financier_Telephone()
        {
            PhoneNumber phoneNumber = PhoneNumber.Create("817-874-9999");
            Financier financier = GetFinancier();

            Assert.NotEqual(phoneNumber, financier.FinancierTelephone);

            financier.UpdateFinancierTelephone(phoneNumber);

            Assert.Equal(phoneNumber, financier.FinancierTelephone);
        }

        [Fact]
        public void ShouldUpdate_Financier_Address()
        {
            Address address = Address.Create("999 4th Ave", "Ste 9", "Plano", "TX", "78547");
            Financier financier = GetFinancier();

            Assert.NotEqual(address, financier.FinancierAddress);

            financier.UpdateFinancierAddress(address);

            Assert.Equal(address, financier.FinancierAddress);
        }

        [Fact]
        public void ShouldUpdate_Financier_ContactPerson()
        {
            PointOfContact contact = PointOfContact.Create("Dave", "Lee", "K", "817-874-9999");


            Financier financier = GetFinancier();

            Assert.NotEqual(contact, financier.PointOfContact);

            financier.UpdatePointOfContact(contact);

            Assert.Equal(contact, financier.PointOfContact);
        }

        [Fact]
        public void ShouldUpdate_Financier_UserId()
        {
            Guid newID = Guid.NewGuid();
            Financier financier = GetFinancier();

            Assert.NotEqual(financier.UserId, newID);

            financier.UpdateUserId(EntityGuidID.Create(newID));

            Assert.Equal(financier.UserId, newID);
        }


        [Fact]
        public void ShouldUpdate_Financier_IsActive()
        {
            Financier financier = GetFinancier();

            Assert.True(financier.IsActive);

            financier.UpdateFinancierStatus(false);

            Assert.False(financier.IsActive);
        }

        private Financier GetFinancier() =>
            new Financier
            (
                FinancierAgent.Create(EntityGuidID.Create(Guid.NewGuid())),
                OrganizationName.Create("Testing"),
                PhoneNumber.Create("555-555-5555"),
                Address.Create("123 Main St", "Ste 1", "Somewhere", "OK", "87455"),
                PointOfContact.Create("Jon", "Doe", "O", "555-555-5555"),
                EntityGuidID.Create(Guid.NewGuid()),
                true
            );
    }
}