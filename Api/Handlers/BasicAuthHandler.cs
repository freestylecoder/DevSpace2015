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
	class BasicAuthHandler : DelegatingHandler {
		private string UserEmail { get; set; }
		private byte[] SHA512Password { get; set; }

		private void GetCredentials( string EncodedBasicAuth ) {
			byte[] HeaderBytes = Convert.FromBase64String( EncodedBasicAuth );

			string RawBasicAuth = Encoding.UTF8.GetString( HeaderBytes );
			UserEmail = RawBasicAuth.Substring( 0, RawBasicAuth.IndexOf( ':' ) );

			// The RawBasicAuth has the password in clear text
			// Thus, we want to get rid of it as soon as possible.
			RawBasicAuth = null;

			// This has the effect of using the email as a salt for the password.
			SHA512Password = System.Security.Cryptography.SHA512Managed.Create().ComputeHash( HeaderBytes );

			// The HeaderBytes also has the raw password.
			// Thus, clear it out, now that we're done with it
			for( int index = 0; index < HeaderBytes.Length; ++index )
				HeaderBytes[index] = 0;
		}

		protected async override Task<HttpResponseMessage> SendAsync( HttpRequestMessage Request, CancellationToken CancelToken ) {
			if( null != Request.Headers.Authorization ) {
				// IF we have a "Basic" Authorization header, we need to login
				// ELSE we need to let the SessionToken cookie have a wack at it, so we do nothing
				if( Request.Headers.Authorization.Scheme.ToUpper() == "BASIC" ) {
					GetCredentials( Request.Headers.Authorization.Parameter );

					// Do Basic Auth
					using( SqlConnection Connection = new SqlConnection( RoleEnvironment.GetConfigurationSettingValue( "DatabaseConnectionString" ) ) ) {
						Connection.Open();

						using( SqlCommand Command = new SqlCommand() ) {
							Command.Connection = Connection;
							Command.CommandText = "SELECT EmailAddress FROM Speakers WHERE PasswordHash = @PasswordHash";
							Command.Parameters.Add( "PasswordHash", System.Data.SqlDbType.VarBinary ).Value = SHA512Password;

							using( SqlDataReader DataReader = await Command.ExecuteReaderAsync() ) {
								// IF Basic Auth Succeeds
								if( DataReader.Read() && UserEmail.ToUpper().Equals( DataReader.GetString( 0 ).ToUpper() ) ) {
									GenericIdentity UserIdentity = new GenericIdentity( UserEmail );
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
