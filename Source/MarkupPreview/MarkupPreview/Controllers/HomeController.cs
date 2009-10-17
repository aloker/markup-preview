#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkupPreview.Controllers
{
  using System;
  using Castle.MonoRail.Framework;
  using Processing;

  [Layout("default")]
  public class HomeController : SmartDispatcherController
  {
    public IMarkupProcessorFactory ProcessorFactory { get; set; }

    public void Index()
    {
      PropertyBag["markupTypes"] = Enum.GetValues(typeof(MarkupType));
    }

    public void Process(MarkupType type, string source)
    {
      try
      {
        var processor = ProcessorFactory.GetProcessor(type);
        PropertyBag["result"] = processor.Process(source);
      }
      catch (ArgumentOutOfRangeException)
      {
        RenderView("error");
        PropertyBag["message"] = "No suitable markup processor found";
      }
    }
  }
}