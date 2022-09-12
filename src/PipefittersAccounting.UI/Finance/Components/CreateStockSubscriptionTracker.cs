using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Fluxor;

using PipefittersAccounting.SharedModel.WriteModels.Financing;
using PipefittersAccounting.UI.Store.State.Finance.StockSubscription;
using PipefittersAccounting.UI.Services.Fluxor.Financing;
using PipefittersAccounting.UI.Utilities;

namespace PipefittersAccounting.UI.Finance.Components
{
    public class CreateStockSubscriptionTracker : ComponentBase
    {
        [Parameter] public EditContext? CreateEditContext { get; set; }
        [Inject] private IState<StockSubscriptionsState>? _stockSubscriptionState { get; set; }
        [Inject] private StateFacade? _facade { get; set; }

        protected override void OnInitialized()
        {
            if (CreateEditContext is null)
            {
                throw new InvalidOperationException
                (
                    $"{nameof(CreateStockSubscriptionTracker)} requires a parameter of type {nameof(EditContext)}"
                );
            }

            CreateEditContext.OnFieldChanged += EditContextOnFieldChangedHandler!;
            Console.WriteLine("CreateStockSubscriptionTracker.OnInitialized");
        }

        private void EditContextOnFieldChangedHandler(object sender, FieldChangedEventArgs e)
        {
            StockSubscriptionWriteModel subscription = (StockSubscriptionWriteModel)e.FieldIdentifier.Model;
            Console.WriteLine("CreateStockSubscriptionTracker.EditContextOnFieldChangedHandler");
            Console.WriteLine(subscription.ToJson());

            if (subscription.StockId == Guid.Empty)
            {
                _facade!.UpdateStockSubscriptionCreateModel(subscription);
                Console.WriteLine("_facade!.UpdateStockSubscriptionCreateModel called ...");
            }
        }
    }
}