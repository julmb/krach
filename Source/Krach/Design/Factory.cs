// Copyright © Julian Brunner 2009 - 2011

// This file is part of Stream Visualizer (streamvis).
// 
// Stream Visualizer is free software: you can redistribute it and/or modify it
// under the terms of the GNU General Public License as published by the Free
// Software Foundation, either version 3 of the License, or (at your option) any
// later version.
// 
// Stream Visualizer is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
// FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more
// details.
// 
// You should have received a copy of the GNU General Public License along with
// Stream Visualizer. If not, see <http://www.gnu.org/licenses/>.

using System;

namespace Krach.Design
{
	public class Factory<TResult> : IFactory<TResult>
	{
		readonly Func<TResult> create;

		public Factory(Func<TResult> create)
		{
			if (create == null) throw new ArgumentNullException("create");

			this.create = create;
		}

		public TResult Create()
		{
			return create();
		}
	}
	public class Factory<TResult, T1> : IFactory<TResult, T1>
	{
		readonly Func<T1, TResult> create;

		public Factory(Func<T1, TResult> create)
		{
			if (create == null) throw new ArgumentNullException("create");

			this.create = create;
		}

		public TResult Create(T1 parameter1)
		{
			return create(parameter1);
		}
	}
	public class Factory<TResult, T1, T2> : IFactory<TResult, T1, T2>
	{
		readonly Func<T1, T2, TResult> create;

		public Factory(Func<T1, T2, TResult> create)
		{
			if (create == null) throw new ArgumentNullException("create");

			this.create = create;
		}

		public TResult Create(T1 parameter1, T2 parameter2)
		{
			return create(parameter1, parameter2);
		}
	}
	public class Factory<TResult, T1, T2, T3> : IFactory<TResult, T1, T2, T3>
	{
		readonly Func<T1, T2, T3, TResult> create;

		public Factory(Func<T1, T2, T3, TResult> create)
		{
			if (create == null) throw new ArgumentNullException("create");

			this.create = create;
		}

		public TResult Create(T1 parameter1, T2 parameter2, T3 parameter3)
		{
			return create(parameter1, parameter2, parameter3);
		}
	}
	public class Factory<TResult, T1, T2, T3, T4> : IFactory<TResult, T1, T2, T3, T4>
	{
		readonly Func<T1, T2, T3, T4, TResult> create;

		public Factory(Func<T1, T2, T3, T4, TResult> create)
		{
			if (create == null) throw new ArgumentNullException("create");

			this.create = create;
		}

		public TResult Create(T1 parameter1, T2 parameter2, T3 parameter3, T4 parameter4)
		{
			return create(parameter1, parameter2, parameter3, parameter4);
		}
	}
	public class Factory<TResult, T1, T2, T3, T4, T5> : IFactory<TResult, T1, T2, T3, T4, T5>
	{
		readonly Func<T1, T2, T3, T4, T5, TResult> create;

		public Factory(Func<T1, T2, T3, T4, T5, TResult> create)
		{
			if (create == null) throw new ArgumentNullException("create");

			this.create = create;
		}

		public TResult Create(T1 parameter1, T2 parameter2, T3 parameter3, T4 parameter4, T5 parameter5)
		{
			return create(parameter1, parameter2, parameter3, parameter4, parameter5);
		}
	}
}
