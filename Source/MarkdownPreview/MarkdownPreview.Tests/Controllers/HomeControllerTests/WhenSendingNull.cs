#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkdownPreview.Controllers.HomeControllerTests
{
  using NUnit.Framework;

  [TestFixture]
  public class WhenSendingNull : ControllerTest<HomeController>
  {
    [Test]
    public void ShouldSetEmptyStringAsResult()
    {
      ExecuteAction(x => x.Process(null));
      Assert.That(ControllerContext.PropertyBag["result"], Is.EqualTo(string.Empty));
    }
  }
}