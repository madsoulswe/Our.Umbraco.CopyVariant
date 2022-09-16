using Our.Umbraco.CopyVariant.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;


#if NETFRAMEWORK
using Umbraco.Core.Composing;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.JavaScript;
using System.Web.Mvc;
using System.Web.Routing;

#else
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core;
using Umbraco.Extensions;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Routing;
#endif

namespace Our.Umbraco.CopyVariant
{
#if NETFRAMEWORK
    [RuntimeLevel(MinLevel = RuntimeLevel.Run)]
    internal class CopyVariantComposer : ComponentComposer<CopyVariantComponent>
    { }

    internal class CopyVariantComponent : IComponent
    {
        public void Initialize()
        {
            ServerVariablesParser.Parsing += ServerVariablesParser_Parsing;
        }

        private void ServerVariablesParser_Parsing(object sender, Dictionary<string, object> e)
        {
            if (HttpContext.Current == null) throw new InvalidOperationException("HttpContext is null");
            var urlHelper = new UrlHelper(new RequestContext(new HttpContextWrapper(HttpContext.Current), new RouteData()));

            e["Our.Umbraco.CopyVariant"] = new Dictionary<string, object>()
            {
                { "apiController", urlHelper.GetUmbracoApiServiceBaseUrl<CopyVariantApiController>(controller => controller.GetApi()) }
            };
        }

        public void Terminate()
        {

        }
    }
#else
    internal class StartupComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.AddNotificationHandler<ServerVariablesParsingNotification, ServerVariablesParsingNotificationHandler>();
        }
    }

    internal class ServerVariablesParsingNotificationHandler : INotificationHandler<ServerVariablesParsingNotification>
    {
        private readonly LinkGenerator linkGenerator;

        public ServerVariablesParsingNotificationHandler(LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
        }

        public void Handle(ServerVariablesParsingNotification notification)
        {
            notification.ServerVariables.Add("Our.Umbraco.CopyVariant", new
            {
                apiController = linkGenerator.GetUmbracoApiServiceBaseUrl<CopyVariantApiController>(controller => controller.GetApi())
            });
        }
    }
#endif
}
