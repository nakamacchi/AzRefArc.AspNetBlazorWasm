using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzRefArc.AspNetBlazorWasm.Shared.Shared
{
    public class TitleOverview
    {
        public string TitleId { get; set; }
        public string TitleName { get; set; }
        public decimal? Price { get; set; }
        public DateTime PublishedDate { get; set; }
        public string ImageUrl { get; set; }
        public string ImageThumbnailUrl { get; set; }
    }
}
