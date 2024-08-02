using System;
using Godot;
using System.Collections.Generic;
using Godot.Collections;

public partial class ObjectiveController : Node
{
	[Export] public int InitialObjectiveCount = 0;
	public event Action<IReadOnlyList<Objective>>? ObjectivesUpdated;
	public event Action<Objective>? ObjectiveCompleted;
	
	private readonly List<Objective> _activeObjectives = new();
	

	public override void _Ready()
	{
		CombatEventHandler.CombatEventTriggered += CombatEventHandlerOnCombatEventTriggered;

		var initialObjectiveTimer = GetTree().CreateTimer(0.2f);
		initialObjectiveTimer.Timeout += () =>
		{
			for (var i = 0; i < InitialObjectiveCount; i++)
			{
				AddObjective(new ProjectilesFiredObjective(30));
			}
		};
	}

	private void CombatEventHandlerOnCombatEventTriggered(CombatEvent @event)
	{
		foreach (var objective in _activeObjectives)
		{
			var changed = objective.HandleEvent(@event);
			if (changed)
			{
				ObjectivesUpdated?.Invoke(_activeObjectives.AsReadOnly());
			}
		}
	}

	public void AddObjective(Objective obj)
	{
		_activeObjectives.Add(obj);
		obj.Completed += ObjOnCompleted;
		ObjectivesUpdated?.Invoke(_activeObjectives);
	}

	private void ObjOnCompleted(Objective obj)
	{
		ObjectiveCompleted?.Invoke(obj);
		obj.Completed -= ObjOnCompleted;

		var objectiveCompletedTimer = GetTree().CreateTimer(1.5f);
		objectiveCompletedTimer.Timeout += () => RemoveObjective(obj);
		
		var newObjectiveTimer = GetTree().CreateTimer(2f);
		newObjectiveTimer.Timeout += () => AddObjective(new ProjectilesFiredObjective(30));
	}

	private void RemoveObjective(Objective obj)
	{
		_activeObjectives.Remove(obj);
		ObjectivesUpdated?.Invoke(_activeObjectives);
	}
}

public abstract partial class Objective : RefCounted
{
	[Signal]
	public delegate void CompletedEventHandler(Objective objective);
	
	private int _currentProgression = 0;
	private int RequiredProgression { get; }
	protected abstract string ObjectiveText(int requiredProgression);
	public string DisplayText => ObjectiveText(RequiredProgression) + $" ({_currentProgression}/{RequiredProgression})";

	protected abstract bool ValidateProgression(CombatEvent @event);
	protected abstract bool ValidateFailure(CombatEvent @event);

	protected Objective(int requiredProgression)
	{
		RequiredProgression = requiredProgression;
	}

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
		}

		return true;
	}

	private void Complete()
	{
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
	protected override string ObjectiveText(int requiredProgression)
	{
		return $"Fire {requiredProgression} projectiles";
	}

	protected override bool ValidateProgression(CombatEvent @event)
	{
		return @event.Name == CombatEvent.ProjectileFired.Name;
	}

	protected override bool ValidateFailure(CombatEvent @event)
	{
		return false;
	}

	public ProjectilesFiredObjective(int requiredProgression) : base(requiredProgression)
	{
	}
}
