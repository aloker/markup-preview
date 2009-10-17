namespace MarkupPreview
{
  using System;
  using App;

  public class Global : WebApplication
  {
    [CoverageExclude]
    protected internal void Application_Start(object sender, EventArgs e)
    {
      Start();
    }
  }
}