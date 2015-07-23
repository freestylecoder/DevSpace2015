using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.WindowsAzure.ServiceRuntime;
using Newtonsoft.Json;
using DevSpace.Api.Models;

namespace DevSpace.Api.Controllers {
	public class SessionsController : ApiController {
		[AllowAnonymous]
		public async Task<HttpResponseMessage> Get( short Id ) {
			using( SqlConnection Connection = new SqlConnection( RoleEnvironment.GetConfigurationSettingValue( "DatabaseConnectionString" ) ) ) {
				Connection.Open();

				HttpResponseMessage Response = new HttpResponseMessage( HttpStatusCode.OK );
				Response.Content = new StringContent( await JsonConvert.SerializeObjectAsync( await Models.Session.Select( Id, Connection ), Formatting.None ) );
				return Response;
			}
		}

		[AllowAnonymous]
		[Route( "v1/Speakers/{Id}/Sessions" )]
		public async Task<HttpResponseMessage> GetSpeakerSessions( short Id ) {
			using( SqlConnection Connection = new SqlConnection( RoleEnvironment.GetConfigurationSettingValue( "DatabaseConnectionString" ) ) ) {
				Connection.Open();

				HttpResponseMessage Response = new HttpResponseMessage( HttpStatusCode.OK );
				Response.Content = new StringContent( await JsonConvert.SerializeObjectAsync( await Models.Session.SelectBySpeaker( Id, Connection ), Formatting.None ) );
				return Response;
			}
		}

		[Authorize]
		public async Task<HttpResponseMessage> Post( Models.Session PostedSession ) {
			using( SqlConnection Connection = new SqlConnection( RoleEnvironment.GetConfigurationSettingValue( "DatabaseConnectionString" ) ) ) {
				Connection.Open();
				HttpResponseMessage Response = new HttpResponseMessage();

				if( PostedSession.Id == -1 ) {
					Response.StatusCode = HttpStatusCode.Created;
					await PostedSession.Insert( Connection );
				} else {
					Response.StatusCode = HttpStatusCode.OK;
					await PostedSession.Update( Connection );
				}

				Response.Content = new StringContent( await Newtonsoft.Json.JsonConvert.SerializeObjectAsync( PostedSession, Formatting.None ) );
				return Response;
			}
		}


		[Authorize]
		public async Task<HttpResponseMessage> Delete( short Id ) {
			using( SqlConnection Connection = new SqlConnection( RoleEnvironment.GetConfigurationSettingValue( "DatabaseConnectionString" ) ) ) {
				Connection.Open();
				try {
					if( await Models.Session.Delete( Id, Connection ) ) {
						return new HttpResponseMessage( HttpStatusCode.NoContent );
					} else {
						return new HttpResponseMessage( HttpStatusCode.NotFound );
					}
				} catch {
					return new HttpResponseMessage( HttpStatusCode.InternalServerError );
				}
			}
		}
	}
}
