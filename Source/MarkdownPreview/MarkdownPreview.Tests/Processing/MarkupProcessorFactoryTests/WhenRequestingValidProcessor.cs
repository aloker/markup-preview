#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkdownPreview.Processing.MarkupProcessorFactoryTests
{
  using System;
  using NUnit.Framework;

  [TestFixture]
  public class WhenRequestingValidProcessor
  {
    [TestCase(MarkupType.Markdown, typeof(MarkdownProcessor))]
    [TestCase(MarkupType.Textile, typeof(TextileProcessor))]
    public void ShouldReturnCorrectInstance(MarkupType type, Type expectedProcessor)
    {
      var factory = new MarkupProcessorFactory();
      var processor = factory.GetProcessor(type);
      Assert.That(processor, Is.InstanceOf(expectedProcessor));
    }
  }
}