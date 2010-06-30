using System;
using System.Threading;

namespace Utility.Utilities
{
	public static class ThreadUtility
	{
		public static T Try<T>(Func<T> get, double timeout)
		{
			T result = default(T);

			Thread thread = new Thread(delegate() { result = get(); });

			thread.IsBackground = true;
			thread.Start();
			thread.Join((int)(timeout * 1000));

			return result;
		}
	}
}
