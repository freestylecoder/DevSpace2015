using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using DevSpace.Api.Models;
using System.Data.SqlClient;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace DevSpace.Api.Controllers {
	public class TagsController : ApiController {
		[AllowAnonymous]
		public async Task<HttpResponseMessage> Get() {
			using( SqlConnection Connection = new SqlConnection( RoleEnvironment.GetConfigurationSettingValue( "DatabaseConnectionString" ) ) ) {
				Connection.Open();

				HttpResponseMessage Response = new HttpResponseMessage( HttpStatusCode.OK );
				Response.Content = new StringContent( await JsonConvert.SerializeObjectAsync( await Models.Tag.Select( Connection ), Formatting.None ) );
				return Response;
			}
		}

		[Authorize]
		public async Task<HttpResponseMessage> Post( Models.Tag PostedTag ) {
			using( SqlConnection Connection = new SqlConnection( RoleEnvironment.GetConfigurationSettingValue( "DatabaseConnectionString" ) ) ) {
				Connection.Open();

				if( -1 == PostedTag.Id ) {
					await PostedTag.Insert( Connection );
					HttpResponseMessage Response = new HttpResponseMessage( HttpStatusCode.Created );
					Response.Content = new StringContent( await JsonConvert.SerializeObjectAsync( PostedTag ) );
					return Response;
				} else {
					await PostedTag.Update( Connection );
					return new HttpResponseMessage( HttpStatusCode.OK );
				}
			}
		}
	}
}
