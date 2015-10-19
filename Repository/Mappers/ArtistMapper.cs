using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;

namespace Repository
{
	public class ArtistMapper : Mapper<Artist>
	{
		protected override string FindStatement
		{
			get { return "SELECT Id, FirstName, LastName FROM Artists WHERE Id = @Id"; }
		}

		protected override string UpdateStatement
		{
			get { return "UPDATE Artists SET FirstName = @FirstName, LastName = @LastName WHERE Id = @Id"; }
		}

		protected override Artist DoLoad(Guid id, System.Data.SqlClient.SqlDataReader reader)
		{
			var firstName = reader.GetString(1);
			var lastName = reader.GetString(2);
			var artist = new Artist(id, firstName, lastName, null);
			return artist;
		}

		protected override void DoUpdate(Artist entity, System.Data.SqlClient.SqlCommand updateCommand)
		{
			updateCommand.Parameters.AddWithValue("@FirstName", entity.FirstName);
			updateCommand.Parameters.AddWithValue("@LastName", entity.LastName);
		}

		#region Alternative implementation

		protected override string FindMultitableStatement
		{
			get { return string.Empty; }
		}

		protected override Artist DoMultitableLoad(Guid id, System.Data.SqlClient.SqlDataReader reader)
		{
			return null;
		}

		#endregion

		protected override void DoCollectionReferenceLoad(Artist entity, System.Data.SqlClient.SqlDataReader reader)
		{
			entity.FirstName = reader.GetString(1);
			entity.LastName = reader.GetString(2);
			entity.Albums = MapperRegistry.Albums.FindBy(entity.Id);
		}
	}
}
