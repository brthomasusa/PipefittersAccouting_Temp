using PipefittersAccounting.SharedModel;
using PipefittersAccounting.SharedModel.ReadModels;
using PipefittersAccounting.SharedModel.Readmodels.HumanResources;
using PipefittersAccounting.SharedModel.WriteModels.HumanResources;

namespace PipefittersAccounting.UI.HumanResources.Components
{
    public class EmployeeDataEntryState
    {
        public EmployeeWriteModel EmployeeWriteModel { get; set; } = new();
        public List<EmployeeManager> Managers { get; set; } = new();
        public List<EmployeeTypes> EmployeeTypes { get; set; } = new();
    }
}