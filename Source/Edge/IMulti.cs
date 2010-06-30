namespace Utilities
{
	public interface IMulti<T1, T2>
	{
		T1 Data1 { get; }
		T2 Data2 { get; }
	}
	public interface IMulti<T1, T2, T3>
	{
		T1 Data1 { get; }
		T2 Data2 { get; }
		T3 Data3 { get; }
	}
	public interface IMulti<T1, T2, T3, T4>
	{
		T1 Data1 { get; }
		T2 Data2 { get; }
		T3 Data3 { get; }
		T4 Data4 { get; }
	}
	public interface IMulti<T1, T2, T3, T4, T5>
	{
		T1 Data1 { get; }
		T2 Data2 { get; }
		T3 Data3 { get; }
		T4 Data4 { get; }
		T5 Data5 { get; }
	}
}
