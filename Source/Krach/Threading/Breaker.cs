// Copyright © Julian Brunner 2010

// This file is part of Krach.
//
// Krach is free software: you can redistribute it and/or modify it under the
// terms of the GNU Lesser General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
//
// Krach is distributed in the hope that it will be useful, but WITHOUT ANY
// WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR
// A PARTICULAR PURPOSE. See the GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License along with
// Krach. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Threading;

namespace Krach.Threading
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
