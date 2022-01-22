namespace TombIDE.Services.Generic;

public interface IRepository<TEntity, TPrimaryKey> where TEntity : class
{
	TEntity? Get(TPrimaryKey primaryKey);
	IEnumerable<TEntity> GetAll();

	TEntity? Find(Predicate<TEntity> predicate);
	IEnumerable<TEntity> FindAll(Predicate<TEntity> predicate);

	void Add(TEntity entity);
	void AddRange(IEnumerable<TEntity> entities);

	void Remove(TEntity entity);
	void RemoveRange(IEnumerable<TEntity> entities);
}
