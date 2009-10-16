#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkdownPreview.App.WebApplicationTests
{
  using Castle.MonoRail.Framework.Routing;
  using NUnit.Framework;

  [TestFixture]
  public class WhenUsingDefaultConstructor
  {
    [Test]
    public void ShouldUseRoutingModuleEx()
    {
      Assert.That(new WebApplication().RoutingRuleContainer, Is.SameAs(RoutingModuleEx.Engine));
    }
  }
}