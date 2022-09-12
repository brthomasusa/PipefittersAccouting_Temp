using Fluxor;
using Blazorise;
using PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.CreateStockSubscription.Actions;
using PipefittersAccounting.SharedModel.Readmodels.Financing;
using PipefittersAccounting.UI.Interfaces;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.CreateStockSubscription.Effects
{
    public class CreateStockSubscriptionEffect : Effect<CreateStockSubscriptionAction>
    {
        private IStockSubscriptionRepository? _stockSubscriptionService;
        private IMessageService? _messageService;

        public CreateStockSubscriptionEffect() { }


        public override async Task HandleAsync(CreateStockSubscriptionAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new CreateStockSubscriptionSuccessAction(action.StockSubscriptionWriteModel));
            }
            catch (Exception ex)
            {
                dispatcher.Dispatch(new CreateStockSubscriptionFailureAction(ex.Message));
            }

            await Task.CompletedTask;
        }
    }
}