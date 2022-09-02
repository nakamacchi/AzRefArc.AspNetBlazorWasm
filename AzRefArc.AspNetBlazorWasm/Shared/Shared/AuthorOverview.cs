using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzRefArc.AspNetBlazorWasm.Shared.Shared
{
    public class AuthorOverview
    {
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string Phone { get; set; }
        public string State { get; set; }
        public bool Contract { get; set; }
    }
}
