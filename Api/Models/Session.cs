using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DevSpace.Api.Models {
	[DataContract]
	[KnownType( typeof(Tag) )]
	[KnownType( typeof(Speaker) )]
	public class Session {
		#region Properties
		[DataMember]public short Id { get; set; }
		[DataMember]public Speaker Speaker { get; set; }
		[DataMember]public string Title { get; set; }
		[DataMember]public string Abstract { get; set; }
		[DataMember]public string Notes { get; set; }
		[DataMember]public string TimeSlot { get; set; }
		[DataMember]public List<Tag> Tags { get; set; }
		public bool Accepted { get; set; }
		#endregion

		#region Methods
		#region Static
		public async static Task<List<Session>> Select( SqlConnection Connection, SqlTransaction Transaction = null ) {
			List<Session> FoundSessions = new List<Session>();
			Dictionary<short, short> SpeakerIds = new Dictionary<short, short>();

			using( SqlCommand Command = new SqlCommand() ) {
				Command.Connection = Connection;
				if( null != Transaction ) Command.Transaction = Transaction;

				Command.CommandText = "SELECT Sessions.Id, SpeakerId, Title, Abstract, Notes, Accepted, Text AS TimeSlot FROM Sessions LEFT JOIN TimeSlots ON Sessions.TimeSlotId = TimeSlots.Id WHERE Accepted = 1 ORDER BY TimeSlotId, Title;";
				using( SqlDataReader DataReader = await Command.ExecuteReaderAsync() ) {
					while( DataReader.Read() ) {
						Session FoundSession = new Session();
						FoundSession.Id = DataReader.GetInt16( 0 );
						SpeakerIds.Add( FoundSession.Id, DataReader.GetInt16( 1 ) );
						FoundSession.Title = DataReader.GetString( 2 );
						FoundSession.Abstract = DataReader.GetString( 3 );
						FoundSession.Notes = DataReader.IsDBNull( 4 ) ? null : DataReader.GetString( 4 );
						FoundSession.Accepted = DataReader.IsDBNull( 5 ) ? false : DataReader.GetBoolean( 5 );
						FoundSession.TimeSlot = DataReader.IsDBNull( 6 ) ? null : DataReader.GetString( 6 );
						FoundSessions.Add( FoundSession );
					}
				}
			}

			foreach( Session FoundSession in FoundSessions ) { 
				FoundSession.Speaker = await Speaker.Select( SpeakerIds[FoundSession.Id], Connection );

				FoundSession.Tags = new List<Tag>();
				using( SqlCommand Command = new SqlCommand() ) {
					Command.Connection = Connection;
					if( null != Transaction ) Command.Transaction = Transaction;

					Command.CommandText = "SELECT Id, Text FROM Tags WHERE Id IN ( SELECT TagId FROM SessionTags WHERE SessionId = @SessionId );";
					Command.Parameters.Add( "SessionId", System.Data.SqlDbType.SmallInt ).Value = FoundSession.Id;
					using( SqlDataReader DataReader = await Command.ExecuteReaderAsync() ) {
						while( DataReader.Read() ) {
							FoundSession.Tags.Add( new Tag {
								Id = DataReader.GetInt16( 0 ),
								Text = DataReader.GetString( 1 )
							} );
						}
					}
				}
			}

			return FoundSessions;
		}

		public async static Task<Session> Select( int Id, SqlConnection Connection, SqlTransaction Transaction = null ) {
			short SpeakerId = -1;
			Session FoundSession = null;

			using( SqlCommand Command = new SqlCommand() ) {
				Command.Connection = Connection;
				if( null != Transaction )
					Command.Transaction = Transaction;

				Command.CommandText = "SELECT Sessions.Id, SpeakerId, Title, Abstract, Notes, Accepted, Text AS TimeSlot FROM Sessions LEFT JOIN TimeSlots ON Sessions.TimeSlotId = TimeSlots.Id WHERE Sessions.Id = @Id;";
				Command.Parameters.Add( "Id", System.Data.SqlDbType.SmallInt ).Value = Id;
				using( SqlDataReader DataReader = await Command.ExecuteReaderAsync() ) {
					if( DataReader.Read() ) {
						FoundSession = new Session();
						FoundSession.Id = DataReader.GetInt16( 0 );
						SpeakerId = DataReader.GetInt16( 1 );
						FoundSession.Title = DataReader.GetString( 2 );
						FoundSession.Abstract = DataReader.GetString( 3 );
						FoundSession.Notes = DataReader.IsDBNull( 4 ) ? null : DataReader.GetString( 4 );
						FoundSession.Accepted = DataReader.IsDBNull( 5 ) ? false : DataReader.GetBoolean( 5 );
						FoundSession.TimeSlot = DataReader.IsDBNull( 6 ) ? null : DataReader.GetString( 6 );
					}
				}
			}

			if( null != FoundSession ) {
				FoundSession.Speaker = await Speaker.Select( SpeakerId, Connection );

				FoundSession.Tags = new List<Tag>();
				using( SqlCommand Command = new SqlCommand() ) {
					Command.Connection = Connection;
					if( null != Transaction )
						Command.Transaction = Transaction;

					Command.CommandText = "SELECT Id, Text FROM Tags WHERE Id IN ( SELECT TagId FROM SessionTags WHERE SessionId = @SessionId );";
					Command.Parameters.Add( "SessionId", System.Data.SqlDbType.SmallInt ).Value = FoundSession.Id;
					using( SqlDataReader DataReader = await Command.ExecuteReaderAsync() ) {
						while( DataReader.Read() ) {
							FoundSession.Tags.Add( new Tag {
								Id = DataReader.GetInt16( 0 ),
								Text = DataReader.GetString( 1 )
							} );
						}
					}
				}
			}

			return FoundSession;
		}

		public async static Task<List<Session>> SelectBySpeaker( short Id, SqlConnection Connection, SqlTransaction Transaction = null ) {
			List<Session> FoundSessions = new List<Session>();
			Speaker TheSpeaker = await Speaker.Select( Id, Connection );

			using( SqlCommand Command = new SqlCommand() ) {
				Command.Connection = Connection;
				if( null != Transaction )
					Command.Transaction = Transaction;

				Command.CommandText = "SELECT Sessions.Id, SpeakerId, Title, Abstract, Notes, Accepted, Text AS TimeSlot FROM Sessions LEFT JOIN TimeSlots ON Sessions.TimeSlotId = TimeSlots.Id WHERE SpeakerId = @Id;";
				Command.Parameters.Add( "Id", System.Data.SqlDbType.SmallInt ).Value = Id;
				using( SqlDataReader DataReader = await Command.ExecuteReaderAsync() ) {
					while( DataReader.Read() ) {
						FoundSessions.Add( new Session {
							Id = DataReader.GetInt16( 0 ),
							Speaker = TheSpeaker,
							Title = DataReader.GetString( 2 ),
							Abstract = DataReader.GetString( 3 ),
							Notes = DataReader.IsDBNull( 4 ) ? null : DataReader.GetString( 4 ),
							Accepted = DataReader.IsDBNull( 5 ) ? false : DataReader.GetBoolean( 5 ),
							TimeSlot = DataReader.IsDBNull( 6 ) ? null : DataReader.GetString( 6 )
						} );
					}
				}
			}

			foreach( Session FoundSession in FoundSessions ) {
				FoundSession.Tags = new List<Tag>();
				using( SqlCommand Command = new SqlCommand() ) {
					Command.Connection = Connection;
					if( null != Transaction )
						Command.Transaction = Transaction;

					Command.CommandText = "SELECT Id, Text FROM Tags WHERE Id IN ( SELECT TagId FROM SessionTags WHERE SessionId = @SessionId );";
					Command.Parameters.Add( "SessionId", System.Data.SqlDbType.SmallInt ).Value = FoundSession.Id;
					using( SqlDataReader DataReader = await Command.ExecuteReaderAsync() ) {
						while( DataReader.Read() ) {
							FoundSession.Tags.Add( new Tag {
								Id = DataReader.GetInt16( 0 ),
								Text = DataReader.GetString( 1 )
							} );
						}
					}
				}
			}

			return FoundSessions;
		}

		public async static Task<List<Session>> SelectByTag( short TagId, SqlConnection Connection, SqlTransaction Transaction = null ) {
			List<Session> FoundSessions = new List<Session>();
			Dictionary<short, short> SpeakerIds = new Dictionary<short, short>();

			using( SqlCommand Command = new SqlCommand() ) {
				Command.Connection = Connection;
				if( null != Transaction )
					Command.Transaction = Transaction;

				Command.CommandText = "SELECT Sessions.Id, SpeakerId, Title, Abstract, Notes, Accepted, Text AS TimeSlot FROM Sessions LEFT JOIN TimeSlots ON Sessions.TimeSlotId = TimeSlots.Id WHERE Sessions.Id IN ( SELECT DISTINCT SessionId FROM SessionTags WHERE TagId = @TagId ) ORDER BY TimeSlotId, Title;";
				Command.Parameters.Add( "TagId", System.Data.SqlDbType.SmallInt ).Value = TagId;
				using( SqlDataReader DataReader = await Command.ExecuteReaderAsync() ) {
					while( DataReader.Read() ) {
						Session FoundSession = new Session {
							Id = DataReader.GetInt16( 0 ),
							Title = DataReader.GetString( 2 ),
							Abstract = DataReader.GetString( 3 ),
							Notes = DataReader.IsDBNull( 4 ) ? null : DataReader.GetString( 4 ),
							Accepted = DataReader.IsDBNull( 5 ) ? false : DataReader.GetBoolean( 5 ),
							TimeSlot = DataReader.IsDBNull( 6 ) ? null : DataReader.GetString( 6 )
						};

						FoundSessions.Add( FoundSession );

						SpeakerIds.Add( FoundSession.Id, DataReader.GetInt16( 1 ) );
					}
				}
			}

			foreach( Session FoundSession in FoundSessions ) {
				FoundSession.Speaker = await Speaker.Select( SpeakerIds[FoundSession.Id], Connection );

				FoundSession.Tags = new List<Tag>();
				using( SqlCommand Command = new SqlCommand() ) {
					Command.Connection = Connection;
					if( null != Transaction )
						Command.Transaction = Transaction;

					Command.CommandText = "SELECT Id, Text FROM Tags WHERE Id IN ( SELECT TagId FROM SessionTags WHERE SessionId = @SessionId );";
					Command.Parameters.Add( "SessionId", System.Data.SqlDbType.SmallInt ).Value = FoundSession.Id;
					using( SqlDataReader DataReader = await Command.ExecuteReaderAsync() ) {
						while( DataReader.Read() ) {
							FoundSession.Tags.Add( new Tag {
								Id = DataReader.GetInt16( 0 ),
								Text = DataReader.GetString( 1 )
							} );
						}
					}
				}
			}

			return FoundSessions;
		}

		public async static Task<bool> Delete( short Id, SqlConnection Connection, SqlTransaction Transaction = null ) {
			using( SqlCommand Command = new SqlCommand() ) {
				Command.Connection = Connection;
				if( null != Transaction )
					Command.Transaction = Transaction;

				Command.CommandText = "DELETE Sessions WHERE Id = @Id;";
				Command.Parameters.Add( "Id", System.Data.SqlDbType.SmallInt ).Value = Id;
				return await Command.ExecuteNonQueryAsync() > 0;
			}
		}
		#endregion
		#endregion
	}

	internal static class SessionExtensions {
		public static async Task Insert( this Session NewSession, SqlConnection Connection, SqlTransaction Transaction = null ) {
			using( SqlCommand Command = new SqlCommand() ) {
				Command.Connection = Connection;
				if( null != Transaction ) Command.Transaction = Transaction;

				Command.CommandText = "INSERT Sessions ( SpeakerId, Title, Abstract, Notes ) OUTPUT INSERTED.Id VALUES ( @SpeakerId, @Title, @Abstract, @Notes )";
				Command.Parameters.Add( "SpeakerId", System.Data.SqlDbType.SmallInt ).Value = NewSession.Speaker.Id;
				Command.Parameters.Add( "Title", System.Data.SqlDbType.VarChar ).Value = NewSession.Title;
				Command.Parameters.Add( "Abstract", System.Data.SqlDbType.VarChar ).Value = NewSession.Abstract;

				if( string.IsNullOrWhiteSpace( NewSession.Notes ) )
					Command.Parameters.Add( "Notes", System.Data.SqlDbType.VarChar ).Value = System.DBNull.Value;
				else
					Command.Parameters.Add( "Notes", System.Data.SqlDbType.VarChar ).Value = NewSession.Notes;

				NewSession.Id = (short)await Command.ExecuteScalarAsync();
			}

			foreach( Tag TagToLink in NewSession.Tags ) {
				using( SqlCommand Command = new SqlCommand() ) {
					Command.Connection = Connection;
					if( null != Transaction ) Command.Transaction = Transaction;

					Command.CommandText = "INSERT SessionTags ( SessionId, TagId ) VALUES ( @SessionId, @TagId );";
					Command.Parameters.Add( "SessionId", System.Data.SqlDbType.SmallInt ).Value = NewSession.Id;
					Command.Parameters.Add( "TagId", System.Data.SqlDbType.SmallInt ).Value = TagToLink.Id;
					await Command.ExecuteNonQueryAsync();
				}
			}
		}

		public static async Task Update( this Session SessionToUpdate, SqlConnection Connection, SqlTransaction Transaction = null ) {
			using( SqlCommand Command = new SqlCommand() ) {
				Command.Connection = Connection;
				if( null != Transaction ) Command.Transaction = Transaction;

				StringBuilder CommandText = new StringBuilder( "UPDATE Sessions SET " );

				Command.Parameters.Add( "SpeakerId", System.Data.SqlDbType.SmallInt ).Value = SessionToUpdate.Speaker.Id;
				CommandText.Append( "SpeakerId = @SpeakerId" );

				Command.Parameters.Add( "Title", System.Data.SqlDbType.VarChar ).Value = SessionToUpdate.Title;
				CommandText.Append( ", Title = @Title" );

				Command.Parameters.Add( "Abstract", System.Data.SqlDbType.VarChar ).Value = SessionToUpdate.Abstract;
				CommandText.Append( ", Abstract = @Abstract" );

				CommandText.Append( ", Notes = @Notes" );
				if( string.IsNullOrWhiteSpace( SessionToUpdate.Notes ) ) {
					Command.Parameters.Add( "Notes", System.Data.SqlDbType.VarChar ).Value = DBNull.Value;
				} else {
					Command.Parameters.Add( "Notes", System.Data.SqlDbType.VarChar ).Value = SessionToUpdate.Notes;
				}

				CommandText.Append( " WHERE Id = @Id;" );
				Command.Parameters.Add( "Id", System.Data.SqlDbType.SmallInt ).Value = SessionToUpdate.Id;

				Command.CommandText = CommandText.ToString();
				await Command.ExecuteNonQueryAsync();
			}

			using( SqlCommand Command = new SqlCommand() ) {
				Command.Connection = Connection;
				if( null != Transaction ) Command.Transaction = Transaction;

				Command.CommandText = "DELETE SessionTags WHERE SessionId = @SessionId";
				Command.Parameters.Add( "SessionId", System.Data.SqlDbType.SmallInt ).Value = SessionToUpdate.Id;
				await Command.ExecuteNonQueryAsync();
			}

			foreach( Tag TagToLink in SessionToUpdate.Tags ) {
				using( SqlCommand Command = new SqlCommand() ) {
					Command.Connection = Connection;
					if( null != Transaction ) Command.Transaction = Transaction;

					Command.CommandText = "INSERT SessionTags ( SessionId, TagId ) VALUES ( @SessionId, @TagId );";
					Command.Parameters.Add( "SessionId", System.Data.SqlDbType.SmallInt ).Value = SessionToUpdate.Id;
					Command.Parameters.Add( "TagId", System.Data.SqlDbType.SmallInt ).Value = TagToLink.Id;
					await Command.ExecuteNonQueryAsync();
				}
			}
		}
	}
}
