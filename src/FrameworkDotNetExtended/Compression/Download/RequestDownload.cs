namespace FrameworkDotNetExtended.Compression.Download
{
    using System.Collections.Generic;

    public class RequestDownload
    {
        public RequestDownload()
        {
            Ids = new List<string>();
        }

        public IList<string> Ids { get; set; }
        public string SuggestedFilename { get; set; }
    }
}
