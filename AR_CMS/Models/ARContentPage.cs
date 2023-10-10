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
    public enum ContentType
    {
        [Display(Description = "3D Object Models")]
        Object,
        [Display(Description = "Image Content")]
        Image,
        [Display(Description = "Video Content")]
        Video
    }

    [BlockType(Name = "AR Content Segment", Category = "Content", Icon = "fas fa-paragraph")]
    public class ARItem
    {
        [Field(Title = "Content for Tracked Image")]
        public MediaField Content { get; set; }

        [Field(Title = "Content Type", Options = FieldOption.HalfWidth)]
        public SelectField<ContentType> ContentType { get; set; }

        [Field(Title = "Tracked Image")]
        public MediaField TrackedImage { get; set; }

        [Field(Title = "Name")]
        public StringField Name { get; set; }

    }

    [PageType(Title = "AR Artifacts Page", UseBlocks = true, IsArchive = true)]
    public class ARContentPage : Page<ARContentPage>
    {
        [Region(ListTitle = "Title")]
        public IList<ARItem> AR_Artifacts { get; set; }
    }

}
