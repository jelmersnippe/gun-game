using System;
using Godot;
using System.Collections.Generic;

public partial class ObjectiveController : Node
{
	[Signal]
	public delegate void ObjectiveUpdatedEventHandler(Objective objective);
	
	[Signal]
	public delegate void ObjectiveCompletedEventHandler(Objective objective);
	
	private readonly List<Objective> _activeObjectives = new();

	public override void _Ready()
	{
		CombatEventHandler.CombatEventTriggered += CombatEventHandlerOnCombatEventTriggered;
		AddObjective(new ProjectilesFiredObjective());
	}

	private void CombatEventHandlerOnCombatEventTriggered(CombatEvent @event)
	{
		foreach (var objective in _activeObjectives)
		{
			var changed = objective.HandleEvent(@event);
			if (changed)
			{
				EmitSignal(SignalName.ObjectiveUpdated, objective);
			}
		}
	}

	public void AddObjective(Objective obj)
	{
		_activeObjectives.Add(obj);
		obj.Completed += ObjOnCompleted;
	}

	private void ObjOnCompleted(Objective obj)
	{
		GD.Print("ObjectiveController - completed");
		EmitSignal(SignalName.ObjectiveCompleted, obj);
		obj.Completed -= ObjOnCompleted;
		// _activeObjectives.Remove(obj);
	}
}

public abstract partial class Objective : RefCounted
{
	[Signal]
	public delegate void CompletedEventHandler(Objective objective);
	
	private int _currentProgression = 0;
	protected abstract int RequiredProgression { get; }
	protected abstract string ObjectiveText { get; }
	public string DisplayText => ObjectiveText + $" ({_currentProgression}/{RequiredProgression})";
		
	public abstract bool ValidateProgression(CombatEvent @event);
	public abstract bool ValidateFailure(CombatEvent @event);

	public bool HandleEvent(CombatEvent @event)
	{
		return TryProgress(@event) || TryFailure(@event);
	}

	private bool TryProgress(CombatEvent @event)
	{
		if (_currentProgression >= RequiredProgression || !ValidateProgression(@event))
		{
			return false;
		}

		_currentProgression++;

		if (_currentProgression >= RequiredProgression)
		{
			Complete();
			// Return false so the complete event takes precedence
			return false;
		}

		return true;
	}

	private void Complete()
	{
		GD.Print("Objective completed");
		EmitSignal(SignalName.Completed, this);
	}

	private bool TryFailure(CombatEvent @event)
	{
		if (!ValidateFailure(@event))
		{
			return false;
		}

		_currentProgression = 0;
		return true;
	}
}

public partial class ProjectilesFiredObjective : Objective
{
	protected override int RequiredProgression => 30;
	protected override string ObjectiveText => $"Fire {RequiredProgression} projectiles";
	
	public override bool ValidateProgression(CombatEvent @event)
	{
		return @event.Name == CombatEvent.ProjectileFired.Name;
	}

	public override bool ValidateFailure(CombatEvent @event)
	{
		return false;
	}
}
