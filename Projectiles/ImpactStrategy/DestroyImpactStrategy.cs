using Godot;

[GlobalClass]
public partial class DestroyImpactStrategy : ImpactStrategy {
	public override void Apply(Projectile projectile, KinematicCollision2D? collision) {
		projectile.QueueFree();
	}
}
