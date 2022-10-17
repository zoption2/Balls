using System;
using System.Collections.Generic;

namespace TheGame.Events
{
	public class EventBus : IEventBus
	{
		private EventBus() {}

		private readonly Dictionary<Type, IGameEvent> typesToEventsMap = new Dictionary<Type, IGameEvent>();

		public TEventType GetEvent<TEventType>() where TEventType : class, IGameEvent, new() 
		{
			var type = typeof(TEventType);

			if(!typesToEventsMap.ContainsKey(type))
			{
				var newEvent = new TEventType();
				typesToEventsMap.Add(type, newEvent);
			}

			return (TEventType)typesToEventsMap[type];
		}
	}

	public interface IEventBus
    {
		public TEventType GetEvent<TEventType>() where TEventType : class, IGameEvent, new();

	}
}
