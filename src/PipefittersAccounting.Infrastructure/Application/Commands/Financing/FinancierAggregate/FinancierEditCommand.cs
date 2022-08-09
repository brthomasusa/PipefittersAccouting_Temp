using PipefittersAccounting.Core.Financing.FinancierAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Core.Interfaces.Financing;
using PipefittersAccounting.Core.Shared;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing.FinancierAggregate
{
    public class FinancierEditCommand
    {
        public static async Task<OperationResult<bool>> Execute
        (
            FinancierWriteModel model,
            IFinancierAggregateRepository repo,
            IUnitOfWork unitOfWork
        )
        {
            try
            {
                OperationResult<bool> result = await repo.Exists(model.Id);

                if (!result.Success)
                {
                    string errMsg = $"Delete failed, a financier with id: {model.Id} could not be found!";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                OperationResult<Guid> dupeNameResult = await repo.CheckForDuplicateFinancierName(model.FinancierName);
                if (dupeNameResult.Result != Guid.Empty && dupeNameResult.Result != model.Id)
                {
                    string errMsg = $"A financier with name: {model.FinancierName} is already in the database.";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                OperationResult<Financier> getResult = await repo.GetByIdAsync(model.Id);

                if (getResult.Success)
                {
                    Financier financier = getResult.Result;
                    financier.UpdateFinancierName(OrganizationName.Create(model.FinancierName));
                    financier.UpdateFinancierTelephone(PhoneNumber.Create(model.Telephone));
                    financier.UpdateEmailAddress(EmailAddress.Create(model.EmailAddress));
                    financier.UpdateFinancierAddress(Address.Create(model.AddressLine1, model.AddressLine2!, model.City, model.StateCode, model.Zipcode));
                    financier.UpdatePointOfContact(PointOfContact.Create(model.ContactFirstName, model.ContactLastName, model.ContactMiddleInitial!, model.ContactTelephone));
                    financier.UpdateUserId(EntityGuidID.Create(model.UserId));
                    financier.UpdateFinancierStatus(model.IsActive);

                    OperationResult<bool> updateResult = repo.Update(financier);

                    if (updateResult.Success)
                    {
                        await unitOfWork.Commit();

                        return OperationResult<bool>.CreateSuccessResult(true);
                    }
                    else
                    {
                        return OperationResult<bool>.CreateFailure(updateResult.NonSuccessMessage);
                    }
                }
                else
                {
                    return OperationResult<bool>.CreateFailure(getResult.NonSuccessMessage);
                }
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }
    }
}