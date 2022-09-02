using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzRefArc.AspNetBlazorWasm.Shared.Shared
{
    public class PublisherOverview
    {
        public string PublisherId { get; set; }
        public string PublisherName { get; set; }
        public List<TitleOverview> TitleOverviews { get; set; }
    }
}
