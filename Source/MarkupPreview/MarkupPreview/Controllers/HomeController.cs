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