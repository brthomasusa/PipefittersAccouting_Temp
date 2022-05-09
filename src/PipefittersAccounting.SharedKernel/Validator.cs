#pragma warning disable CS8602

using PipefittersAccounting.SharedKernel.Interfaces;

namespace PipefittersAccounting.SharedKernel
{
    public abstract class Validator<T> : IValidator<T>
    {
        private IValidator<T>? Next { get; set; }

        public IValidator<T> SetNext(IValidator<T> next)
        {
            Next = next;
            return Next;
        }

        public virtual async Task<ValidationResult> Validate(T request)
        {
            ValidationResult validationResult = new();

            await Next?.Validate(request);

            return validationResult;
        }
    }
}