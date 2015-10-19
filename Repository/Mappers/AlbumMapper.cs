using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Repository
{
	public class AlbumMapper : Mapper<Album>, IAlbumMapper
	{
		public AlbumMapper() { }

		protected override string FindStatement
		{
			get { return "SELECT Id, Title, ArtistId FROM Albums WHERE Id = @Id"; }
		}

		protected override Album DoLoad(Guid id, SqlDataReader reader)
		{
			var title = reader.GetString(1);
			var artistId = reader.GetGuid(2);
			var artist = MapperRegistry.Artists.Find(artistId) as Artist;
			var album = new Album(id, title, artist);
			return album;
		}

		protected override string UpdateStatement
		{
			get { return "UPDATE Albums SET Title = @Title, ArtistID = @ArtistID WHERE Id = @Id"; }
		}

		protected override void DoUpdate(Album entity, SqlCommand updateCommand)
		{
			updateCommand.Parameters.AddWithValue("@Title", entity.Title);
			updateCommand.Parameters.AddWithValue("@ArtistID", entity.Artist.Id);
		}

		#region Alternative implementation

		protected override string FindMultitableStatement
		{
			get
			{
				return "SELECT A.Id, A.Title, A.ArtistId, R.FirstName, R.LastName " +
						"FROM Albums A, Artists R " +
						"WHERE A.Id = @Id and A.ArtistId = R.Id";
			}
		}

		protected override Album DoMultitableLoad(Guid id, SqlDataReader reader)
		{
			var title = reader.GetString(1);
			var artistId = reader.GetGuid(2);
			var artistMapper = MapperRegistry.Artists;
			var artist = artistMapper.IsLoaded(artistId) ? artistMapper.Find(artistId) as Artist : LoadArtist(artistId, reader);
			var result = new Album(id, title, artist);
			return result;
		}

		private Artist LoadArtist(Guid id, SqlDataReader reader)
		{
			var firstName = reader.GetString(3);
			var lastName = reader.GetString(4);
			var result = new Artist(id, firstName, lastName);
			MapperRegistry.Artists.DoRegister(result.Id, result);
			return result;
		}

		#endregion

		public List<Album> FindBy(Guid artistId)
		{
			var albums = new List<Album>();
			var query = "SELECT Id, Title, ArtistId FROM Albums WHERE ArtistId = @ArtistId";
			var sqlConnection = new SqlConnection(SQL_CONNECTION);
			sqlConnection.Open();
			try
			{
				using (var sqlCommand = new SqlCommand(query, sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("@ArtistId", artistId);
					var reader = sqlCommand.ExecuteReader();
					Album album;
					while ((album = CollectionReferenceLoad(reader) as Album) != null)
					{
						albums.Add(album);
					}
					reader.Close();
				}
			}
			catch (SqlException e)
			{
				throw new ApplicationException(e.Message, e);
			}
			finally
			{
				sqlConnection.Close();
			}
			return albums;
		}

		protected override void DoCollectionReferenceLoad(Album entity, SqlDataReader reader)
		{
			entity.Title = reader.GetString(1);
			var artistId = reader.GetGuid(2);
			entity.Artist = MapperRegistry.Artists.CollectionReferenceFind(artistId) as Artist;
		}
	}
}
