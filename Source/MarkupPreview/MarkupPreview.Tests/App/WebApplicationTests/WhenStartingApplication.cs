#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// Redistribution and use in source and binary forms, with or without 
// modification, are permitted provided that the following conditions are met:
// 
// * Redistributions of source code must retain the above copyright notice, 
//   this list of conditions and the following disclaimer.
// * Redistributions in binary form must reproduce the above copyright notice, 
//   this list of conditions and the following disclaimer in the documentation 
//   and/or other materials provided with the distribution.
// * Neither the name of Andre Loker nor the names of the project contributors 
//   may be used to endorse or promote products derived from this software 
//   without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" 
// AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
// IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE 
// ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE 
// LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF 
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS 
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN 
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
// POSSIBILITY OF SUCH DAMAGE.
#endregion
namespace MarkupPreview.App.WebApplicationTests
{
  using System;
  using System.Linq;
  using Castle.MonoRail.Framework.Configuration;
  using Castle.MonoRail.Framework.Container;
  using Castle.MonoRail.Framework.Routing;
  using Castle.MonoRail.Framework.Services;
  using Castle.MonoRail.Views.Brail;
  using Controllers;
  using NUnit.Framework;
  using Processing;
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
    public void ShouldRegisterMarkupProcessorFactory()
    {
      Assert.That(app.Container.Kernel.GetHandler(typeof(IMarkupProcessorFactory)), Is.Not.Null);
    }
  }
}