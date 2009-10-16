#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkdownPreview.Controllers
{
  using Castle.MonoRail.Framework;

  [Layout("default")]
  public class HomeController : SmartDispatcherController
  {
    public void Index()
    {
    }

    public void Process(string source)
    {
      PropertyBag["result"] = source ?? string.Empty;
    }
  }
}