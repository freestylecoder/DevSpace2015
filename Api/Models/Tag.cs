using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DevSpace.Api.Models {
	[DataContract]public class Tag {
		#region Properties
		[DataMember]public short Id { get; set; }
		[DataMember]public string Text { get; set; }
		#endregion

		#region Methods
		#region Static
		public async static Task<List<Tag>> Select( SqlConnection Connection, SqlTransaction Transaction = null ) {
			List<Tag> TagList = new List<Tag>();

			using( SqlCommand Command = new SqlCommand() ) {
				Command.Connection = Connection;
				if( null != Transaction ) Command.Transaction = Transaction;

				Command.CommandText = "SELECT Id, Text FROM Tags ORDER BY Text;";
				using( SqlDataReader DataReader = await Command.ExecuteReaderAsync() ) {
					while( DataReader.Read() ) {
						TagList.Add( new Tag {
							Id = DataReader.GetInt16( 0 ),
							Text = DataReader.GetString( 1 )
						} );
					}
				}
			}

			return TagList;
		}
		#endregion
		#endregion
	}

	internal static class TagExtensions {
		public static async Task Insert( this Tag NewTag, SqlConnection Connection, SqlTransaction Transaction = null ) {
			using( SqlCommand Command = new SqlCommand() ) {
				Command.Connection = Connection;
				if( null != Transaction ) Command.Transaction = Transaction;

				Command.CommandText = "INSERT Tags ( Text ) OUTPUT INSERTED.Id VALUES ( @Text )";
				Command.Parameters.Add( "Text", System.Data.SqlDbType.VarChar ).Value = NewTag.Text;

				NewTag.Id = (short)await Command.ExecuteScalarAsync();
			}
		}

		public static async Task Update( this Tag TagToUpdate, SqlConnection Connection, SqlTransaction Transaction = null ) {
			using( SqlCommand Command = new SqlCommand() ) {
				Command.Connection = Connection;
				if( null != Transaction ) Command.Transaction = Transaction;

				Command.CommandText = "UPDATE Tags SET Text = @Text WHERE Id = @Id;";
				Command.Parameters.Add( "Text", System.Data.SqlDbType.VarChar ).Value = TagToUpdate.Text;
				Command.Parameters.Add( "Id", System.Data.SqlDbType.SmallInt ).Value = TagToUpdate.Id;

				await Command.ExecuteNonQueryAsync();
			}
		}
	}
}
