using System;
using System.Collections.Generic;
using Krach.Maps.Abstract;

namespace Krach
{
	public class DictionaryMap<TSource, TDestination> : Dictionary<TSource, TDestination>, IMap<TSource, TDestination>
	{
		public TDestination Map (TSource value) { return this[value]; }
	}
}

