using System;

namespace Model
{
	public class Album : Entity
	{
		private string title;
		private Artist artist;

		public Album() { }
		public Album(Guid id, string title, Artist artist)
			: base(id)
		{
			this.title = title;
			this.artist = artist;
		}
		public string Title
		{
			get { return title; }
			set { title = value; }
		}
		public Artist Artist
		{
			get { return artist; }
			set { artist = value; }
		}
	}
}
