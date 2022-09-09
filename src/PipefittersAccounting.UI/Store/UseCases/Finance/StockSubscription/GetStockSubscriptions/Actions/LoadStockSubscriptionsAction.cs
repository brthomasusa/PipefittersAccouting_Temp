using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipefittersAccounting.UI.Store.UseCases.Finance.StockSubscription.GetStockSubscriptions.Actions
{
    public readonly record struct LoadStockSubscriptionsAction(string filterName, int PageNumber, int PageSize);
}