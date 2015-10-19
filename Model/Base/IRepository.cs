using System;

namespace Model
{
	public interface IRepository<T>
		where T : Entity
	{
		T Find(Guid id);

		Guid Insert(T entity);
		void Update(T entity);
		void Delete(T entity);


		T FindMultitable(Guid id);
	}
}
