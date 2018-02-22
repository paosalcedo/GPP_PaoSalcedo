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

	private readonly Dictionary<System.Type, GameEvent.Handler> _eventTypeToHandlersMap = new Dictionary<System.Type, GameEvent.Handler>();

	public void Register<T>(GameEvent.Handler handler) where T : GameEvent
	{
		System.Type type = typeof(GameEvent);
		if (_eventTypeToHandlersMap.ContainsKey(type))
		{
			_eventTypeToHandlersMap[type] += handler;
		}
		else
		{
			_eventTypeToHandlersMap[type] = handler; 
		}
	}

	public void Unregister<T>(GameEvent.Handler handler) where T : GameEvent
	{
		System.Type type = typeof(GameEvent);
		GameEvent.Handler handlers;
		if (_eventTypeToHandlersMap.TryGetValue(type, out handlers))
		{
			handlers -= handler;
			if (handlers == null)
			{				
				_eventTypeToHandlersMap.Remove(type);
			}

		}
	}

	public void Fire(GameEvent e)
	{
		System.Type type = e.GetType();
		GameEvent.Handler handlers;
		if (_eventTypeToHandlersMap.TryGetValue(type, out handlers))
		{
			handlers(e);
		}
	}
}


	
 
