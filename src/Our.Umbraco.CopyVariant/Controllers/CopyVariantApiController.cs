using Our.Umbraco.CopyVariant.Models;
using System.Linq;


#if NETFRAMEWORK
using System.Web.Http;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web.WebApi;
using Umbraco.Web.WebApi.Filters;
#else
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Extensions;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core.Services;
#endif


namespace Our.Umbraco.CopyVariant.Controllers
{
	//Route: ~/Umbraco/Api/[YourControllerName]

	public class CopyVariantApiController : UmbracoAuthorizedApiController
	{

#if NET5_0_OR_GREATER
        public ServiceContext Services { get; }
        public CopyVariantApiController(ServiceContext services)
		{
			Services = services;
		}
#endif

        [HttpGet]
		public bool GetApi() => true;

		public AvailableCultures AvailableCultures(int id)
		{
			var content = Services.ContentService.GetById(id);

			if (content == null)
				return default;
            
			return new AvailableCultures()
			{
				Cultures = content.PublishedCultures,
				Properties = content.Properties.Where(x => x.PropertyType.VariesByCulture()).ToDictionary(x => x.Alias, y => y.PropertyType.Name)
			};
		}

		[HttpPost]
		public bool CopyCulture(CopyCulture model)
		{
			var content = Services.ContentService.GetById(model.Id);

			if (content == null)
				return default;

			//if (content.IsCultureAvailable(model.FromCulture)) throw new System.Exception($"From {model.FromCulture} is not available");

			if (!model.Create && !content.IsCultureAvailable(model.ToCulture)) throw new System.Exception($"To {model.ToCulture} is not available");

			if (model.Create && !content.IsCultureAvailable(model.ToCulture))
			{
				content.SetCultureName($"{content.Name} ({model.ToCulture})", model.ToCulture);
				Services.ContentService.Save(content);
			}

			var complexProperty = new string[] {
				//global::Umbraco.Core.Constants.PropertyEditors.Aliases.NestedContent,
				//global::Umbraco.Core.Constants.PropertyEditors.Aliases.BlockList
			};

			foreach (var property in content.Properties.Where(x => 
				x.PropertyType.VariesByCulture() &&
				!complexProperty.Contains(x.PropertyType.Alias) &&
				(model.Properties.Any() && model.Properties.Contains(x.Alias))
			))
			{
				var fromObj = property.GetValue(model.FromCulture);

				if (fromObj == null)
					continue;

				var toObj = property.GetValue(model.ToCulture);

				if (toObj == null || (toObj != null && model.Overwrite))
					property.SetValue(fromObj, model.ToCulture);
			}

			if(model.Publish)
				return Services.ContentService.SaveAndPublish(content).Success;

			return Services.ContentService.Save(content).Success;
		}

	}
}