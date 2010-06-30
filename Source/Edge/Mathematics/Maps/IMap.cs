namespace Edge.Mathematics.Maps
{
	public interface IMap<TSource, TDestination>
	{
		TDestination ForwardMap(TSource value);
		TSource ReverseMap(TDestination value);
	}
}