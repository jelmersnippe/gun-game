using Godot;

[GlobalClass]
public partial class DestroyImpactStrategy : ImpactStrategy {
	public override void Apply(Projectile projectile, Node2D collision) {
		projectile.QueueFree();
	}
}
