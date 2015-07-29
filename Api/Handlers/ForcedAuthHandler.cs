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
	class ForcedAuthHandler : DelegatingHandler {
		protected async override Task<HttpResponseMessage> SendAsync( HttpRequestMessage Request, CancellationToken CancelToken ) {
			if( null != Request.Headers.Authorization ) {
				// IF we have a "Basic" Authorization header, we need to login
				// ELSE we need to let the SessionToken cookie have a wack at it, so we do nothing
				if( Request.Headers.Authorization.Scheme.ToUpper() == "FORCE" ) {
					Guid Token;
					if( !Guid.TryParse( Request.Headers.Authorization.Parameter, out Token ) ) {
						HttpResponseMessage Response401 = new HttpResponseMessage( System.Net.HttpStatusCode.Unauthorized );
						Response401.Headers.Add( "Access-Control-Allow-Origin", Request.Headers.GetValues( "Origin" ).First() );
						Response401.Headers.Add( "Access-Control-Allow-Credentials", "true" );
						return Response401;
					}

					// Do Basic Auth
					using( SqlConnection Connection = new SqlConnection( RoleEnvironment.GetConfigurationSettingValue( "DatabaseConnectionString" ) ) ) {
						Connection.Open();

						using( SqlCommand Command = new SqlCommand() ) {
							Command.Connection = Connection;
							Command.CommandText = "DELETE Tokens WHERE Expires < @CurrentTime;";
							Command.Parameters.Add( "CurrentTime", System.Data.SqlDbType.DateTime ).Value = DateTime.UtcNow;
							await Command.ExecuteNonQueryAsync();
						}
						
						using( SqlCommand Command = new SqlCommand() ) {
							Command.Connection = Connection;
							Command.CommandText = "SELECT EmailAddress FROM Speakers WHERE Id IN ( SELECT Id FROM Tokens WHERE Token = @Token )";
							Command.Parameters.Add( "Token", System.Data.SqlDbType.UniqueIdentifier ).Value = Token;

							using( SqlDataReader DataReader = await Command.ExecuteReaderAsync() ) {
								// IF Basic Auth Succeeds
								if( DataReader.Read() ) {
									GenericIdentity UserIdentity = new GenericIdentity( DataReader.GetString( 0 ).ToUpper() );
									GenericPrincipal UserPrincipal = new GenericPrincipal( UserIdentity, null );

									// Set Principle
									Thread.CurrentPrincipal = UserPrincipal;
									Request.GetRequestContext().Principal = UserPrincipal;
								}
								// ELSE Basic Auth Fails
								else {
									// return 401
									//Access-Control-Allow-Origin: https://www.devspaceconf.com
									//Access-Control-Allow-Credentials: true

									HttpResponseMessage Response401 = new HttpResponseMessage( System.Net.HttpStatusCode.Unauthorized );
									Response401.Headers.Add( "Access-Control-Allow-Origin", Request.Headers.GetValues( "Origin" ).First() );
									Response401.Headers.Add( "Access-Control-Allow-Credentials", "true" );
									return Response401;
								}
							}
						}

						using( SqlCommand Command = new SqlCommand() ) {
							Command.Connection = Connection;
							Command.CommandText = "DELETE Tokens WHERE Token < @Token;";
							Command.Parameters.Add( "Token", System.Data.SqlDbType.UniqueIdentifier ).Value = Token;
							await Command.ExecuteNonQueryAsync();
						}

						// Complete the request. We'll set the login cookie on the way out
						HttpResponseMessage Response = await base.SendAsync( Request, CancelToken );

						Guid SessionToken = Guid.NewGuid();
						using( SqlCommand Command = new SqlCommand() ) {
							Command.Connection = Connection;
							Command.CommandText = "UPDATE Speakers SET SessionToken = @SessionToken, SessionExpires = @SessionExpires WHERE EmailAddress = @EmailAddress";
							Command.Parameters.Add( "SessionToken", System.Data.SqlDbType.UniqueIdentifier ).Value = SessionToken;
							Command.Parameters.Add( "SessionExpires", System.Data.SqlDbType.DateTime ).Value = DateTime.UtcNow.AddMinutes( 20 );
							Command.Parameters.Add( "EmailAddress", System.Data.SqlDbType.VarChar ).Value = Thread.CurrentPrincipal.Identity.Name;
							await Command.ExecuteNonQueryAsync();
						}

						CookieHeaderValue SessionCookie = new CookieHeaderValue( "SessionToken", SessionToken.ToString() );
						#if DEBUG == false
						SessionCookie.Secure = true;
						#endif
						SessionCookie.HttpOnly = true;
						Response.Headers.AddCookies( new CookieHeaderValue[] { SessionCookie } );

						return Response;
					}
				}
			}

			return await base.SendAsync( Request, CancelToken );
		}
	}
}
