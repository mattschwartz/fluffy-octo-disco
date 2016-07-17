using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace ReactViewEngine
{
    public static class Extensions
    {
        /// <summary>
        /// Adds the react view engine to the .NET Core render view engines
        /// which invokes calls to an internal node express server to handle
        /// the rendering of a React web app.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="nodeOptions"></param>
        /// <param name="isDevelopment">
        /// If false, will start the node server in the background, which you 
        /// should only do if you don't want to see helpful errors.
        /// </param>
        public static void AddReactViewEngine(
            this IServiceCollection services, 
            NodeServerOptions nodeOptions,
            bool isDevelopment = false)
        {
            if (nodeOptions == null) {
                throw new ArgumentNullException(nameof(nodeOptions));
            }

            NodeInstance.Options = nodeOptions;

            if (isDevelopment) {
                NodeInstance.Start();
            } else {
                Task.Run(() => NodeInstance.Start());
            }

            services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MvcViewOptions>, ReactViewEngineMvcViewOptionsSetup>());
            services.AddTransient<IViewEngine, ReactRenderViewEngine>();
        }

        private class ReactViewEngineMvcViewOptionsSetup : ConfigureOptions<MvcViewOptions>
        {
            public ReactViewEngineMvcViewOptionsSetup(IServiceProvider provider) 
                : base(o => ConfigureMvc(provider, o))
            {
            }

            public static void ConfigureMvc(IServiceProvider provider, MvcViewOptions options)
            {
                var reactViewEngine = provider.GetRequiredService<IViewEngine>();
                options.ViewEngines.Add(reactViewEngine);
            }
        }
    }
}
