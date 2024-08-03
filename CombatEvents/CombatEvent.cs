using Godot;

public enum CombatEventType {
	ProjectileFired,
	ProjectileHit,
	ProjectileMiss,
	EnemyKilled
}

public abstract class CombatEvent {
	public abstract CombatEventType Type { get; }
}

public class ProjectileFiredCombatEvent : CombatEvent {
	public ProjectileFiredCombatEvent(Node2D projectile) {
		ProjectileId = projectile.Name;
	}

	public override CombatEventType Type => CombatEventType.ProjectileFired;
	public string ProjectileId { get; private set; }
}

public class ProjectileHitCombatEvent : CombatEvent {
	public ProjectileHitCombatEvent(Node2D projectile) {
		ProjectileId = projectile.Name;
	}

	public override CombatEventType Type => CombatEventType.ProjectileHit;
	public string ProjectileId { get; private set; }
}

public class ProjectileMissCombatEvent : CombatEvent {
	public ProjectileMissCombatEvent(Node2D projectile) {
		ProjectileId = projectile.Name;
	}

	public override CombatEventType Type => CombatEventType.ProjectileMiss;
	public string ProjectileId { get; private set; }
}

public class EnemyKilledCombatEvent : CombatEvent {
	public EnemyKilledCombatEvent(float distance) {
		Distance = distance;
	}

	public override CombatEventType Type => CombatEventType.EnemyKilled;
	public float Distance { get; private set; }
}
