using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Repository
{
	public abstract class Mapper<T> : IMapper
		where T : Entity, new()
	{
		protected static readonly string SQL_CONNECTION = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=D:\GitHub\ForeignKeyMapper\Repository\TestData\TestDB.mdf;Integrated Security=True;Connect Timeout=30";

		private static IdentityMap loadedMap = new IdentityMap();

		#region IMapper implementation

		public Entity Find(Guid id)
		{
			var result = loadedMap.Get(id);
			if (result != null) return result;

			var sqlConnection = new SqlConnection(SQL_CONNECTION);
			sqlConnection.Open();
			try
			{
				using (var sqlCommand = new SqlCommand(FindStatement, sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("@Id", id);
					var reader = sqlCommand.ExecuteReader();
					result = Load(reader);
					reader.Close();
				}
				return result;
			}
			catch (SqlException e)
			{
				throw new ApplicationException(e.Message, e);
			}
			finally
			{
				sqlConnection.Close();
			}
		}

		public Guid Insert(Entity entity)
		{
			throw new NotImplementedException();
		}
		public void Update(Entity entity)
		{
			var connection = new SqlConnection(SQL_CONNECTION);
			connection.Open();
			using (var trans = connection.BeginTransaction())
			{
				try
				{
					var updateCommand = new SqlCommand(UpdateStatement, connection, trans);
					updateCommand.Parameters.AddWithValue("@Id", entity.Id);
					DoUpdate(entity as T, updateCommand);
					var rowCount = updateCommand.ExecuteNonQuery();
				}
				catch (SqlException e)
				{
					throw new ApplicationException(e.Message, e);
				}
				finally
				{
					connection.Close();
				}
			}
		}
		public void Delete(Entity entity)
		{
			throw new NotImplementedException();
		}

		#region Alternative implementation

		public Entity FindMultitable(Guid id)
		{
			var result = loadedMap.Get(id);
			if (result != null) return result;

			var sqlConnection = new SqlConnection(SQL_CONNECTION);
			sqlConnection.Open();
			try
			{
				using (var sqlCommand = new SqlCommand(FindMultitableStatement, sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("@Id", id);
					var reader = sqlCommand.ExecuteReader();
					result = MultitableLoad(reader);
					reader.Close();
				}
				return result;
			}
			catch (SqlException e)
			{
				throw new ApplicationException(e.Message, e);
			}
			finally
			{
				sqlConnection.Close();
			}
		}

		#endregion

		#endregion

		public bool IsLoaded(Guid id)
		{
			return loadedMap.Contains(id);
		}

		protected Entity Load(SqlDataReader reader)
		{
			if (!reader.HasRows || !reader.Read()) return default(T);
			var id = reader.GetGuid(0);
			if (loadedMap.Contains(id)) return loadedMap.Get(id);
			var result = DoLoad(id, reader);
			DoRegister(id, result);
			return result;
		}

		public void DoRegister(Guid id, Entity result)
		{
			loadedMap.Add(id, result);
		}

		protected abstract string FindStatement { get; }
		protected abstract string UpdateStatement { get; }
		protected abstract T DoLoad(Guid id, SqlDataReader reader);
		protected abstract void DoUpdate(T entity, SqlCommand updateCommand);

		#region Alternative implementation

		protected Entity MultitableLoad(SqlDataReader reader)
		{
			if (!reader.HasRows) return default(T);
			reader.Read();
			var id = reader.GetGuid(0);
			if (loadedMap.Contains(id)) return loadedMap.Get(id);
			var result = DoMultitableLoad(id, reader);
			DoRegister(id, result);
			return result;
		}

		protected abstract string FindMultitableStatement { get; }
		protected abstract T DoMultitableLoad(Guid id, SqlDataReader reader);

		#endregion

		#region CollectionReference

		public Entity CollectionReferenceFind(Guid id)
		{
			var result = loadedMap.Get(id);
			if (result != null) return result;

			var sqlConnection = new SqlConnection(SQL_CONNECTION);
			sqlConnection.Open();
			try
			{
				using (var sqlCommand = new SqlCommand(FindStatement, sqlConnection))
				{
					sqlCommand.Parameters.AddWithValue("@Id", id);
					var reader = sqlCommand.ExecuteReader();
					result = CollectionReferenceLoad(reader);
					reader.Close();
				}
				return result;
			}
			catch (SqlException e)
			{
				throw new ApplicationException(e.Message, e);
			}
			finally
			{
				sqlConnection.Close();
			}
		}

		protected Entity CollectionReferenceLoad(SqlDataReader reader)
		{
			if (!reader.HasRows || !reader.Read()) return default(T);
			var id = reader.GetGuid(0);
			if (loadedMap.Contains(id)) return loadedMap.Get(id);
			var entity = new T();
			entity.Id = id;
			DoRegister(id, entity);
			DoCollectionReferenceLoad(entity, reader);
			return entity;
		}

		protected abstract void DoCollectionReferenceLoad(T entity, SqlDataReader reader);
		#endregion
	}
}
