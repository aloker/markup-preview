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

namespace MarkupPreview.App
{
  using System;
  using System.IO;
  using System.Reflection;
  using System.Web;
  using Castle.MicroKernel.Registration;
  using Castle.MonoRail.Framework;
  using Castle.MonoRail.Framework.Configuration;
  using Castle.MonoRail.Framework.Container;
  using Castle.MonoRail.Framework.Internal;
  using Castle.MonoRail.Framework.Routing;
  using Castle.MonoRail.Views.Brail;
  using Castle.MonoRail.WindsorExtension;
  using Castle.Windsor;
  using Processing;

  public class WebApplication : HttpApplication, 
                                IContainerAccessor, 
                                IMonoRailConfigurationEvents, 
                                IMonoRailContainerEvents
  {
    private readonly IRoutingRuleContainer routingRuleContainer;
    private static IWindsorContainer windsorContainer;

    public WebApplication()
      : this(RoutingModuleEx.Engine)
    {
    }

    public WebApplication(IRoutingRuleContainer routingRuleContainer)
    {
      this.routingRuleContainer = routingRuleContainer;
    }

    public IRoutingRuleContainer RoutingRuleContainer
    {
      get { return routingRuleContainer; }
    }

    public IWindsorContainer Container
    {
      get { return windsorContainer; }
    }

    public void Start()
    {
      InitializeContainer();
      RegisterComponents();
      InstallRoutes();
    }

    public void Configure(IMonoRailConfiguration configuration)
    {
      SetupBrailViewEngine(configuration);
    }

    private static void SetupBrailViewEngine(IMonoRailConfiguration configuration)
    {
      var viewEngineConfig = configuration.ViewEngineConfig;
      viewEngineConfig.ViewPathRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Views");
      viewEngineConfig.ViewEngines.Add(new ViewEngineInfo(typeof(BooViewEngine), false));
    }

    [CoverageExclude]
    public void Created(IMonoRailContainer container)
    {
    }

    public void Initialized(IMonoRailContainer container)
    {
      container.UrlBuilder.UseExtensions = false;
    }

    private void InstallRoutes()
    {
      MainRoutes.Setup(routingRuleContainer);
    }

    private static void InitializeContainer()
    {
      windsorContainer = new WindsorContainer();
      windsorContainer.AddFacility<MonoRailFacility>();
    }

    private static void RegisterComponents()
    {
      windsorContainer.Register(AllTypes.FromAssembly(Assembly.GetExecutingAssembly()).BasedOn<Controller>());
      windsorContainer.Register(
        Component
          .For<IMarkupProcessorFactory>()
          .ImplementedBy<MarkupProcessorFactory>()
          .LifeStyle.Singleton);
    }
  }
}