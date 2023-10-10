using Piranha.Data;
using Piranha.Extend.Fields;
using Piranha.Extend;
using System.Xml.Linq;
using Piranha.AttributeBuilder;
using Piranha.Models;
using System.IO.Compression;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace AR_CMS.Models
{
    [BlockType(Name = "AR About Content", Category = "Content", Icon = "fas fa-paragraph")]
    public class AboutContent
    {
        [Field(Title = "Logo for AR Application")]
        public MediaField Logo { get; set; }

        [Field(Title = "Description")]
        public HtmlField Description { get; set; }

        [Field(Title = "Link to Company Site")]
        public StringField SiteLink { get; set; }
    }

    [PageType(Title = "AR About Page", UseBlocks = true, IsArchive = true)]
    public class ARContentAboutPage : Page<ARContentAboutPage>
    {
        [Region(ListTitle = "About")]
        public AboutContent AboutContent { get; set; }
    }
}
