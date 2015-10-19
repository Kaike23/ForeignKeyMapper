using System;

namespace Model
{
	public class Entity
	{
		private Guid id;
		public Entity() { }
		public Entity(Guid id)
		{
			this.id = id;
		}
		public Guid Id
		{
			get { return id; }
			set { id = value; }
		}
	}
}
