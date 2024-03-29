using PipefittersAccounting.SharedModel;
using PipefittersAccounting.SharedModel.Interfaces;
using PipefittersAccounting.SharedModel.Readmodels.CashManagement;
using PipefittersAccounting.SharedModel.WriteModels.CashManagement;

namespace PipefittersAccounting.Infrastructure.Application.Services.CashManagement
{
    public class CreatePayrollCashTransactions : IWriteModel
    {
        private readonly List<TimeCardPaymentInfo> _timeCardInfoList;
        private List<CashTransactionWriteModel> _cashTransactions = new();

        public CreatePayrollCashTransactions
        (
            Guid cashAccountId,
            Guid userId,
            List<TimeCardPaymentInfo> timeCardPaymentList
        )
        {
            CashAccountId = cashAccountId;
            UserId = userId;
            _timeCardInfoList = timeCardPaymentList;

            CreateCashDisbursements();
        }

        public Guid CashAccountId { get; init; }
        public Guid UserId { get; init; }
        public virtual List<CashTransactionWriteModel> CashTransactions => _cashTransactions.ToList();

        private void CreateCashDisbursements()
        {
            _timeCardInfoList.ForEach(info =>
                _cashTransactions.Add(info.Map(CashAccountId, UserId))
            );
        }
    }
}