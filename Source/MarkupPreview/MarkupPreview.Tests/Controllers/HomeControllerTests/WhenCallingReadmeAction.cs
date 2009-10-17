#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkupPreview.Controllers.HomeControllerTests
{
  using Processing;
  using NUnit.Framework;

  [TestFixture]
  public class WhenCallingReadmeAction : ControllerTest<HomeController>
  {
    public override void Setup()
    {
      base.Setup();
      ExecuteAction(x => x.Readme());
    }

    [Test]
    public void ShouldLoadReadmeFromResource()
    {
      Assert.That(ControllerContext.PropertyBag["source"].ToString().Contains("Markup Preview"));
    }

    [Test]
    public void ShouldSetType()
    {
      Assert.That(ControllerContext.PropertyBag["type"], Is.EqualTo(MarkupType.Markdown));
    }

    [Test]
    public void ShouldSetIndexView()
    {
      Assert.That(ControllerContext.SelectedViewName, Is.EqualTo("Home\\Index").IgnoreCase);
    }

    [Test]
    public void ShouldLoadMarkupTypes()
    {
      Assert.That(ControllerContext.PropertyBag["markupTypes"], Is.Not.Empty);
    }
  }
}