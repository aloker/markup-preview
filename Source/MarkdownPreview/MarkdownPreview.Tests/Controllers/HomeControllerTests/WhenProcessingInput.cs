#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkdownPreview.Controllers.HomeControllerTests
{
  using NUnit.Framework;

  [TestFixture]
  public class WhenProcessingInput : ControllerTest<HomeController>
  {
    [Test]
    public void ShouldSaveInputAsResult()
    {
      ExecuteAction(x => x.Process("foobar"));

      Assert.That(ControllerContext.PropertyBag["result"], Is.EqualTo("foobar"));
    }
  }
}