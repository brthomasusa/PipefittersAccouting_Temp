using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipefittersAccounting.UI.Store.UseCases.StockSubscription.GetStockSubscriptions.Actions
{
    public readonly record struct LoadStockSubscriptionsFilteredAction(string InvestorName, int PageNumber, int PageSize);
}