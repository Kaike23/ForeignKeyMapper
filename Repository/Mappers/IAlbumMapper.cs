using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	using Model;

	public interface IAlbumMapper : IMapper
	{
		List<Album> FindBy(Guid artistId);
	}
}
