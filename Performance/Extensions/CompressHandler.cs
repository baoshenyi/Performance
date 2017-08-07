using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;

namespace Performance.Extensions
{
    public class CompressionHandler : DelegatingHandler
    {
        public Collection<ICompressor> Compressors { get; private set; }

        public CompressionHandler()
        {
            Compressors = new Collection<ICompressor>();

            Compressors.Add(new GZipCompressor());
            Compressors.Add(new DeflateCompressor());
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (request.Headers.AcceptEncoding != null)
            {
                var encoding = request.Headers.AcceptEncoding.First();

                var compressor = Compressors.FirstOrDefault(c => c.EncodingType.Equals(encoding.Value, StringComparison.InvariantCultureIgnoreCase));

                if (compressor != null)
                {
                    response.Content = new CompressedContent(response.Content, compressor);
                }
            }

            return response;
        }
    }
}