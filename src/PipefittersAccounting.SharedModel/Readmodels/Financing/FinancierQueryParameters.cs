
namespace PipefittersAccounting.SharedModel.Readmodels.Financing
{
    public class GetFinancier
    {
        public Guid FinancierId { get; set; }
    }

    public class GetFinanciers
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
    }

    public class GetFinanciersLookup { }

    public class DoFinancierDependencyCheck
    {
        public Guid FinancierId { get; set; }
    }
}