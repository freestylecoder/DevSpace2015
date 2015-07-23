using System;
using System.Net;
using Microsoft.Owin.Hosting;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace DevSpace.Api {
	public class WorkerRole : RoleEntryPoint {
		private IDisposable HttpApp = null;
		private IDisposable HttpsApp = null;

		public override bool OnStart() {
			// Set the maximum number of concurrent connections
			ServicePointManager.DefaultConnectionLimit = 12;

			// HttpEndpoint
			RoleInstanceEndpoint HttpEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["Http"];
			string BaseUri = string.Format( "{0}://{1}", HttpEndpoint.Protocol, HttpEndpoint.IPEndpoint );
			HttpApp = WebApp.Start<Startup>( new StartOptions( url: BaseUri ) );

			// HttpsEndpoint
			RoleInstanceEndpoint HttpsEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["Https"];
			BaseUri = string.Format( "{0}://{1}", HttpsEndpoint.Protocol, HttpsEndpoint.IPEndpoint );
			HttpsApp = WebApp.Start<Startup>( new StartOptions( url: BaseUri ) );

			return base.OnStart();
		}

		public override void OnStop() {
			if( null != HttpApp )
				HttpApp.Dispose();

			if( null != HttpsApp )
				HttpsApp.Dispose();

			base.OnStop();
		}
	}
}
