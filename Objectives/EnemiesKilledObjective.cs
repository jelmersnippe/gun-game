using Godot;

[GlobalClass]
public partial class EnemiesKilledObjective : Objective {
	protected override string ObjectiveText(int requiredProgression) {
		return $"Kill {requiredProgression} enemies";
	}

	protected override bool ValidateProgression(CombatEvent @event) {
		return @event.Type == CombatEventType.EnemyKilled;
	}

	protected override bool ValidateFailure(CombatEvent @event) {
		return false;
	}
}
