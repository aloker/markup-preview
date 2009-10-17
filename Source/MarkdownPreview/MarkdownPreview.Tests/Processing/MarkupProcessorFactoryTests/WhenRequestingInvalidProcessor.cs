#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkdownPreview.Processing.MarkupProcessorFactoryTests
{
  using System;
  using NUnit.Framework;

  [TestFixture]
  public class WhenRequestingInvalidProcessor
  {
    [Test]
    public void ShouldThrowArgumentOutOfRange()
    {
      var factory = new MarkupProcessorFactory();
      Assert.That(() => factory.GetProcessor((MarkupType)983243), Throws.InstanceOf<ArgumentOutOfRangeException>());
    }
  }
}