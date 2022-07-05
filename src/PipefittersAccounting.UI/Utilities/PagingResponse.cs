using PipefittersAccounting.SharedModel.ReadModels;
namespace PipefittersAccounting.UI.Utilities
{
    public class PagingResponse<T> where T : class
    {
        public List<T>? Items { get; set; }
        public MetaData? MetaData { get; set; }
    }
}