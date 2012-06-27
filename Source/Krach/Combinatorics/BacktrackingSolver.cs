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
		readonly ICombinatoricsProblem<TPart> problem;
		readonly IEnumerator<IEnumerable<TPart>> enumerator;
		readonly List<IEnumerable<TPart>> result;

		public IEnumerable<IEnumerable<TPart>> Result { get { return result; } }
		public double Progress { get { return result.Count == 0 ? 0 : GetPosition(problem, result[result.Count - 1]); } }

		public BacktrackingSolver(ICombinatoricsProblem<TPart> problem)
		{
			if (problem == null) throw new ArgumentNullException("problem");

			this.problem = problem;
			this.enumerator = GetSolutions(problem, Enumerable.Empty<TPart>()).GetEnumerator();
			this.result = new List<IEnumerable<TPart>>();
		}

		public bool PerformStep()
		{
			if (!enumerator.MoveNext()) return false;

			result.Add(enumerator.Current);

			return true;
		}

		static IEnumerable<IEnumerable<TPart>> GetSolutions(ICombinatoricsProblem<TPart> problem, IEnumerable<TPart> configuration)
		{
			switch (problem.GetState(configuration))
			{
				case CombinatoricsProblemState.IncompleteSolution:
					foreach (TPart part in problem.Parts)
						foreach (IEnumerable<TPart> solution in GetSolutions(problem, configuration.Append(part).ToArray()))
							yield return solution;
					break;
				case CombinatoricsProblemState.Solution:
					yield return configuration;
					break;
			}
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
