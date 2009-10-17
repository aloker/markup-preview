#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
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