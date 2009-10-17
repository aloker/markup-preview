#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkdownPreview.Processing.MarkdownProcessorTests
{
  using System.Collections.Generic;
  using System.IO;
  using NUnit.Framework;

  [TestFixture]
  public class WhenProcessingValidMarkdownInput
  {
    [Test]
    [TestCaseSource("GetTestData")]
    public void ShouldTransformMarkdownToHtml(string input, string expectedOutput)
    {
      var proc = new MarkdownProcessor();
      Assert.That(proc.Process(input).StripReturn(), Is.EqualTo(expectedOutput.StripReturn()));
    }

    private static IEnumerable<string[]> GetTestData()
    {
      yield return new[] { "Test\n====", "<h1>Test</h1>\n" };
      yield return new[] { "### Test", "<h3>Test</h3>\n" };

      yield return new[]
      {
        File.ReadAllText("Examples\\markdown-input.txt"), 
        File.ReadAllText("Examples\\markdown-expected.txt").StripReturn()
      };
    }
  }
}