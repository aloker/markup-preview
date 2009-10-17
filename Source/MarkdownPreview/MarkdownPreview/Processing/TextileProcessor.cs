#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkdownPreview.Processing
{
  using Textile;

  public class TextileProcessor : IMarkupProcessor
  {
    public string Process(string input)
    {
      if (input == null)
      {
        return string.Empty;
      }

      var output = new StringBuilderTextileFormatter();
      var formatter = new TextileFormatter(output);
      formatter.Format(input);
      return output.GetFormattedText();
    }
  }
}