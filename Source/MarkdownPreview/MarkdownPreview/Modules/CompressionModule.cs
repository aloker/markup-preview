#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
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

    private enum SupportedCompression
    {
      None,
      Gzip,
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