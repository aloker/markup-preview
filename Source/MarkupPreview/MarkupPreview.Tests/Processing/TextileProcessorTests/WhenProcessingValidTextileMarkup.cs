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

namespace MarkupPreview.Processing.TextileProcessorTests
{
  using System.Collections.Generic;
  using System.IO;
  using NUnit.Framework;

  [TestFixture]
  public class WhenProcessingValidTextileMarkup
  {
    [Test]
    [TestCaseSource("GetTestData")]
    public void ShouldTransformTextileToHtml(string input, string expectedOutput)
    {
      var proc = new TextileProcessor();
      Assert.That(proc.Process(input).StripReturn(), Is.EqualTo(expectedOutput.StripReturn()));
    }

    private static IEnumerable<string[]> GetTestData()
    {
      yield return new[] { "h1.Test", "<h1>Test</h1>" };
      yield return new[] { " * An item", "<ul><li>An item</li></ul>" };

      yield return new[]
      {
        File.ReadAllText("Examples\\textile-input.txt"), 
        File.ReadAllText("Examples\\textile-expected.txt").StripReturn()
      };
    }
  }
}