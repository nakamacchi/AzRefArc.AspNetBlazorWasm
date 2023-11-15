using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzRefArc.AspNetWebApi.Shared
{
    public class PublisherOverview
    {
        public string PublisherId { get; set; } = string.Empty;
        public string PublisherName { get; set; } = string.Empty;
        public List<TitleOverview> TitleOverviews { get; set; } = new List<TitleOverview>();
    }
}
