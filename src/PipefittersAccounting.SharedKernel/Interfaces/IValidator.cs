namespace PipefittersAccounting.SharedKernel.Interfaces
{
    public interface IValidator<T>
    {
        void SetNext(IValidator<T> next);

        Task<ValidationResult> Validate(T request);
    }
}