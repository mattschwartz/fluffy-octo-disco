using Microsoft.AspNetCore.Mvc.ViewEngines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ReactViewEngine
{
    public class ReactView : IView
    {
        public string Path { get; set; }

        public async Task RenderAsync(ViewContext context)
        {
            await NodeExpressServer.RequestAsync(context);
        }
    }
}
