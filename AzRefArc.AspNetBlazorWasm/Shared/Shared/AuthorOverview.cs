using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzRefArc.AspNetBlazorWasm.Shared.Shared
{
    public class AuthorOverview
    {
        public string AuthorId { get; set; } = String.Empty;
        public string AuthorName { get; set; } = String.Empty;
        public string Phone { get; set; } = String.Empty;
        public string State { get; set; } = String.Empty;
        public bool Contract { get; set; }
    }
}
