#pragma warning disable CS8618

using PipefittersAccounting.Core.HumanResources.EmployeeAggregate.ValueObjects;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Interfaces;

namespace PipefittersAccounting.Core.HumanResources.EmployeeAggregate
{
    public class Employee : AggregateRoot<Guid>
    {
        protected Employee() { }

        public Employee
        (
            EntityGuidID employeeId,
            EmployeeTypeEnum employeeType,
            EntityGuidID supervisorId,
            PersonName employeeName,
            SocialSecurityNumber ssn,
            PhoneNumber telephone,
            Address address,
            MaritalStatus maritalStatus,
            TaxExemption exemption,
            PayRate payRate,
            StartDate startDate,
            bool isActive,
            bool isSupervisor
        )
            : this()
        {
            Id = employeeId ?? throw new ArgumentNullException("The employee id is required.");
            EmployeeType = employeeType;
            SupervisorId = supervisorId ?? throw new ArgumentNullException("A supervisor id is required.");
            EmployeeName = employeeName ?? throw new ArgumentNullException("An employee name is required.");
            SSN = ssn ?? throw new ArgumentNullException("A social security number is required.");
            EmployeeTelephone = telephone ?? throw new ArgumentNullException("A phone number is required.");
            EmployeeAddress = address ?? throw new ArgumentNullException("An address is required.");
            MaritalStatus = maritalStatus ?? throw new ArgumentNullException("A marital status is required.");
            TaxExemptions = exemption ?? throw new ArgumentNullException("A tax exemption is required.");
            EmployeePayRate = payRate ?? throw new ArgumentNullException("A pay rate is required.");
            EmploymentDate = startDate ?? throw new ArgumentNullException("A hire date is required.");
            IsActive = isActive;
            IsSupervisor = isSupervisor;
        }

        public EmployeeTypeEnum EmployeeType { get; private set; }
        public void UpdateEmployeeType(EmployeeTypeEnum employeeType)
        {
            //TODO Don't allow editing if the account has transactions not compatible with desired new type
            EmployeeType = employeeType;
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public Guid SupervisorId { get; private set; }

        public void UpdateSupervisorId(EntityGuidID value)
        {
            SupervisorId = value ?? throw new ArgumentNullException("The supervisor id can not be null.");
            UpdateLastModifiedDate();
        }

        public PersonName EmployeeName { get; private set; }

        public void UpdateEmployeeName(PersonName value)
        {
            EmployeeName = value ?? throw new ArgumentNullException("An employee name is required.");
            UpdateLastModifiedDate();
        }

        public SocialSecurityNumber SSN { get; private set; }

        public void UpdateSSN(SocialSecurityNumber value)
        {
            SSN = value ?? throw new ArgumentNullException("A social security number is required.");
            UpdateLastModifiedDate();
        }

        public PhoneNumber EmployeeTelephone { get; private set; }

        public void UpdateEmployeePhoneNumber(PhoneNumber value)
        {
            EmployeeTelephone = value ?? throw new ArgumentNullException("A phone number is required.");
            UpdateLastModifiedDate();
        }

        public Address EmployeeAddress { get; private set; }

        public void UpdateEmployeeAddress(Address value)
        {
            EmployeeAddress = value ?? throw new ArgumentNullException("An address is required.");
            UpdateLastModifiedDate();
        }

        public MaritalStatus MaritalStatus { get; private set; }

        public void UpdateMaritalStatus(MaritalStatus value)
        {
            MaritalStatus = value ?? throw new ArgumentNullException("A marital status is required.");
            UpdateLastModifiedDate();
        }

        public TaxExemption TaxExemptions { get; private set; }

        public void UpdateTaxExemptions(TaxExemption value)
        {
            TaxExemptions = value ?? throw new ArgumentNullException("A tax exemption is required.");
            UpdateLastModifiedDate();
        }

        public PayRate EmployeePayRate { get; private set; }

        public void UpdateEmployeePayRate(PayRate value)
        {
            EmployeePayRate = value ?? throw new ArgumentNullException("A pay rate is required.");
            UpdateLastModifiedDate();
        }

        public StartDate EmploymentDate { get; private set; }

        public void UpdateEmploymentDate(StartDate value)
        {
            EmploymentDate = value ?? throw new ArgumentNullException("A hire date is required.");
            UpdateLastModifiedDate();
        }

        public bool IsActive { get; private set; }

        public void UpdateEmployeeStatus(bool value)
        {
            IsActive = value;
            UpdateLastModifiedDate();
        }

        public bool IsSupervisor { get; private set; }

        public void UpdateIsSupervisor(bool value)
        {
            IsSupervisor = value;
            UpdateLastModifiedDate();
        }
    }
}