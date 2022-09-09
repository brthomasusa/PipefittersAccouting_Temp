using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.GetStockSubscriptions.Actions
{
    public readonly record struct LoadStockSubscriptionsWithSearchTermAction(string SearchTerm, int PageNumber, int PageSize);
}