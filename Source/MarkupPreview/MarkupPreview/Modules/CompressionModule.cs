#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met:
// 
// * Redistributions of source code must retain the above copyright notice, 
//   this list of conditions and the following disclaimer.
// * Redistributions in binary form must reproduce the above copyright notice, 
//   this list of conditions and the following disclaimer in the documentation 
//   and/or other materials provided with the distribution.
// * Neither the name of Andre Loker nor the names of the project contributors 
//   may be used to endorse or promote products derived from this software 
//   without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE.
#endregion

namespace MarkupPreview.Modules
{
  using System;
  using System.IO;
  using System.IO.Compression;
  using System.Linq;
  using System.Web;
  using Castle.MonoRail.Framework;

  /// <summary>
  /// Provides compression for MonoRail documents.
  /// </summary>
  /// <remarks>
  /// Untested (taken from a different project)
  /// </remarks>
  public class CompressionModule : IHttpModule
  {
    public static readonly string EncodingGzip = "gzip";
    public static readonly string EncodingDeflate = "deflate";
    public static readonly string HeaderAcceptEncoding = "Accept-Encoding";
    public static readonly string HeaderVary = "Vary";
    public static readonly string HeaderContentEncoding = "Content-Encoding";
    private static readonly string[] compressibleTypes = { "text/html", "text/css", "text/javascript" };

    /// <summary>
    /// Defines the type of compression supported by the current request.
    /// </summary>
    private enum SupportedCompression
    {
      /// <summary>
      /// No compression is supported.
      /// </summary>
      None, 

      /// <summary>
      /// GZip compression is supported.
      /// </summary>
      Gzip, 

      /// <summary>
      /// Deflate compression is supported.
      /// </summary>
      Deflate
    }

    public void Init(HttpApplication context)
    {
      context.PostRequestHandlerExecute += Process;
    }

    private static void Process(object sender, EventArgs e)
    {
      var ctx = HttpContext.Current;
      if (CanCompressContent(ctx))
      {
        CompressResponse(ctx);
      }
    }

    private static bool CanCompressContent(HttpContext ctx)
    {
      return ctx.Handler is MonoRailHttpHandler &&
             compressibleTypes.Any(x => x.Equals(ctx.Response.ContentType));
    }

    private static void CompressResponse(HttpContext context)
    {
      switch (GetSupportedCompression(context))
      {
        case SupportedCompression.Gzip:
          SetupCompressionFilter(context, CreateGzipWrapper(context.Response.Filter), EncodingGzip);
          break;
        case SupportedCompression.Deflate:
          SetupCompressionFilter(context, CreateDeflateWrapper(context.Response.Filter), EncodingDeflate);
          break;
      }
    }

    private static SupportedCompression GetSupportedCompression(HttpContext context)
    {
      var userAgent = context.Request.UserAgent;

      if ((userAgent != null) && userAgent.Contains("MSIE 6"))
      {
        return SupportedCompression.None;
      }

      if (AcceptsEncoding(context, EncodingGzip))
      {
        return SupportedCompression.Gzip;
      }

      if (AcceptsEncoding(context, EncodingDeflate))
      {
        return SupportedCompression.Deflate;
      }

      return SupportedCompression.None;
    }

    private static GZipStream CreateGzipWrapper(Stream baseStream)
    {
      return new GZipStream(baseStream, CompressionMode.Compress, true);
    }

    private static DeflateStream CreateDeflateWrapper(Stream baseStream)
    {
      return new DeflateStream(baseStream, CompressionMode.Compress, true);
    }

    private static void SetupCompressionFilter(HttpContext context, Stream filter, string encoding)
    {
      context.Response.Filter = filter;
      context.Response.AppendHeader(HeaderContentEncoding, encoding);
      context.Response.AppendHeader(HeaderVary, HeaderAcceptEncoding);
    }

    private static bool AcceptsEncoding(HttpContext context, string encoding)
    {
      var header = context.Request.Headers[HeaderAcceptEncoding];
      return header != null && header.Contains(encoding);
    }

    public void Dispose()
    {
    }
  }
}