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
  using System.Linq.Expressions;
  using Castle.MonoRail.Framework;
  using Castle.MonoRail.TestSupport;
  using NUnit.Framework;

  public class ControllerTest<T> : BaseControllerTest, IDisposable
    where T : Controller, new()
  {
    private T controller;

    public T Controller
    {
      get { return controller; }
    }

    [SetUp]
    public virtual void Setup()
    {
      controller = new T();
    }

    protected void PrepareAction(Expression<Action<T>> actionExpression)
    {
      var controllerName = GetControllerName();
      var actionName = ((MethodCallExpression)actionExpression.Body).Method.Name;
      PrepareController(controller, controllerName, actionName);
    }

    protected void ExecuteAction(Expression<Action<T>> actionExpression)
    {
      PrepareAction(actionExpression);
      actionExpression.Compile()(controller);
    }

    private static string GetControllerName()
    {
      const string Suffix = "Controller";
      var name = typeof(T).Name;
      if (name.EndsWith(Suffix, StringComparison.OrdinalIgnoreCase))
      {
        name = name.Substring(0, name.Length - Suffix.Length);
      }

      return name;
    }

    [TearDown]
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        controller.Dispose();
      }
    }
  }
}