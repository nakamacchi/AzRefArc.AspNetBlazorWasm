using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzRefArc.AspNetWebApi.Shared
{
    public class TitleOverview
    {
        public string TitleId { get; set; } = string.Empty;
        public string TitleName { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public DateTime PublishedDate { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string ImageThumbnailUrl { get; set; } = string.Empty;
    }
}
