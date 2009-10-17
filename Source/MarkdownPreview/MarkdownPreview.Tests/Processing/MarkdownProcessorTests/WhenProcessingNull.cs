#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkdownPreview.Processing.MarkdownProcessorTests
{
  using NUnit.Framework;

  [TestFixture]
  public class WhenProcessingNull
  {
    [Test]
    public void ShouldReturnEmptyString()
    {
      var proc = new MarkdownProcessor();
      Assert.That(proc.Process(null), Is.EqualTo(string.Empty));
    }
  }
}