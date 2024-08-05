using Godot;

[GlobalClass]
public partial class BounceImpactStrategy : ImpactStrategy {
	public override void Apply(Projectile projectile, KinematicCollision2D? collision) {
		if (collision == null) {
			return;
		}

		var direction = new Vector2(Mathf.Cos(projectile.GlobalRotation), Mathf.Sin(projectile.GlobalRotation));
		Vector2 collisionNormal = collision.GetNormal();

		Vector2 reflectedDirection = direction.Bounce(collisionNormal);

		projectile.GlobalRotation = reflectedDirection.Angle();
	}
}
