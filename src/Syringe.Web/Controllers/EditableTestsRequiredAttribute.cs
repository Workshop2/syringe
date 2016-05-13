using System.Reflection;
using System.Web.Mvc;
using Syringe.Client;
using Syringe.Core.Configuration;

namespace Syringe.Web.Controllers
{
	public class EditableTestsRequiredAttribute : ActionMethodSelectorAttribute
	{
		private readonly IConfiguration _config;

		public EditableTestsRequiredAttribute()
		{
			//
			// TODO: get rid of bastard DI. This will require DI wireup of the attribute:
			// (example: https://github.com/roadkillwiki/roadkill/blob/master/src/Roadkill.Core/DependencyResolution/MVC/MvcAttributeProvider.cs)
			//

			MvcConfiguration mvcConfiguration = MvcConfiguration.Load();
			var configClient = new ConfigurationClient(mvcConfiguration.ServiceUrl);
			_config = configClient.GetConfiguration();
		}

		public override bool IsValidForRequest(ControllerContext controllerContext, MethodInfo methodInfo)
		{
			if (_config.ReadonlyMode)
			{
				return false;
			}

			return true;
		}
	}
}