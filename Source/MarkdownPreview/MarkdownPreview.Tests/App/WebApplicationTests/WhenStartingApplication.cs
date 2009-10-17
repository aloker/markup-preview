#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkdownPreview.App.WebApplicationTests
{
  using System;
  using System.Linq;
  using Castle.MonoRail.Framework.Configuration;
  using Castle.MonoRail.Framework.Container;
  using Castle.MonoRail.Framework.Routing;
  using Castle.MonoRail.Framework.Services;
  using Castle.MonoRail.Views.Brail;
  using Controllers;
  using Processing;
  using NUnit.Framework;
  using Rhino.Mocks;

  [TestFixture]
  public sealed class WhenStartingApplication : IDisposable
  {
    private WebApplication app;

    [SetUp]
    public void Setup()
    {
      app = new WebApplication(new RoutingRuleContainer());
      app.Start();
    }

    [TearDown]
    public void Dispose()
    {
      app.Dispose();
    }

    [Test]
    public void ShouldRegisterControllers()
    {
      Assert.That(app.Container.Kernel.GetHandler(typeof(HomeController)), Is.Not.Null);
    }

    [Test]
    public void ShouldSetupBrail()
    {
      var config = new MonoRailConfiguration();
      app.Configure(config);

      Assert.That(config.ViewEngineConfig.ViewPathRoot.EndsWith("\\Views", StringComparison.OrdinalIgnoreCase));
      Assert.That(config.ViewEngineConfig.ViewEngines.Any(x => x.Engine == typeof(BooViewEngine)));
    }

    [Test]
    public void ShouldDisableExtensions()
    {
      var container = MockRepository.GenerateStub<IMonoRailContainer>();
      var urlBuilder = container.UrlBuilder = MockRepository.GenerateMock<IUrlBuilder>();

      app.Initialized(container);

      urlBuilder.AssertWasCalled(x => x.UseExtensions = false);
    }

    [Test]
    public void ShouldHaveRegisterdMarkupProcessorFactory()
    {
      Assert.That(app.Container.Kernel.GetHandler(typeof(IMarkupProcessorFactory)), Is.Not.Null);
    }
  }
}