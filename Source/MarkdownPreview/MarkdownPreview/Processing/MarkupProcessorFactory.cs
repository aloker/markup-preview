#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkupPreview.Processing
{
  using System;

  public class MarkupProcessorFactory : IMarkupProcessorFactory
  {
    public IMarkupProcessor GetProcessor(MarkupType markupType)
    {
      switch (markupType)
      {
        case MarkupType.Markdown:
          return new MarkdownProcessor();
        case MarkupType.Textile:
          return new TextileProcessor();
        default:
          throw new ArgumentOutOfRangeException("markupType");
      }
    }
  }
}