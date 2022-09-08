using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipefittersAccounting.Infrastructure.Application.Queries.Financing.StockSubscriptionAggregate
{
    public static class StockSubscriptionQueries
    {
        public const string SelectAllStockSubscriptionListItems =
            @"SELECT 
                    * 
                FROM 
                (
                    SELECT 
                        sub.StockId, sub.StockNumber,fin.FinancierName AS InvestorName, 
                        fin.ContactFirstName + ' ' + ISNULL(fin.ContactMiddleInitial, '') + ' ' + fin.ContactLastName AS ContactName,
                        fin.ContactTelephone, sub.StockIssueDate, sub.SharesIssured, sub.PricePerShare, 
                        cash.CashAcctTransactionDate AS DateReceived, 
                        CASE
                            WHEN cash.CashAcctTransactionAmount IS NULL THEN 0        
                            ELSE cash.CashAcctTransactionAmount
                        END AS AmountReceived
                    FROM Finance.StockSubscriptions sub
                    LEFT JOIN Finance.Financiers fin  ON fin.FinancierID = sub.FinancierId
                    LEFT JOIN CashManagement.CashTransactions cash ON sub.StockId = cash.EventID
                ) AS InnerQuery
                ORDER BY InvestorName, StockIssueDate ";

        public const string SelectStockSubscriptionListItemsByInvestName =
            @"SELECT 
                * 
            FROM 
            (
                SELECT 
                    sub.StockId, sub.StockNumber,fin.FinancierName AS InvestorName, 
                    fin.ContactFirstName + ' ' + ISNULL(fin.ContactMiddleInitial, '') + ' ' + fin.ContactLastName AS ContactName,
                    fin.ContactTelephone, sub.StockIssueDate, sub.SharesIssured, sub.PricePerShare, 
                    cash.CashAcctTransactionDate AS DateReceived, 
                    CASE
                        WHEN cash.CashAcctTransactionAmount IS NULL THEN 0        
                        ELSE cash.CashAcctTransactionAmount
                    END AS AmountReceived
                FROM Finance.StockSubscriptions sub
                LEFT JOIN Finance.Financiers fin  ON fin.FinancierID = sub.FinancierId
                LEFT JOIN CashManagement.CashTransactions cash ON sub.StockId = cash.EventID
            ) AS InnerQuery
            WHERE InvestorName LIKE CONCAT('%',@InvestorName,'%')
            ORDER BY InvestorName, StockIssueDate ";

        public const string SelectStockSubscriptionListItemsByFundsRcvd =
            @"SELECT 
                * 
            FROM 
            (
                SELECT 
                    sub.StockId, sub.StockNumber,fin.FinancierName AS InvestorName, 
                    fin.ContactFirstName + ' ' + ISNULL(fin.ContactMiddleInitial, '') + ' ' + fin.ContactLastName AS ContactName,
                    fin.ContactTelephone, sub.StockIssueDate, sub.SharesIssured, sub.PricePerShare, 
                    cash.CashAcctTransactionDate AS DateReceived, 
                    CASE
                        WHEN cash.CashAcctTransactionAmount IS NULL THEN 0        
                        ELSE cash.CashAcctTransactionAmount
                    END AS AmountReceived
                FROM Finance.StockSubscriptions sub
                LEFT JOIN Finance.Financiers fin  ON fin.FinancierID = sub.FinancierId
                LEFT JOIN CashManagement.CashTransactions cash ON sub.StockId = cash.EventID
            ) AS InnerQuery
            WHERE AmountReceived > 0
            ORDER BY InvestorName, StockIssueDate ";

        public const string SelectStockSubscriptionListItemsByFundsNotRcvd =
            @"SELECT 
                * 
            FROM 
            (
                SELECT 
                    sub.StockId, sub.StockNumber,fin.FinancierName AS InvestorName, 
                    fin.ContactFirstName + ' ' + ISNULL(fin.ContactMiddleInitial, '') + ' ' + fin.ContactLastName AS ContactName,
                    fin.ContactTelephone, sub.StockIssueDate, sub.SharesIssured, sub.PricePerShare, 
                    cash.CashAcctTransactionDate AS DateReceived, 
                    CASE
                        WHEN cash.CashAcctTransactionAmount IS NULL THEN 0        
                        ELSE cash.CashAcctTransactionAmount
                    END AS AmountReceived
                FROM Finance.StockSubscriptions sub
                LEFT JOIN Finance.Financiers fin  ON fin.FinancierID = sub.FinancierId
                LEFT JOIN CashManagement.CashTransactions cash ON sub.StockId = cash.EventID
            ) AS InnerQuery
            WHERE AmountReceived = 0
            ORDER BY InvestorName, StockIssueDate ";
    }
}