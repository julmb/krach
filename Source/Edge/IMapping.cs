namespace Utilities
{
	public interface IMapping<T, U>
	{
		U this[T item] { get; }
		T this[U item] { get; }
	}
}