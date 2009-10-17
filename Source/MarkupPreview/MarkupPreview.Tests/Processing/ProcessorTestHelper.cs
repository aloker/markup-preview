#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkupPreview.Processing
{
  public static class ProcessorTestHelper
  {
    public static string StripReturn(this string input)
    {
      if (input == null)
      {
        return null;
      }

      return input.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty);
    }
  }
}