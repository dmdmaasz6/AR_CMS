namespace AR_CMS.Models
{
    public class ARContentAboutPageJSON
    {
        public About about { get; set; }

        public class About
        {
            public string logo { get; set; }
            public string description { get; set; }
            public string link { get; set; }
        }
    }
}
