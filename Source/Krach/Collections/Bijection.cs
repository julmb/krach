using System;
using System.Collections.Generic;
using Krach.Maps.Abstract;

namespace Krach.Collections
{
	public class Bijection<TSource, TDestination> : ISymmetricMap<TSource, TDestination>
	{
		readonly DictionaryMap<TSource, TDestination> forward = new DictionaryMap<TSource, TDestination>();
		readonly DictionaryMap<TDestination, TSource> reverse = new DictionaryMap<TDestination, TSource>();
				
		public IMap<TSource, TDestination> Forward { get { return forward; } }
		public IMap<TDestination, TSource> Reverse { get { return reverse; } }
		
		public void Add(TSource source, TDestination destination) 
		{
			forward.Add(source, destination);
			reverse.Add(destination, source);
		}
		public void Remove(TSource source, TDestination destination)
		{
			if (EqualityComparer<TDestination>.Default.Equals(forward[source], destination)) throw new ArgumentException("Source doesn't map to destination.");
			if (EqualityComparer<TSource>.Default.Equals(reverse[destination], source)) throw new ArgumentException("Destination doesn't map to source.");
			
			forward.Remove(source);
			reverse.Remove(destination);
		}
		public void RemoveSource(TSource source) 
		{
			Remove(source, forward.Map(source));
		}
		public void RemoveDestination(TDestination destination) 
		{
			Remove(reverse.Map(destination), destination);
		}
	}
}

