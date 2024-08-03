using Godot;

[GlobalClass]
public partial class AccuracyObjective : Objective {
	protected override string ObjectiveText(int requiredProgression) {
		return $"Fire {requiredProgression} projectiles without missing";
	}

	protected override bool ValidateProgression(CombatEvent @event) {
		return @event.Type == CombatEventType.ProjectileHit;
	}

	protected override bool ValidateFailure(CombatEvent @event) {
		return @event.Type == CombatEventType.ProjectileMiss;
	}
}
