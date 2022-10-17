using System;
using System.Collections.Generic;
using System.Linq;

namespace TheGame.Events
{
	public class GameEvent<TArgs> : IGameEvent
		where TArgs : GameEventArgs
	{
		private readonly HashSet<Action<TArgs>> globalSubscribedActions = new HashSet<Action<TArgs>>();

		public void Subscribe(Action<TArgs> action)
		{
			globalSubscribedActions.Add(action);
		}

		public void Unsubscribe(Action<TArgs> action)
		{
			globalSubscribedActions.Remove(action);
		}

		public void InvokeForGlobal(TArgs args)
		{
			foreach(var subscribedAction in globalSubscribedActions.ToList())
			{
				if (subscribedAction!= null && subscribedAction.Target != null)
				{
					subscribedAction.Invoke(args);	
				}
			}
		}
	}
}
