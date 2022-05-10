#pragma warning disable CS8602

using PipefittersAccounting.SharedKernel.Interfaces;

namespace PipefittersAccounting.SharedKernel
{
    public abstract class Validator<T> : IValidator<T>
    {
        protected IValidator<T>? Next { get; private set; }

        public void SetNext(IValidator<T> next)
        {
            Next = next;
        }

        public virtual async Task<ValidationResult> Validate(T request)
        {
            ValidationResult validationResult = new();

            if (Next is not null)
            {
                await Next.Validate(request);
            }

            return validationResult;
        }
    }
}