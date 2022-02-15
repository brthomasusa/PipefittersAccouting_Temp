namespace PipefittersAccounting.SharedKernel.Utilities
{
    public class TypedOperationResult<T> : OperationResult
    {
        protected TypedOperationResult(T returnValue)
            : base()
        {
            ReturnValue = returnValue;
        }

        public T? ReturnValue { get; protected set; }

        public static OperationResult Value(T value)
        {
            return new TypedOperationResult<T>(value);
        }
    }
}