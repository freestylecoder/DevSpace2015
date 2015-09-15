using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace DevSpace.Api.Handlers {
	class SessionHandler : DelegatingHandler {
		private Guid GetSessionToken( HttpRequestMessage Request ) {
			if( ( null != Request.Headers.GetCookies() ) && ( 0 < Request.Headers.GetCookies().Count ) ) {
				foreach( CookieHeaderValue HeaderValue in Request.Headers.GetCookies() ) {
					foreach( CookieState State in HeaderValue.Cookies ) {
						if( State.Name == "SessionToken" ) {
							Guid SessionToken = Guid.Empty;
							if( Guid.TryParse( State.Value, out SessionToken ) ) {
								return SessionToken;
							}
						}
					}
				}
			}

			return Guid.Empty;
		}

		protected async override Task<HttpResponseMessage> SendAsync( HttpRequestMessage Request, CancellationToken CancelToken ) {
			Guid SessionToken = GetSessionToken( Request );
			if( !SessionToken.Equals( Guid.Empty ) ) {
				using( SqlConnection Connection = new SqlConnection( RoleEnvironment.GetConfigurationSettingValue( "DatabaseConnectionString" ) ) ) {
					Connection.Open();

					using( SqlCommand Command = new SqlCommand() ) {
						Command.Connection = Connection;
						Command.CommandText = "SELECT EmailAddress FROM Speakers WHERE SessionToken = @SessionToken AND SessionExpires > @CurrentTime";
						Command.Parameters.Add( "SessionToken", System.Data.SqlDbType.UniqueIdentifier ).Value = SessionToken;
						Command.Parameters.Add( "CurrentTime", System.Data.SqlDbType.DateTime ).Value = DateTime.UtcNow;

						using( SqlDataReader DataReader = await Command.ExecuteReaderAsync() ) {
							if( DataReader.Read() ) {
								GenericIdentity UserIdentity = new GenericIdentity( DataReader.GetString( 0 ) );
								GenericPrincipal UserPrincipal = new GenericPrincipal( UserIdentity, null );
								Thread.CurrentPrincipal = UserPrincipal;
								Request.GetRequestContext().Principal = UserPrincipal;
							} else {
								// return 401
								HttpResponseMessage Response401 = new HttpResponseMessage( System.Net.HttpStatusCode.Unauthorized );
								Response401.Headers.Add( "Access-Control-Allow-Origin", Request.Headers.GetValues( "Origin" ).First() );
								Response401.Headers.Add( "Access-Control-Allow-Credentials", "true" );
								return Response401;
							}
						}
					}

					HttpResponseMessage Response = await base.SendAsync( Request, CancelToken );

					using( SqlCommand Command = new SqlCommand() ) {
						Command.Connection = Connection;
						Command.CommandText = "UPDATE Speakers SET SessionExpires = @SessionExpires WHERE SessionToken = @SessionToken";
						Command.Parameters.Add( "SessionToken", System.Data.SqlDbType.UniqueIdentifier ).Value = SessionToken;
						Command.Parameters.Add( "SessionExpires", System.Data.SqlDbType.DateTime ).Value = DateTime.UtcNow.AddMinutes( 20 );
						await Command.ExecuteNonQueryAsync();
					}

					return Response;
				}
			}
				
			return await base.SendAsync( Request, CancelToken );
		}
	}
}
