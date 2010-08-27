using System.Collections.Generic;

namespace Krach.MachineLearning
{
	public class SimpleGameAI : GameAI
	{
		IGameState currentState;

		public SimpleGameAI(IEnumerable<IGameAction> actions) : base(actions) { }

		public IGameAction PerformStep(IGameState state, double rating)
		{
			if (currentState != null) AddTransition(currentState, state, GetAction(currentState));

			currentState = state;

			AddRating(currentState, rating);

			return GetAction(currentState);
		}
	}
}
