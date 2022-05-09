namespace PipefittersAccounting.SharedKernel.Interfaces
{
    public interface IValidator<T>
    {
        IValidator<T> SetNext(IValidator<T> next);

        Task<ValidationResult> Validate(T request);
    }
}