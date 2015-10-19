using System;
using System.Collections.Generic;

namespace Model
{
	public interface IAlbumRepository : IRepository<Album>
	{
		List<Album> FindBy(Guid artistId);
	}
}
