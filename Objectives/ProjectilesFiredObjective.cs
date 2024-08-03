public partial class ProjectilesFiredObjective : Objective {
	public ProjectilesFiredObjective(int requiredProgression) : base(requiredProgression) {
	}

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
