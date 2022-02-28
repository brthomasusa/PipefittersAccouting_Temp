using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PipefittersAccounting.SharedModel.ReadModels
{
    public record PagedResult<T>
    {
        public IEnumerable<T>? readModels { get; init; }
        public MetaData? metaData { get; init; }
    }
}