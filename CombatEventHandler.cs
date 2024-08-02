using Godot;
using System;

public static class CombatEventHandler
{
	public static event Action<CombatEvent>? CombatEventTriggered;
	
	public static void HandleEvent(CombatEvent @event)
	{
		CombatEventTriggered?.Invoke(@event);
	}
}

public class CombatEvent
{
	public string Name { get; private set; }

	public static readonly CombatEvent ProjectileFired = new ("ProjectileFired");

	private CombatEvent(string name)
	{
		Name = name;
	}
}
