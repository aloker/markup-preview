#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkupPreview.Controllers
{
  using System;
  using System.IO;
  using System.Reflection;
  using Castle.MonoRail.Framework;
  using Processing;

  [Layout("default")]
  public class HomeController : SmartDispatcherController
  {
    public IMarkupProcessorFactory ProcessorFactory { get; set; }

    public void Index()
    {
      LoadMarkupTypes();
    }

    private void LoadMarkupTypes()
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

    public void Readme()
    {
      var assembly = Assembly.GetExecutingAssembly();
      string content;

      using (var stream = assembly.GetManifestResourceStream("MarkupPreview.Resources.README.md"))
      {
        content = new StreamReader(stream).ReadToEnd();
      }

      PropertyBag["source"] = content;
      PropertyBag["type"] = MarkupType.Markdown;
      LoadMarkupTypes();
      RenderView("index");
    }
  }
}