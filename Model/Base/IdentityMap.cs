using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Model
{
	public class IdentityMap
	{
		private Dictionary<Guid, Entity> entities;

		public IdentityMap()
		{
			this.entities = new Dictionary<Guid, Entity>();
		}

		public Entity Get(Guid key)
		{
			Entity entity = null;
			this.entities.TryGetValue(key, out entity);
			return entity;
		}

		public void Add(Guid key, Entity value)
		{
			Debug.Assert(!this.entities.ContainsKey(key));
			this.entities.Add(key, value);
		}

		public void Remove(Guid key)
		{
			this.entities.Remove(key);
		}

		public void Clear()
		{
			this.entities.Clear();
		}

		public bool Contains(Guid key)
		{
			return this.entities.ContainsKey(key);
		}
	}
}
