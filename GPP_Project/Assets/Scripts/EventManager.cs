using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEvent
{
	public delegate void Handler(GameEvent e);
}

public class EventManager {

	private static EventManager instance;

	public static EventManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new EventManager();
			}
			return instance;
		}
	}

	private readonly Dictionary<System.Type, GameEvent.Handler> eventTypeToHandlersMap = new Dictionary<System.Type, GameEvent.Handler>();

	public void Register<T>(GameEvent.Handler handler) where T : GameEvent
	{
		System.Type type = typeof(T);
		if (eventTypeToHandlersMap.ContainsKey(type))
		{
			eventTypeToHandlersMap[type] += handler;
		}
		else
		{
			eventTypeToHandlersMap[type] = handler; 
		}
	}

	public void Unregister<T>(GameEvent.Handler handler) where T : GameEvent
	{
		System.Type type = typeof(T);
		GameEvent.Handler handlers;
		if (eventTypeToHandlersMap.TryGetValue(type, out handlers))
		{
			handlers -= handler;
			if (handlers == null)
			{				
				eventTypeToHandlersMap.Remove(type);
			}
			else
			{
				eventTypeToHandlersMap[type] = handlers;
			}

		}
	}

	public void Fire(GameEvent e)
	{
		System.Type type = e.GetType();
		GameEvent.Handler handlers;
		if (eventTypeToHandlersMap.TryGetValue(type, out handlers))
		{
			handlers(e);
		}
	}
}


	
 
