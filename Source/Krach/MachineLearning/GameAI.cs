// Copyright © Julian Brunner 2010

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
using Krach.Extensions;
using System.Linq;

namespace Krach.MachineLearning
{
	// TODO:
	// - What about (continuous) states that are very similar? How do we calculate transition probabilities correctly?
	// - Backward propagation (if B is great and A has a good chance to lead into B, then A is also good)
	// - States of varying size (Snake)
	public class GameAI
	{
		readonly IEnumerable<IGameAction> actions;

		public GameAI(IEnumerable<IGameAction> actions)
		{
			if (actions == null) throw new ArgumentNullException("actions");

			this.actions = actions;
		}

		public void AddTransition(IGameState source, IGameState destination, IGameAction action)
		{
			throw new NotImplementedException();
		}
		public void AddRating(IGameState state, double rating)
		{
			throw new NotImplementedException();
		}
		public IGameAction GetAction(IGameState state)
		{
			throw new NotImplementedException();
		}
	}
}
