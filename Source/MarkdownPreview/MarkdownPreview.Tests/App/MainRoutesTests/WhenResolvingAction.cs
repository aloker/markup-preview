#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkdownPreview.App.MainRoutesTests
{
  using System.Collections;
  using Castle.MonoRail.Framework.Routing;
  using Castle.MonoRail.Framework.Test;
  using NUnit.Framework;

  [TestFixture]
  public class WhenResolvingAction
  {
    private RouteContext routeContext;
    private RoutingRuleContainer routingRuleContainer;

    [SetUp]
    public void Setup()
    {
      routingRuleContainer = new RoutingRuleContainer();
      MainRoutes.Setup(routingRuleContainer);
      routeContext = FakeContext();
    }

    [TestCase("/")]
    [TestCase("/home")]
    [TestCase("/home/index")]
    public void ShouldFindHomepage(string request)
    {
      CheckMatch(request, "home", "index");
    }

    [TestCase("/home/process")]
    public void ShouldFindProcess(string request)
    {
      CheckMatch(request, "home", "process");
    }

    private void CheckMatch(string request, string expectedController, string expectedAction)
    {
      var match = routingRuleContainer.FindMatch(request, routeContext);
      Assert.That(match, Is.Not.Null);
      Assert.That(match.Parameters["controller"], Is.EqualTo(expectedController));
      Assert.That(match.Parameters["action"], Is.EqualTo(expectedAction));
    }

    private static RouteContext FakeContext()
    {
      return new RouteContext(new StubRequest("GET"), null, "/", new Hashtable());
    }
  }
}