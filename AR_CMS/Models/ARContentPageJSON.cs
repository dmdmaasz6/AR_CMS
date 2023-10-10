using Microsoft.Extensions.Options;
using Piranha.Extend.Fields;
using Piranha.Extend;
using Piranha.Models;
#nullable disable

namespace AR_CMS.Models
{
    public class ARItemJSON
    {
        public string ContentLink { get; set; }

        public int ContentType { get; set; }

        public string Link { get; set; }

        public string Name { get; set; }
    }

    public class ARContentPageJSON
    {
        public ARItemJSON[] TrackImageItem { get; set; }
    }
}
