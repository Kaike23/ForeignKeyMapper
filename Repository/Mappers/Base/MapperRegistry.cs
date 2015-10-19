using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	public static class MapperRegistry
	{
		static MapperRegistry()
		{
			Albums = new AlbumMapper();
			Artists = new ArtistMapper();
		}

		public static AlbumMapper Albums { get; set; }
		public static ArtistMapper Artists { get; set; }
	}
}
