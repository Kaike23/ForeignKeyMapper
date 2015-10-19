using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
	using System.Data.SqlClient;
	using Model;

	public interface IMapper
	{
		Entity Find(Guid id);

		Guid Insert(Entity entity);
		void Update(Entity entity);
		void Delete(Entity entity);


		Entity FindMultitable(Guid id);
	}
}
