using Godot;

[GlobalClass]
public partial class ProjectilesFiredObjective : Objective {
	protected override string ObjectiveText(int requiredProgression) {
		return $"Fire {requiredProgression} projectiles";
	}

	protected override bool ValidateProgression(CombatEvent @event) {
		return @event.Type == CombatEventType.ProjectileFired;
	}

	protected override bool ValidateFailure(CombatEvent @event) {
		return false;
	}
}
