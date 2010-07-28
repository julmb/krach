using System;

namespace Krach
{
	public class EventArgs<T> : EventArgs
	{
		readonly T parameter;

		public T Parameter { get { return parameter; } }

		public EventArgs(T parameter)
		{
			this.parameter = parameter;
		}
	}
}
