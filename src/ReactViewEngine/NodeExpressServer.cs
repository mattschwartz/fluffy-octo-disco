using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ReactViewEngine
{
    class NodeExpressServer
    {
        private static Uri GetBaseUri(ViewContext context)
        {
            string uri = string.Format("http://localhost:{0}{1}",
                NodeInstance.Options.Port, 
                context.HttpContext.Request.Path);

            return new Uri(uri);
        }

        public static async Task RequestAsync(ViewContext context)
        {
            var request = WebRequest.Create(GetBaseUri(context));
            byte[] data = Encoding.UTF8.GetBytes(GetJson(context));

            foreach (var header in context.HttpContext.Request.Headers) {
                request.Headers[header.Key] = header.Value.ToString();
            }

            request.Method = HttpMethod.Post.Method;
            request.ContentType = "application/json";
            request.Headers["ContentLength"] = data.Length.ToString();

            using (var stream = await request.GetRequestStreamAsync()) {
                stream.Write(data, 0, data.Length);
            }

            string html = null;

            using (var response = await request.GetResponseAsync())
            using (var stream = response.GetResponseStream())
            using (var sr = new StreamReader(stream)) {
                html = await sr.ReadToEndAsync();
            }

            await context.Writer.WriteAsync(html);
        }

        public static string GetJson(ViewContext context)
        {
            object model = context.ViewData.Model;
            return JsonConvert.SerializeObject(model, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
        }
    }
}
