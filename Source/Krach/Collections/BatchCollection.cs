using System;
using System.Collections;
using System.Collections.Generic;

namespace Dash.Collections
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
