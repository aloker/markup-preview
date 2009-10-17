#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkdownPreview.Processing
{
  using System;

  /// <summary>
  /// Instantiates <see cref="IMarkupProcessor"/>s.
  /// </summary>
  public interface IMarkupProcessorFactory
  {
    /// <summary>
    /// Gets the processor for the given markup type.
    /// </summary>
    /// <param name="markupType">Type of the markup.</param>
    /// <returns>The processor for the given markup type</returns>
    /// <exception cref="ArgumentOutOfRangeException">The markup type is invalid.</exception>
    IMarkupProcessor GetProcessor(MarkupType markupType);
  }
}