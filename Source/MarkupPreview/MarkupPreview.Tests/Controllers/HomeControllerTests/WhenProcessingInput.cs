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
  using NUnit.Framework;
  using Processing;
  using Rhino.Mocks;

  [TestFixture]
  public class WhenProcessingInput : ControllerTest<HomeController>
  {
    [Test]
    public void ShouldUseProcessorFactoryToFindProcessor()
    {
      Controller.ProcessorFactory = MockRepository.GenerateMock<IMarkupProcessorFactory>();
      Controller.ProcessorFactory
        .Expect(x => x.GetProcessor(MarkupType.Markdown))
        .Return(MockRepository.GenerateStub<IMarkupProcessor>());

      ExecuteAction(x => x.Process(MarkupType.Markdown, "dont care"));

      Controller.ProcessorFactory.VerifyAllExpectations();
    }

    [Test]
    public void ShouldUseSelectedProcessor()
    {
      var processor = MockRepository.GenerateMock<IMarkupProcessor>();
      Controller.ProcessorFactory = MockRepository.GenerateStub<IMarkupProcessorFactory>();
      Controller.ProcessorFactory.Stub(x => x.GetProcessor(MarkupType.Textile)).Return(processor);

      const string Inputstring = "inputstring";

      ExecuteAction(x => x.Process(MarkupType.Textile, Inputstring));

      processor.AssertWasCalled(x => x.Process(Inputstring));
    }

    [Test]
    public void ShouldAddProcessedMarkupToPropertyBag()
    {
      const string InputString = "This is the input";
      const string OutputString = "This is the output";
      var processor = MockRepository.GenerateStub<IMarkupProcessor>();
      processor.Stub(x => x.Process(InputString)).Return(OutputString);
      Controller.ProcessorFactory = MockRepository.GenerateStub<IMarkupProcessorFactory>();
      Controller.ProcessorFactory.Stub(x => x.GetProcessor(MarkupType.Textile)).Return(processor);

      ExecuteAction(x => x.Process(MarkupType.Textile, InputString));
      Assert.That(ControllerContext.PropertyBag["result"], Is.EqualTo(OutputString));
    }
  }
}