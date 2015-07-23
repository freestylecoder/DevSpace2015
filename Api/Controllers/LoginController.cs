using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace DevSpace.Api.Controllers {
	[AllowAnonymous]
	public class LoginController : ApiController {
		public async Task<Int16> Get() {
			if( ( Thread.CurrentPrincipal != null ) && ( Thread.CurrentPrincipal.Identity != null ) && Thread.CurrentPrincipal.Identity.IsAuthenticated ) {
				using( SqlConnection Connection = new SqlConnection( RoleEnvironment.GetConfigurationSettingValue( "DatabaseConnectionString" ) ) ) {
					Connection.Open();
					Models.Speaker SelectedSpeaker = await Models.Speaker.Select( Thread.CurrentPrincipal.Identity.Name, Connection );

					if( null == SelectedSpeaker ) {
						return -1;
					}

					return SelectedSpeaker.Id;
				}
			}

			return -1;
		}
	}
}
