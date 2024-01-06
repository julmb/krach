// Copyright © Julian Brunner 2010 - 2011

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
using System.Collections.Generic;
using System.Linq;
using Krach.Extensions;

namespace Krach.Combinatorics
{
	public class BacktrackingSolver<TPart> : IAction<IEnumerable<IEnumerable<TPart>>>
	{
		readonly Stack<IEnumerable<TPart>> prefixes = new Stack<IEnumerable<TPart>>(Enumerables.Create(Enumerable.Empty<TPart>()));
		readonly List<IEnumerable<TPart>> solutions = new List<IEnumerable<TPart>>();
		readonly ICombinatoricsProblem<TPart> problem;

		public IEnumerable<IEnumerable<TPart>> Result { get { return solutions; } }
		public double Progress { get { return prefixes.Count == 0 ? 1 : GetPosition(problem, prefixes.Peek()); } }

		public BacktrackingSolver(ICombinatoricsProblem<TPart> problem)
		{
			if (problem == null) throw new ArgumentNullException("problem");

			this.problem = problem;
		}

		public bool PerformStep()
		{
			if (prefixes.Count == 0) return false;

			IEnumerable<TPart> configuration = prefixes.Pop();

			switch (problem.GetState(configuration))
			{
				case CombinatoricsProblemState.IncompleteSolution:
					foreach (TPart part in problem.Parts.Reverse()) prefixes.Push(configuration.Append(part).ToArray());
					break;
				case CombinatoricsProblemState.Solution:
					solutions.Add(configuration);
					break;
				case CombinatoricsProblemState.Contradiction:
					break;
			}

			return true;
		}

		static double GetPosition(ICombinatoricsProblem<TPart> problem, IEnumerable<TPart> configuration)
		{
			if (!configuration.Any()) return 0;

			double headFraction = (double)problem.Parts.GetIndex(configuration.First()) / (double)problem.Parts.Count();
			double tailFraction = GetPosition(problem, configuration.Skip(1).ToArray()) / (double)problem.Parts.Count();

			return headFraction + tailFraction;
		}
	}
}
