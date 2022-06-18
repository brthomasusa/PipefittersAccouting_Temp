#pragma warning disable CS8618

using PipefittersAccounting.Core.HumanResources.EmployeeAggregate.ValueObjects;
using PipefittersAccounting.SharedKernel;
using PipefittersAccounting.SharedKernel.CommonValueObjects;

namespace PipefittersAccounting.Core.HumanResources.EmployeeAggregate
{
    public class TimeCard : Entity<Guid>
    {
        protected TimeCard() { }

        public TimeCard
        (
            EntityGuidID timeCardId,
            EntityGuidID employeeId,
            EntityGuidID supervisorId,
            EntityDate payPeriodEnded,
            DecimalNotNegative regularHours,
            DecimalNotNegative overtimeHours,
            EntityGuidID userId
        ) : this()
        {
            Id = timeCardId ?? throw new ArgumentNullException("The time card id is required.");
            EmployeeId = employeeId ?? throw new ArgumentNullException("An employee id is required.");
            SupervisorId = supervisorId ?? throw new ArgumentNullException("A supervisor id is required.");
            PayPeriodEnded = payPeriodEnded ?? throw new ArgumentNullException("The pay period ending date is required.");
            RegularHours = regularHours ?? throw new ArgumentNullException("The regular hours worked are required (0 if none).");
            OvertimeHours = overtimeHours ?? throw new ArgumentNullException("The overtime hours worked are required (0 if none).");
            UserId = userId ?? throw new ArgumentNullException("The user id is required.");

            CheckValidity();
        }

        public Guid EmployeeId { get; init; }

        public Guid SupervisorId { get; private set; }

        public void UpdateSupervisorId(EntityGuidID value)
        {
            SupervisorId = value ?? throw new ArgumentNullException("The supervisor id can not be null.");
            UpdateLastModifiedDate();
        }

        public EntityDate PayPeriodEnded { get; init; }

        public DecimalNotNegative RegularHours { get; private set; }
        public void UpdateRegularHours(DecimalNotNegative value)
        {
            RegularHours = value ?? throw new ArgumentNullException("The regular hours worked are required (0 if none).");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public DecimalNotNegative OvertimeHours { get; private set; }
        public void UpdateOvertimeHours(DecimalNotNegative value)
        {
            OvertimeHours = value ?? throw new ArgumentNullException("The regular hours worked are required (0 if none).");
            UpdateLastModifiedDate();
            CheckValidity();
        }

        public EntityGuidID UserId { get; private set; }
        public void UpdateUserId(EntityGuidID value)
        {
            UserId = value ?? throw new ArgumentNullException("The User id can not be null.");
            UpdateLastModifiedDate();
        }

        protected override void CheckValidity()
        {
            if (RegularHours > 185)
            {
                throw new ArgumentException("Regular hours worked must be between 0 and 185.");
            }

            if (OvertimeHours > 200)
            {
                throw new ArgumentException("Overtime hours worked must be between 0 and 200.");
            }
        }
    }
}