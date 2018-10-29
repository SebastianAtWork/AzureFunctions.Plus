using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Ninject.AzureFunctions.Tests.Utility
{
    public class FakeHttpRequest<T> : HttpRequest,IDisposable
    {
        public FakeHttpRequest(T body)
        {
            Body = new MemoryStream();
            Writer = new StreamWriter(Body);

            var serializedBody = JsonConvert.SerializeObject(body);
            Writer.WriteLine(serializedBody);
            Writer.Flush();
            Body.Seek(0, SeekOrigin.Begin);
            

        }

        public StreamWriter Writer { get; set; }

        public override Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public override HttpContext HttpContext { get; }
        public override string Method { get; set; }
        public override string Scheme { get; set; }
        public override bool IsHttps { get; set; }
        public override HostString Host { get; set; }
        public override PathString PathBase { get; set; }
        public override PathString Path { get; set; }
        public override QueryString QueryString { get; set; }
        public override IQueryCollection Query { get; set; }
        public override string Protocol { get; set; }
        public override IHeaderDictionary Headers { get; }
        public override IRequestCookieCollection Cookies { get; set; }
        public override long? ContentLength { get; set; }
        public override string ContentType { get; set; }
        public override Stream Body { get; set; }
        public override bool HasFormContentType { get; }
        public override IFormCollection Form { get; set; }

        public void Dispose()
        {
            Body?.Dispose();
            Writer?.Dispose();
        }
    }
}
