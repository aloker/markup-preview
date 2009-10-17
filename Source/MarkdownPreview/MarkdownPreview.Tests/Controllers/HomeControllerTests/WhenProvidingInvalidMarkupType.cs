#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkupPreview.Controllers.HomeControllerTests
{
  using System;
  using NUnit.Framework;
  using Processing;
  using Rhino.Mocks;

  [TestFixture]
  public class WhenProvidingInvalidMarkupType : ControllerTest<HomeController>
  {
    public override void Setup()
    {
      base.Setup();
      Controller.ProcessorFactory = MockRepository.GenerateStub<IMarkupProcessorFactory>();
      Controller.ProcessorFactory.Stub(x => x.GetProcessor(Arg<MarkupType>.Is.Anything)).Throw(new ArgumentOutOfRangeException());
    }

    [Test]
    public void ShouldRenderErrorView()
    {
      ExecuteAction(x => x.Process(MarkupType.Markdown, "somthing"));

      Assert.That(ControllerContext.SelectedViewName, Is.EqualTo("Home\\error"));
    }

    [Test]
    public void ShouldSetErrorMessage()
    {
      ExecuteAction(x => x.Process(MarkupType.Markdown, "somthing"));
      Assert.That(ControllerContext.PropertyBag["message"], Is.Not.Null);
    }
  }
}