using Godot;

public partial class Enemy : CharacterBody2D {
	[Export] public MovementComponent MovementComponent;
	[Export] public float Speed = 300.0f;
	[Export] public AnimatedSprite2D Sprite;

	[Export] public Node2D? Target;
	[Export] public VelocityComponent VelocityComponent;

	public override void _PhysicsProcess(double delta) {
		if (Target == null) {
			VelocityComponent.Velocity = Vector2.Zero;
			return;
		}

		Vector2 direction = GlobalPosition.DirectionTo(Target.GlobalPosition).Normalized();
		VelocityComponent.Velocity += direction * Speed;

		Sprite.FlipH = direction.X < 0;

		if (VelocityComponent.Velocity.Length() > 0) {
			Sprite.Play("Move");
		}
		else {
			Sprite.Play("Idle");
		}
	}
}
