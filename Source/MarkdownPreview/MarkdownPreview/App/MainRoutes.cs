#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkdownPreview.App
{
  using System;
  using Castle.MonoRail.Framework.Routing;

  public static class MainRoutes
  {
    public static void Setup(IRoutingRuleContainer routes)
    {
      routes.Add(new PatternRoute("home", "/[controller]/[action]")
        .DefaultForController().Is("home")
        .DefaultForAction().Is("index"));
    }
  }
}