namespace PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.GetStockSubscriptions.Actions
{
    public readonly record struct LoadStockSubscriptionsUnfilteredAction(int PageNumber, int PageSize);
}