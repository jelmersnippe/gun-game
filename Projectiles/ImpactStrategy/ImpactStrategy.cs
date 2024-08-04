using Godot;

[GlobalClass]
public abstract partial class ImpactStrategy : Resource {
	public abstract void Apply(Projectile projectile, KinematicCollision2D? collision);
}
