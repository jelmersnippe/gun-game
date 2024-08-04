using Godot;

[GlobalClass]
public partial class BounceImpactStrategy : ImpactStrategy {
	public override void Apply(Projectile projectile, KinematicCollision2D? collision) {
		if (collision == null) {
			return;
		}

		// TODO: Figure out how to do properly with a Node2D instead so we dont need staticbody2d on everything
		var direction = new Vector2(Mathf.Cos(projectile.GlobalRotation), Mathf.Sin(projectile.GlobalRotation));
		Vector2 collisionNormal = collision.GetNormal();

		Vector2 reflectedDirection = direction.Bounce(collisionNormal);

		projectile.GlobalRotation = reflectedDirection.Angle();
	}
}
