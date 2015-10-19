using System;

namespace Repository
{
	using Model;
	using Repository.Repositories;

	public class ArtistRepository : Repository<Artist>, IArtistRepository
	{
		public ArtistRepository()
			: base(MapperRegistry.Artists)
		{ }
	}
}
