namespace FrameworkDotNetExtended.Compression.Download
{
    public class ResponseDownloadMemory : ResponseDownload
    {
        public byte[] Content { get; set; }

        public override void GenerateReponseDownload(byte[] content, string filename)
        {
            this.Content = content;
            this.Filename = filename;
        }
    }
}
