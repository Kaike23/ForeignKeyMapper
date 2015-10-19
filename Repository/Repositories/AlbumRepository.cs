using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	using System.Data.SqlClient;
	using Model;
	using Repository.Repositories;

	public class AlbumRepository : Repository<Album>, IAlbumRepository
	{
		public AlbumRepository()
			: base(MapperRegistry.Albums)
		{ }

		public List<Album> FindBy(Guid artistId)
		{
			return Mapper.FindBy(artistId);
		}

		public new IAlbumMapper Mapper { get { return (IAlbumMapper)base.Mapper; } }
	}
}
