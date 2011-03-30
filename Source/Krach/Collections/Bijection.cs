using System;
using System.Collections.Generic;
using Krach.Maps.Abstract;
using Krach.Extensions;
using System.Linq;

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
			Remove(source, forward[source]);
		}
		public void RemoveDestination(TDestination destination) 
		{
			Remove(reverse[destination], destination);
		}
		public void RotateSource(IEnumerable<TSource> rotateChain, int offset) 
		{
			IEnumerable<TDestination> oldDestination = from item in rotateChain select forward[item];
			IEnumerable<TSource> rotatedSource = rotateChain.Rotate(offset);
			
			foreach (Tuple<TDestination, TSource> chainItem in Enumerable.Zip(oldDestination, rotatedSource, Tuple.Create).ToArray())
			{
				reverse[chainItem.Item1] = chainItem.Item2;
				forward[chainItem.Item2] = chainItem.Item1;
			}
		}
		public void RotateDestination(IEnumerable<TDestination> rotateChain, int offset) 
		{
			IEnumerable<TSource> oldSource = from item in rotateChain select reverse[item];
			IEnumerable<TDestination> rotatedDestination = rotateChain.Rotate(offset);
			
			foreach (Tuple<TSource, TDestination> chainItem in Enumerable.Zip(oldSource, rotatedDestination, Tuple.Create).ToArray())
			{
				forward[chainItem.Item1] = chainItem.Item2;
				reverse[chainItem.Item2] = chainItem.Item1;
			}
		}
		public void ReplaceSource(IEnumerable<TSource> oldSources, IEnumerable<TSource> newSources)
		{
			if (!oldSources.IsDistinct())
				throw new ArgumentException("The old destination items are not completely distinct.");
			if (!newSources.IsDistinct())
				throw new ArgumentException("The new destination items are not completely distinct.");
			if (oldSources.Count() != newSources.Count())
				throw new ArgumentException("The number of old destination items does not match the number of new destination items.");
			if (!reverse.Values.ContainsAll(oldSources))
				throw new ArgumentException("Not all of the old destination items are part of the bijection.");
			if (reverse.Values.ContainsAny(newSources))
				throw new ArgumentException("Some of the new destination items are already part of the bijection.");

			foreach (Tuple<TSource, TSource> replacement in Enumerable.Zip(oldSources, newSources, Tuple.Create))
			{
				TDestination destination = forward[replacement.Item1];
				reverse[destination] = replacement.Item2;
				forward.Remove(replacement.Item1);
				forward.Add(replacement.Item2, destination);
			}
		}
		public void ReplaceDestination(IEnumerable<TDestination> oldDestinations, IEnumerable<TDestination> newDestinations)
		{
			if (!oldDestinations.IsDistinct())
				throw new ArgumentException("The old destination items are not completely distinct.");
			if (!newDestinations.IsDistinct())
				throw new ArgumentException("The new destination items are not completely distinct.");
			if (oldDestinations.Count() != newDestinations.Count())
				throw new ArgumentException("The number of old destination items does not match the number of new destination items.");
			if (!forward.Values.ContainsAll(oldDestinations))
				throw new ArgumentException("Not all of the old destination items are part of the bijection.");
			if (forward.Values.ContainsAny(newDestinations))
				throw new ArgumentException("Some of the new destination items are already part of the bijection.");

			foreach (Tuple<TDestination, TDestination> replacement in Enumerable.Zip(oldDestinations, newDestinations, Tuple.Create))
			{
				TSource source = reverse[replacement.Item1];
				forward[source] = replacement.Item2;
				reverse.Remove(replacement.Item1);
				reverse.Add(replacement.Item2, source);
			}
		}
	}
}

