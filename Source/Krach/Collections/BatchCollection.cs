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
using System.Collections;
using System.Collections.Generic;

namespace Krach.Collections
{
	public class BatchCollection<T> : IEnumerable<T>
	{
		struct Action
		{
			readonly ActionType type;
			readonly T item;

			public ActionType Type { get { return type; } }
			public T Item { get { return item; } }

			public Action(ActionType type, T item)
			{
				this.type = type;
				this.item = item;
			}
		}
		enum ActionType { Add, Remove }

		readonly List<T> items = new List<T>();
		readonly List<Action> actions = new List<Action>();

		public void Add(T item)
		{
			actions.Add(new Action(ActionType.Add, item));
		}
		public void Remove(T item)
		{
			actions.Add(new Action(ActionType.Remove, item));
		}
		public void Clear()
		{
			foreach (T item in items) Remove(item);
		}
		public void Process()
		{
			foreach (Action action in actions)
				switch (action.Type)
				{
					case ActionType.Add: items.Add(action.Item); break;
					case ActionType.Remove: items.Remove(action.Item); break;
					default: throw new InvalidOperationException();
				}

			actions.Clear();
		}

		public IEnumerator<T> GetEnumerator()
		{
			return items.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
