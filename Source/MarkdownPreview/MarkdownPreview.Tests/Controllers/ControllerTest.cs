#region Copyright © 2009 Andre Loker (mail@andreloker.de). All rights reserved.
// $Id$
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