using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Krach.Extensions;

namespace Krach.Combinatorics
{
	public class BacktrackingSolver<TPart>
	{
		readonly ICombinatoricsProblem<TPart> problem;

		public BacktrackingSolver(ICombinatoricsProblem<TPart> problem)
		{
			if (problem == null) throw new ArgumentNullException("problem");

			this.problem = problem;
		}

		public IEnumerable<IEnumerable<TPart>> GetSolutions()
		{
			return GetSolutions(Enumerable.Empty<TPart>());
		}
		public IEnumerable<IEnumerable<TPart>> GetSolutions(IEnumerable<TPart> configuration)
		{
			switch (problem.GetState(configuration))
			{
				case CombinatoricsProblemState.IncompleteSolution:
					foreach (TPart part in problem.Parts)
						foreach (IEnumerable<TPart> solution in GetSolutions(configuration.Append(part).ToArray()))
							yield return solution;
					break;
				case CombinatoricsProblemState.Solution:
					yield return configuration;
					break;
			}
		}
	}
}
