#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkupPreview.App
{
  using Castle.MonoRail.Framework.Routing;

  public static class MainRoutes
  {
    public static void Setup(IRoutingRuleContainer routes)
    {
      routes.Add(new PatternRoute("readme", "/readme")
        .DefaultForController().Is("home")
        .DefaultForAction().Is("readme"));

      routes.Add(new PatternRoute("home", "/[controller]/[action]")
        .DefaultForController().Is("home")
        .DefaultForAction().Is("index"));
    }
  }
}