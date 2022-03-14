using PipefittersAccounting.Core.Financing.FinancierAggregate;
using PipefittersAccounting.Infrastructure.Interfaces;
using PipefittersAccounting.Infrastructure.Interfaces.Financing;
using PipefittersAccounting.SharedKernel.CommonValueObjects;
using PipefittersAccounting.SharedKernel.Utilities;
using PipefittersAccounting.SharedModel.WriteModels.Financing;

namespace PipefittersAccounting.Infrastructure.Application.Commands.Financing
{
    public class FinancierCreateCommand
    {
        public static async Task<OperationResult<bool>> Execute
        (
            CreateFinancierInfo model,
            IFinancierAggregateRepository repo,
            IUnitOfWork unitOfWork
        )
        {
            try
            {
                if (await repo.Exists(model.Id))
                {
                    string errMsg = $"Create operation failed! A financier with this Id: {model.Id} already exists!";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                OperationResult<Guid> dupeNameResult = await repo.CheckForDuplicateFinancierName(model.FinancierName);
                if (dupeNameResult.Result != Guid.Empty)
                {
                    string errMsg = $"A financier with name: {model.FinancierName} is already in the database.";
                    return OperationResult<bool>.CreateFailure(errMsg);
                }

                Financier financier = new
                (
                    FinancierAgent.Create(EntityGuidID.Create(model.Id)),
                    OrganizationName.Create(model.FinancierName),
                    PhoneNumber.Create(model.Telephone),
                    Address.Create(model.AddressLine1, model.AddressLine2, model.City, model.StateCode, model.Zipcode),
                    PointOfContact.Create(model.ContactFirstName, model.ContactLastName, model.ContactMiddleInitial, model.ContactTelephone),
                    EntityGuidID.Create(model.UserId),
                    true
                );

                await repo.AddAsync(financier);
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