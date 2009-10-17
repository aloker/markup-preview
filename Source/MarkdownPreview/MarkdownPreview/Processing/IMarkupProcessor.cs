#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkdownPreview.Processing
{

  /// <summary>
  /// Common interface for types that can act as a markup processor.
  /// </summary>
  public interface IMarkupProcessor
  {
    string Process(string input);
  }
}