using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Krach.Extensions;

namespace Krach.Combinatorics
{
	public static class BacktrackingSolver
	{
		public static IEnumerable<IEnumerable<TPart>> GetSolutions<TPart>(ICombinatoricsProblem<TPart> problem)
		{
			return GetSolutions(problem, Enumerable.Empty<TPart>());
		}
		public static IEnumerable<IEnumerable<TPart>> GetSolutions<TPart>(ICombinatoricsProblem<TPart> problem, IEnumerable<TPart> configuration)
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
	}
}
