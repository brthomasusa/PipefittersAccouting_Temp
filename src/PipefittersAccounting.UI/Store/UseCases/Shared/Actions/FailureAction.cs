namespace PipefittersAccounting.UI.Store.UseCases.Shared.Actions
{
    public abstract class FailureAction
    {
        protected FailureAction(string errorMessage) =>
            ErrorMessage = errorMessage;

        public string ErrorMessage { get; }
    }
}