using Owin;
using System.Web.Http;
using System.Web.Http.Cors;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace DevSpace.Api {
	class Startup {
		public void Configuration( IAppBuilder App ) {
			HttpConfiguration Config = new HttpConfiguration();

			EnableCorsAttribute CorsOptions = new EnableCorsAttribute( RoleEnvironment.GetConfigurationSettingValue( "AllowedSites" ), "*", "*" );
			CorsOptions.SupportsCredentials = true;
			Config.EnableCors( CorsOptions );

			Config.MessageHandlers.Add( new Handlers.BasicAuthHandler() );
			Config.MessageHandlers.Add( new Handlers.SessionHandler() );
			Config.MessageHandlers.Add( new Handlers.ForcedAuthHandler() );

			Config.MapHttpAttributeRoutes();

			Config.Routes.MapHttpRoute(
				"Default",
				"v1/{controller}/{id}",
				new { id = RouteParameter.Optional } );

			App.UseWebApi( Config );
		}
	}
}