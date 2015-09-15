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
using DevSpace.Api.Models;
using Newtonsoft.Json;

namespace DevSpace.Api.Controllers {
	public class SpeakersController : ApiController {
		[AllowAnonymous]
		public async Task<HttpResponseMessage> Get() {
			using( SqlConnection Connection = new SqlConnection( RoleEnvironment.GetConfigurationSettingValue( "DatabaseConnectionString" ) ) ) {
				Connection.Open();
				List<Models.Speaker> Speakers = await Models.Speaker.Select( Connection );

				if( null == Speakers ) {
					return new HttpResponseMessage( HttpStatusCode.NotFound );
				}

				HttpResponseMessage Response = new HttpResponseMessage( HttpStatusCode.OK );
				Response.Content = new StringContent( await JsonConvert.SerializeObjectAsync( Speakers, Formatting.None ) );
				return Response;
			}
		}

		[AllowAnonymous]
		public async Task<HttpResponseMessage> Get( int Id ) {
			using( SqlConnection Connection = new SqlConnection( RoleEnvironment.GetConfigurationSettingValue( "DatabaseConnectionString" ) ) ) {
				Connection.Open();
				Models.Speaker SelectedSpeaker = await Models.Speaker.Select( Id, Connection );

				if( null == SelectedSpeaker ) {
					return new HttpResponseMessage( HttpStatusCode.NotFound );
				}

				HttpResponseMessage Response = new HttpResponseMessage( HttpStatusCode.OK );
				Response.Content = new StringContent( await JsonConvert.SerializeObjectAsync( SelectedSpeaker, Formatting.None ) );
				return Response;
			}
		}

		[AllowAnonymous]
		public async Task<HttpResponseMessage> Post( Models.Speaker NewSpeaker ) {
			using( SqlConnection Connection = new SqlConnection( RoleEnvironment.GetConfigurationSettingValue( "DatabaseConnectionString" ) ) ) {
				Connection.Open();

				Models.Speaker ExistingRecord = await Models.Speaker.Select( NewSpeaker.EmailAddress, Connection );

				// Register new profile
				if( null == ExistingRecord ) {
					NewSpeaker.PasswordHash = System.Security.Cryptography.SHA512Managed.Create().ComputeHash( Encoding.UTF8.GetBytes( NewSpeaker.EmailAddress + ":" + NewSpeaker.Password ) );
					await NewSpeaker.Insert( Connection );
					return new HttpResponseMessage( HttpStatusCode.Created );
				}

				// You can only update yourself
				if( ( Thread.CurrentPrincipal == null ) || ( Thread.CurrentPrincipal.Identity == null ) || !Thread.CurrentPrincipal.Identity.IsAuthenticated ) {
					return new HttpResponseMessage( HttpStatusCode.Unauthorized );
				}

				if( !Thread.CurrentPrincipal.Identity.Name.ToUpper().Equals( NewSpeaker.EmailAddress.ToUpper() ) ) {
					return new HttpResponseMessage( HttpStatusCode.Unauthorized );
				}

				if( !string.IsNullOrWhiteSpace( NewSpeaker.Password ) ) {
					NewSpeaker.PasswordHash = System.Security.Cryptography.SHA512Managed.Create().ComputeHash( Encoding.UTF8.GetBytes( NewSpeaker.EmailAddress + ":" + NewSpeaker.Password ) );
				}

				await NewSpeaker.Update( Connection );
				return new HttpResponseMessage( HttpStatusCode.OK );
			}
		}

		[Authorize]
		public async Task<HttpResponseMessage> Post( Int16 Id, Models.Speaker NewSpeaker ) {
			using( SqlConnection Connection = new SqlConnection( RoleEnvironment.GetConfigurationSettingValue( "DatabaseConnectionString" ) ) ) {
				Connection.Open();

				Models.Speaker ExistingRecord = await Models.Speaker.Select( Id, Connection );

				// Register new profile
				if( null == ExistingRecord ) {
					return new HttpResponseMessage( HttpStatusCode.NotFound );
				}

				if( string.IsNullOrWhiteSpace( NewSpeaker.EmailAddress ) ) {
					return new HttpResponseMessage( HttpStatusCode.BadRequest );
				}

				if( string.IsNullOrWhiteSpace( NewSpeaker.Password ) ) {
					return new HttpResponseMessage( HttpStatusCode.BadRequest );
				}

				ExistingRecord.EmailAddress = NewSpeaker.EmailAddress;
				ExistingRecord.Password = NewSpeaker.Password;

				await ExistingRecord.Update( Connection );
				return new HttpResponseMessage( HttpStatusCode.OK );
			}
		}
	}
}
