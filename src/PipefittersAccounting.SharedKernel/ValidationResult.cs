namespace PipefittersAccounting.SharedKernel
{
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Messages { get; set; } = new();
    }
}