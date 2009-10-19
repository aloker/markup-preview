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

namespace MarkupPreview.Controllers.HomeControllerTests
{
  using System;
  using NUnit.Framework;
  using Processing;
  using Rhino.Mocks;

  [TestFixture]
  public class WhenProvidingInvalidMarkupType : ControllerTest<HomeController>
  {
    public override void Setup()
    {
      base.Setup();
      Controller.ProcessorFactory = MockRepository.GenerateStub<IMarkupProcessorFactory>();
      Controller.ProcessorFactory.Stub(x => x.GetProcessor(Arg<MarkupType>.Is.Anything)).Throw(
        new ArgumentOutOfRangeException("markupType"));
    }

    [Test]
    public void ShouldRenderErrorView()
    {
      ExecuteAction(x => x.Process(MarkupType.Markdown, "somthing"));

      Assert.That(ControllerContext.SelectedViewName, Is.EqualTo("Home\\error"));
    }

    [Test]
    public void ShouldSetErrorMessage()
    {
      ExecuteAction(x => x.Process(MarkupType.Markdown, "somthing"));
      Assert.That(ControllerContext.PropertyBag["message"], Is.Not.Null);
    }
  }
}