using PipefittersAccounting.Core.Financing.FinancierAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing
{
    public class FinancierEditCommand
    {
        public static async Task<OperationResult<bool>> Execute
        (
            EditFinancierInfo model,
            IFinancierAggregateRepository repo,
            IUnitOfWork unitOfWork
        )
        {
            try
            {
                if (await repo.Exists(model.Id) == false)
                {
                    string errMsg = $"Update failed, a financier with id: {model.Id} could not be found!";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                OperationResult<Guid> dupeNameResult = await repo.CheckForDuplicateFinancierName(model.FinancierName);
                if (dupeNameResult.Result != Guid.Empty && dupeNameResult.Result != model.Id)
                {
                    string errMsg = $"A financier with name: {model.FinancierName} is already in the database.";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                Financier financier = await repo.GetByIdAsync(model.Id);
                financier.UpdateFinancierName(OrganizationName.Create(model.FinancierName));
                financier.UpdateFinancierTelephone(PhoneNumber.Create(model.Telephone));
                financier.UpdateFinancierAddress(Address.Create(model.AddressLine1, model.AddressLine2, model.City, model.StateCode, model.Zipcode));
                financier.UpdatePointOfContact(PointOfContact.Create(model.ContactFirstName, model.ContactLastName, model.ContactMiddleInitial, model.ContactTelephone));
                financier.UpdateUserId(EntityGuidID.Create(model.UserId));
                financier.UpdateFinancierStatus(model.IsActive);

                repo.Update(financier);
                await unitOfWork.Commit();

                return OperationResult<bool>.CreateSuccessResult(true);
            }
            catch (Exception ex)
            {
                return OperationResult<bool>.CreateFailure(ex.Message);
            }
        }
    }
}