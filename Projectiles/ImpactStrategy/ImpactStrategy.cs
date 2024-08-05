using Godot;

[GlobalClass]
public abstract partial class ImpactStrategy : Resource {
	[Export] public bool TriggerImpactEffect = true;

	public abstract void Apply(Projectile projectile, KinematicCollision2D? collision);
}
