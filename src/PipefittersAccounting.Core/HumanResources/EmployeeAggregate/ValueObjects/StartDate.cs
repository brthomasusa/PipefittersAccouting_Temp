using PipefittersAccounting.SharedKernel;

namespace PipefittersAccounting.Core.HumanResources.EmployeeAggregate.ValueObjects
{
    public class StartDate : ValueObject
    {
        public DateTime Value { get; }

        protected StartDate() { }


        private StartDate(DateTime value) : this() => Value = value;

        public static implicit operator DateTime(StartDate self) => self.Value;

        public static StartDate Create(DateTime startDate)
        {
            CheckValidity(startDate);
            return new StartDate(startDate);
        }

        private static void CheckValidity(DateTime value)
        {
            if (value == default)
            {
                throw new ArgumentNullException("The employee start date is required.", nameof(value));
            }

            DateTime validStartDate = new DateTime(1998, 1, 1);

            if (validStartDate > value)
            {
                throw new InvalidOperationException("Employee start date must be greater than or equal to Jan 1, 1998.");
            }
        }
    }
}