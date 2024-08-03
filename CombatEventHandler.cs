using System;

public static class CombatEventHandler {
	public static event Action<CombatEvent>? CombatEventTriggered;

	public static void HandleEvent(CombatEvent @event) {
		CombatEventTriggered?.Invoke(@event);
	}
}
