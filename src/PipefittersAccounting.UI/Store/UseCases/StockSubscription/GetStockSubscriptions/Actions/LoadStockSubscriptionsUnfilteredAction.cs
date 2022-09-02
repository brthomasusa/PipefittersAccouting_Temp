namespace PipefittersAccounting.UI.Store.UseCases.StockSubscription.GetStockSubscriptions.Actions
{
    public readonly record struct LoadStockSubscriptionsUnfilteredAction(int PageNumber, int PageSize);
}