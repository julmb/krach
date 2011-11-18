using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Krach.Combinatorics
{
	public interface ICombinatoricsProblem<TPart>
	{
		IEnumerable<TPart> Parts { get; }

		CombinatoricsProblemState GetState(IEnumerable<TPart> configuration);
	}
}
