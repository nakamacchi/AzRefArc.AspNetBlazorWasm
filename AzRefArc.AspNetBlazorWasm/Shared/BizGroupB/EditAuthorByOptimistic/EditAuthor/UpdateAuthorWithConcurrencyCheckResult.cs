using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzRefArc.AspNetBlazorWasm.Shared.BizGroupB.EditAuthorByOptimistic.EditAuthor
{
    public enum UpdateAuthorWithConcurrencyCheckResult
    {
        Success = 0,
        UpdateFailureByOptimisticConcurrencyConflict = 1
    }
}
