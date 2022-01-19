using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkDotNetExtended.Compression.Files
{
    public class CompressedFileMemory : CompressedFile
    {
        public byte[] Content { get; set; }
    }
}
