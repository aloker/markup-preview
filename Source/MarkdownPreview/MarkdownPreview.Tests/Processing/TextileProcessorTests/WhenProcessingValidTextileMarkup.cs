#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkdownPreview.Processing.TextileProcessorTests
{
  using System.Collections.Generic;
  using System.IO;
  using NUnit.Framework;

  [TestFixture]
  public class WhenProcessingValidTextileMarkup
  {
    [Test]
    [TestCaseSource("GetTestData")]
    public void ShouldTransformTextileToHtml(string input, string expectedOutput)
    {
      var proc = new TextileProcessor();
      Assert.That(proc.Process(input).StripReturn(), Is.EqualTo(expectedOutput.StripReturn()));
    }

    private static IEnumerable<string[]> GetTestData()
    {
      yield return new[] { "h1.Test", "<h1>Test</h1>" };
      yield return new[] { " * An item", "<ul><li>An item</li></ul>" };

      yield return new[]
      {
        File.ReadAllText("Examples\\textile-input.txt"), 
        File.ReadAllText("Examples\\textile-expected.txt").StripReturn()
      };
    }
  }
}