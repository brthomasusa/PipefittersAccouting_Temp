#pragma warning disable CS8618

using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.Core.HumanResources.EmployeeAggregate
{
    public class Employee : AggregateRoot<Guid>
    {
        protected Employee() { }

        public Employee
        (
            ExternalAgent agent,
            EntityGuidID supervisorId,
            PersonName employeeName,
            SocialSecurityNumber ssn,
            PhoneNumber telephone,
            Address address,
            MaritalStatus maritalStatus,
            TaxExemption exemption,
            PayRate payRate,
            StartDate startDate
        )
            : this()
        {
            if (agent.AgentType != AgentType.Employee)
            {
                throw new InvalidOperationException("External agent type must be 'AgentType.Employee'.");
            }

            Id = agent.Id;
            ExternalAgent = agent;
            SupervisorId = supervisorId;
            EmployeeName = employeeName;
            SSN = ssn;
            EmployeeTelephone = telephone;
            EmployeeAddress = address;
            MaritalStatus = maritalStatus;
            TaxExemptions = exemption;
            EmployeePayRate = payRate;
            EmploymentDate = startDate;
        }

        public virtual ExternalAgent ExternalAgent { get; init; }

        public Guid SupervisorId { get; private set; }

        public PersonName EmployeeName { get; private set; }

        public SocialSecurityNumber SSN { get; private set; }

        public PhoneNumber EmployeeTelephone { get; private set; }

        public Address EmployeeAddress { get; private set; }

        public MaritalStatus MaritalStatus { get; private set; }

        public TaxExemption TaxExemptions { get; private set; }

        public PayRate EmployeePayRate { get; private set; }

        public StartDate EmploymentDate { get; private set; }

        public bool IsActive { get; private set; }

        public bool IsSupervisor { get; private set; }
    }
}