using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace ReactViewEngine
{
    public class ReactRenderViewEngine : IViewEngine
    {
        public ViewEngineResult FindView(ActionContext context, string viewName, bool isMainPage)
            => ViewEngineResult.Found(viewName, new ReactView());

        public ViewEngineResult GetView(string executingFilePath, string viewPath, bool isMainPage)
            => ViewEngineResult.Found(viewPath, new ReactView());
    }
}
