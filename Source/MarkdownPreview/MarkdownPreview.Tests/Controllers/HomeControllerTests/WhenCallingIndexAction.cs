#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkdownPreview.Controllers.HomeControllerTests
{
  using NUnit.Framework;

  [TestFixture]
  public class WhenCallingIndexAction : ControllerTest<HomeController>
  {
    [Test]
    public void ShouldSetMarkupTypes()
    {
      ExecuteAction(x => x.Index());

      Assert.That(ControllerContext.PropertyBag["markupTypes"], Is.Not.Empty);
    }
  }
}