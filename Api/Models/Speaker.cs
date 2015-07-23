using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DevSpace.Api.Models {
	[DataContract]public class Speaker {
		#region Properties
		#region Internal
		public byte[] PasswordHash { get; set; }
		public Guid SessionToken { get; set; }
		public DateTime SessionExpires { get; set; }
		#endregion

		[DataMember]public short Id { get; set; }
		[DataMember]public string EmailAddress { get; set; }
		[DataMember]public string DisplayName { get; set; }
		[DataMember]public string Password { get; set; }
		[DataMember]public string Bio { get; set; }
		[DataMember]public string Twitter { get; set; }
		[DataMember]public string Website { get; set; }
		[DataMember]public bool DisplayEmail { get; set; }
		[DataMember]public bool DisplayTwitter { get; set; }
		[DataMember]public bool DisplayWebsite { get; set; }
		#endregion

		#region Methods
		#region Static
		public async static Task<Speaker> Select( int Id, SqlConnection Connection, SqlTransaction Transaction = null ) {
			Speaker FoundSpeaker = null;

			using( SqlCommand Command = new SqlCommand() ) {
				Command.Connection = Connection;
				if( null != Transaction )
					Command.Transaction = Transaction;

				Command.CommandText = "SELECT Id, EmailAddress, DisplayName, PasswordHash, Bio, Twitter, Website, DisplayEmail, DisplayTwitter, DisplayWebsite, SessionToken, SessionExpires FROM Speakers WHERE Id = @Id;";
				Command.Parameters.Add( "Id", System.Data.SqlDbType.SmallInt ).Value = Id;
				using( SqlDataReader DataReader = await Command.ExecuteReaderAsync() ) {
					if( DataReader.Read() ) {
						FoundSpeaker = new Speaker();
						FoundSpeaker.Id = DataReader.GetInt16( 0 );
						FoundSpeaker.EmailAddress = DataReader.GetString( 1 );
						FoundSpeaker.DisplayName = DataReader.GetString( 2 );
						FoundSpeaker.PasswordHash = DataReader.GetSqlBinary( 3 ).Value;
						FoundSpeaker.Bio = DataReader.IsDBNull( 4 ) ? null : DataReader.GetString( 4 );
						FoundSpeaker.Twitter = DataReader.IsDBNull( 5 ) ? null : DataReader.GetString( 5 );
						FoundSpeaker.Website = DataReader.IsDBNull( 6 ) ? null : DataReader.GetString( 6 );
						FoundSpeaker.DisplayEmail = DataReader.IsDBNull( 7 ) ? false : DataReader.GetBoolean( 7 );
						FoundSpeaker.DisplayTwitter = DataReader.IsDBNull( 8 ) ? false : DataReader.GetBoolean( 8 );
						FoundSpeaker.DisplayWebsite = DataReader.IsDBNull( 9 ) ? false : DataReader.GetBoolean( 9 );
						FoundSpeaker.SessionToken = DataReader.IsDBNull( 10 ) ? Guid.Empty : DataReader.GetGuid( 10 );
						FoundSpeaker.SessionExpires = DataReader.IsDBNull( 11 ) ? DateTime.MinValue : DataReader.GetDateTime( 11 );
					}
				}
			}

			return FoundSpeaker;
		}

		public async static Task<Speaker> Select( string EmailAddress, SqlConnection Connection, SqlTransaction Transaction = null ) {
			Speaker FoundSpeaker = null;

			using( SqlCommand Command = new SqlCommand() ) {
				Command.Connection = Connection;
				if( null != Transaction )
					Command.Transaction = Transaction;

				Command.CommandText = "SELECT Id, EmailAddress, DisplayName, PasswordHash, Bio, Twitter, Website, DisplayEmail, DisplayTwitter, DisplayWebsite, SessionToken, SessionExpires FROM Speakers WHERE EmailAddress = @EmailAddress;";
				Command.Parameters.Add( "EmailAddress", System.Data.SqlDbType.VarChar ).Value = EmailAddress;
				using( SqlDataReader DataReader = await Command.ExecuteReaderAsync() ) {
					if( DataReader.Read() ) {
						FoundSpeaker = new Speaker();
						FoundSpeaker.Id = DataReader.GetInt16( 0 );
						FoundSpeaker.EmailAddress = DataReader.GetString( 1 );
						FoundSpeaker.DisplayName = DataReader.GetString( 2 );
						FoundSpeaker.PasswordHash = DataReader.GetSqlBinary( 3 ).Value;
						FoundSpeaker.Bio = DataReader.IsDBNull( 4 ) ? null : DataReader.GetString( 4 );
						FoundSpeaker.Twitter = DataReader.IsDBNull( 5 ) ? null : DataReader.GetString( 5 );
						FoundSpeaker.Website = DataReader.IsDBNull( 6 ) ? null : DataReader.GetString( 6 );
						FoundSpeaker.DisplayEmail = DataReader.IsDBNull( 7 ) ? false : DataReader.GetBoolean( 7 );
						FoundSpeaker.DisplayTwitter = DataReader.IsDBNull( 8 ) ? false : DataReader.GetBoolean( 8 );
						FoundSpeaker.DisplayWebsite = DataReader.IsDBNull( 9 ) ? false : DataReader.GetBoolean( 9 );
						FoundSpeaker.SessionToken = DataReader.IsDBNull( 10 ) ? Guid.Empty : DataReader.GetGuid( 10 );
						FoundSpeaker.SessionExpires = DataReader.IsDBNull( 11 ) ? DateTime.MinValue : DataReader.GetDateTime( 11 );
					}
				}
			}

			return FoundSpeaker;
		}
		#endregion
		#endregion
	}

	internal static class SpeakerExtensions {
		public static async Task Insert( this Speaker NewSpeaker, SqlConnection Connection, SqlTransaction Transaction = null ) {
			using( SqlCommand Command = new SqlCommand() ) {
				Command.Connection = Connection;
				if( null != Transaction ) Command.Transaction = Transaction;

				Command.CommandText = "INSERT Speakers ( EmailAddress, DisplayName, PasswordHash ) OUTPUT INSERTED.Id VALUES ( @EmailAddress, @DisplayName, @PasswordHash )";
				Command.Parameters.Add( "EmailAddress", System.Data.SqlDbType.VarChar ).Value = NewSpeaker.EmailAddress;
				Command.Parameters.Add( "DisplayName", System.Data.SqlDbType.VarChar ).Value = NewSpeaker.DisplayName;
				Command.Parameters.Add( "PasswordHash", System.Data.SqlDbType.VarBinary ).Value = NewSpeaker.PasswordHash;

				NewSpeaker.Id = (short)await Command.ExecuteScalarAsync();
			}
		}

		public static async Task Update( this Speaker NewSpeaker, SqlConnection Connection, SqlTransaction Transaction = null ) {
			using( SqlCommand Command = new SqlCommand() ) {
				Command.Connection = Connection;
				if( null != Transaction ) Command.Transaction = Transaction;

				StringBuilder CommandText = new StringBuilder( "UPDATE Speakers SET " );

				Command.Parameters.Add( "EmailAddress", System.Data.SqlDbType.VarChar ).Value = NewSpeaker.EmailAddress;
				CommandText.Append( "EmailAddress = @EmailAddress" );

				Command.Parameters.Add( "DisplayName", System.Data.SqlDbType.VarChar ).Value = NewSpeaker.DisplayName;
				CommandText.Append( ", DisplayName = @DisplayName" );

				if( !string.IsNullOrWhiteSpace( NewSpeaker.Password ) ) {
					NewSpeaker.PasswordHash = System.Security.Cryptography.SHA512Managed.Create().ComputeHash( Encoding.UTF8.GetBytes( NewSpeaker.EmailAddress + ":" + NewSpeaker.Password ) );
					Command.Parameters.Add( "PasswordHash", System.Data.SqlDbType.VarBinary ).Value = NewSpeaker.PasswordHash;
					CommandText.Append( ", PasswordHash = @PasswordHash" );
				}

				CommandText.Append( ", Bio = @Bio" );
				if( string.IsNullOrWhiteSpace( NewSpeaker.Bio ) ) {
					Command.Parameters.Add( "Bio", System.Data.SqlDbType.VarChar ).Value = DBNull.Value;
				} else {
					Command.Parameters.Add( "Bio", System.Data.SqlDbType.VarChar ).Value = NewSpeaker.Bio;
				}

				CommandText.Append( ", Twitter = @Twitter" );
				if( string.IsNullOrWhiteSpace( NewSpeaker.Twitter ) ) {
					Command.Parameters.Add( "Twitter", System.Data.SqlDbType.VarChar ).Value = DBNull.Value;
				} else {
					Command.Parameters.Add( "Twitter", System.Data.SqlDbType.VarChar ).Value = NewSpeaker.Twitter;
				}

				CommandText.Append( ", Website = @Website" );
				if( string.IsNullOrWhiteSpace( NewSpeaker.Website ) ) {
					Command.Parameters.Add( "Website", System.Data.SqlDbType.VarChar ).Value = DBNull.Value;
				} else {
					Command.Parameters.Add( "Website", System.Data.SqlDbType.VarChar ).Value = NewSpeaker.Website;
				}

				CommandText.Append( ", DisplayEmail = @DisplayEmail" );
				Command.Parameters.Add( "DisplayEmail", System.Data.SqlDbType.Bit ).Value = NewSpeaker.DisplayEmail;

				CommandText.Append( ", DisplayTwitter = @DisplayTwitter" );
				Command.Parameters.Add( "DisplayTwitter", System.Data.SqlDbType.Bit ).Value = NewSpeaker.DisplayTwitter;

				CommandText.Append( ", DisplayWebsite = @DisplayWebsite" );
				Command.Parameters.Add( "DisplayWebsite", System.Data.SqlDbType.Bit ).Value = NewSpeaker.DisplayWebsite;

				CommandText.Append( " WHERE Id = @Id;" );
				Command.Parameters.Add( "Id", System.Data.SqlDbType.SmallInt ).Value = NewSpeaker.Id;

				Command.CommandText = CommandText.ToString();
				await Command.ExecuteNonQueryAsync();
			}
		}
	}
}
