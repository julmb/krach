using System;
using System.Threading;

namespace Utility
{
	public class Breaker<T> : IDisposable
	{
		readonly Func<T> readItem;
		readonly Thread reader;
		readonly ManualResetEvent itemNeeded = new ManualResetEvent(true);
		readonly ManualResetEvent itemAvailable = new ManualResetEvent(false);

		bool disposed = false;
		bool done = false;
		T current;

		T Current
		{
			get
			{
				itemAvailable.WaitOne();

				T item = current;

				itemAvailable.Reset();
				itemNeeded.Set();

				return item;
			}
			set
			{
				itemNeeded.WaitOne();

				current = value;

				itemNeeded.Reset();
				itemAvailable.Set();
			}
		}

		public Breaker(Func<T> readItem)
		{
			this.readItem = readItem;
			this.reader = new Thread(ReadLoop);
			this.reader.Start();
		}
		~Breaker()
		{
			Dispose();
		}

		public virtual void Dispose()
		{
			if (!disposed)
			{
				done = true;

				if (!reader.Join(TimeSpan.FromSeconds(1.0)))
				{
					reader.Abort();
					reader.Join();
				}

				itemNeeded.Close();
				itemAvailable.Close();

				disposed = true;
			}
		}
		public T Read()
		{
			return done ? default(T) : Current;
		}
		public void Break()
		{
			done = true;

			Current = default(T);
		}

		void ReadLoop()
		{
			while (!done) Current = readItem();
		}
	}
}
