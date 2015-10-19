using System;
using System.Collections.Generic;

namespace Model
{
	public class Artist : Entity
	{
		private string firstName;
		private string lastName;
		private List<Album> albums;

		public Artist() { }
		public Artist(Guid id, string firstName, string lastName, List<Album> albums)
			: base(id)
		{
			this.firstName = firstName;
			this.lastName = lastName;
			this.albums = albums;
		}

		public Artist(Guid id, string firstName, string lastName)
			: this(id, firstName, lastName, null)
		{ }

		public string FirstName
		{
			get { return firstName; }
			set { firstName = value; }
		}
		public string LastName
		{
			get { return lastName; }
			set { lastName = value; }
		}
		public List<Album> Albums
		{
			get { return albums; }
			set { albums = value; }
		}
		public void AddAlbum(Album album)
		{
			albums.Add(album);
		}
	}
}
