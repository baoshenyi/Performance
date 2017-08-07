using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Performance.Extensions
{
    public interface ICompressor
    {
        string EncodingType { get; }
        Task Compress(Stream source, Stream destination);
        Task Decompress(Stream source, Stream destination);
    }
}