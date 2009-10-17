#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkupPreview.Processing
{
  using anrControls;

  /// <summary>
  /// Processes markup according to the Markdown syntax.
  /// </summary>
  public class MarkdownProcessor : IMarkupProcessor
  {
    private readonly Markdown internalProcessor = new Markdown();

    public string Process(string input)
    {
      if (input == null)
      {
        return string.Empty;
      }

      return internalProcessor.Transform(input);
    }
  }
}