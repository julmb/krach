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
