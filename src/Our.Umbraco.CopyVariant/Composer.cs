using Our.Umbraco.CopyVariant.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Umbraco.Core.Composing;
using Umbraco.Web;
using Umbraco.Web.JavaScript;

namespace Our.Umbraco.CopyVariant
{
    [global::Umbraco.Core.Composing.RuntimeLevel(MinLevel = global::Umbraco.Core.RuntimeLevel.Run)]
    public class CopyVariantComposer : ComponentComposer<CopyVariantComponent>
    { }

    public class CopyVariantComponent : IComponent
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
}
