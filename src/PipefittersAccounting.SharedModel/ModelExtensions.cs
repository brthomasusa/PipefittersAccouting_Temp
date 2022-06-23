using PipefittersAccounting.SharedModel.WriteModels.HumanResources;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;

namespace PipefittersAccounting.SharedModel
{
    public static class ModelExtensions
    {
        public static PayrollRegisterWriteModel Map(this PayrollRegister input)
        {
            return new PayrollRegisterWriteModel()
            {
                TimeCardId = input.TimeCardId,
                EmployeeId = input.EmployeeId,
                EmployeeName = input.EmployeeName,
                RegularPay = input.RegularPay,
                OvertimePay = input.OvertimePay,
                GrossPay = input.GrossPay,
                FICA = input.FICA,
                Medicare = input.Medicare,
                FederalWithholding = input.FederalWithholding,
                NetPay = input.NetPay
            };
        }
    }
}