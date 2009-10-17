#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
#endregion

namespace MarkdownPreview.Controllers.HomeControllerTests
{
  using Processing;
  using NUnit.Framework;
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