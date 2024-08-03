using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class ObjectiveController : Node {
	private readonly List<Objective> _activeObjectives = new();

	private readonly List<Func<int, Objective>> _availableObjectives = new() {
		requiredProgression => new ProjectilesFiredObjective(requiredProgression),
		requiredProgression => new EnemiesKilledObjective(requiredProgression),
		requiredProgression => new AccuracyObjective(requiredProgression)
	};

	[Export] public int InitialObjectiveCount;
	public event Action<IReadOnlyList<Objective>>? ObjectivesUpdated;
	public event Action<Objective>? ObjectiveCompleted;

	public override void _EnterTree() {
		CombatEventHandler.CombatEventTriggered += CombatEventHandlerOnCombatEventTriggered;
	}

	public override void _ExitTree() {
		CombatEventHandler.CombatEventTriggered -= CombatEventHandlerOnCombatEventTriggered;
	}

	public override void _Ready() {
		SceneTreeTimer? initialObjectiveTimer = GetTree().CreateTimer(0.2f);
		initialObjectiveTimer.Timeout += () => {
			for (int i = 0; i < Mathf.Clamp(InitialObjectiveCount, 0, _availableObjectives.Count); i++) {
				AddRandomObjective();
			}
		};
	}

	private void CombatEventHandlerOnCombatEventTriggered(CombatEvent @event) {
		foreach (Objective objective in _activeObjectives) {
			bool changed = objective.HandleEvent(@event);
			if (changed) {
				ObjectivesUpdated?.Invoke(_activeObjectives.AsReadOnly());
			}
		}
	}

	private void AddRandomObjective() {
		var rng = new RandomNumberGenerator();
		int objectiveIndex = rng.RandiRange(0, _availableObjectives.Count - 1);
		var objectiveCreation = _availableObjectives[objectiveIndex];

		Objective objective = objectiveCreation(30);

		if (_activeObjectives.Any(x => x.GetType() == objective.GetType())) {
			AddRandomObjective();
			return;
		}

		AddObjective(objective);
	}

	public void AddObjective(Objective obj) {
		_activeObjectives.Add(obj);
		obj.Completed += ObjOnCompleted;
		ObjectivesUpdated?.Invoke(_activeObjectives);
	}

	private void ObjOnCompleted(Objective obj) {
		ObjectiveCompleted?.Invoke(obj);
		obj.Completed -= ObjOnCompleted;

		SceneTreeTimer? objectiveCompletedTimer = GetTree().CreateTimer(1.5f);
		objectiveCompletedTimer.Timeout += () => RemoveObjective(obj);

		SceneTreeTimer? newObjectiveTimer = GetTree().CreateTimer(2f);
		newObjectiveTimer.Timeout += AddRandomObjective;
	}

	private void RemoveObjective(Objective obj) {
		_activeObjectives.Remove(obj);
		ObjectivesUpdated?.Invoke(_activeObjectives);
	}
}
