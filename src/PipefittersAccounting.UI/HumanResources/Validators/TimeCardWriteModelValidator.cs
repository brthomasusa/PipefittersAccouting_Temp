using FluentValidation;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.UI.HumanResources.Validators
{
    public class TimeCardWriteModelValidator : AbstractValidator<TimeCardWriteModel>
    {
        public TimeCardWriteModelValidator()
        {
            RuleFor(employee => employee.TimeCardId)
                                        .NotEmpty()
                                        .WithMessage("Missing timecard Id; this is required.");

            RuleFor(employee => employee.EmployeeId)
                                        .NotEmpty()
                                        .WithMessage("Missing employee Id; this is required.");

            RuleFor(employee => employee.SupervisorId)
                                        .NotEmpty()
                                        .WithMessage("Missing supervisor Id; this is required.");

            RuleFor(employee => employee.PayPeriodEnded)
                                        .NotEmpty()
                                        .WithMessage("Pay period ended date is required.");

            RuleFor(employee => employee.RegularHours)
                                        .InclusiveBetween(0, 185)
                                        .WithMessage("Regular hours must be between 0 and 185.");

            RuleFor(employee => employee.OvertimeHours)
                                        .InclusiveBetween(0, 200)
                                        .WithMessage("Overtime hours must be between 0 and 200.");

            RuleFor(employee => employee.UserId)
                                        .NotEmpty()
                                        .WithMessage("Your user Id is required.");
        }
    }
}