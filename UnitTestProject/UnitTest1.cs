using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository;

namespace UnitTestProject
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void SingleValuedReferenceTest()
		{
			// 'Find' function does two DB trips to get all info needed to return an album object.
			var albumRepo = new AlbumRepository();
			Guid uniqueId;
			Guid.TryParse("5e26185b-3de9-478b-b764-48d50a8944b0", out uniqueId);
			var album = albumRepo.Find(uniqueId);

			Debug.Assert(album.Id != null);
		}

		[TestMethod]
		public void MultitableFindTest()
		{
			// 'FindMultiple' function uses a query with INNER JOIN to avoid a second trip to the DB
			var albumRepo = new AlbumRepository();
			Guid uniqueId;
			Guid.TryParse("02545961-2a64-4fdd-8c4d-7cc8bd5c6857", out uniqueId);
			var album = albumRepo.FindMultitable(uniqueId);

			Debug.Assert(album.Id != null);
		}

		[TestMethod]
		public void CollectionReferenceTest()
		{
			// 'CollectionReferenceTest' function loads a collection referenced by an entity. Even when the entities in the collection
			// references the initial entity, the mapping implementation breaks the posibble infinite loop in which the loading process
			// could fall.
			var albumRepo = new AlbumRepository();
			Guid uniqueId;
			Guid.TryParse("d10826ce-5dc2-44e3-8ff7-b3d72bf4a2c0", out uniqueId);
			var albums = albumRepo.FindBy(uniqueId);

			Debug.Assert(albums.Count > 0);
		}
	}
}
