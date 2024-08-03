using Godot;

public partial class Enemy : CharacterBody2D {
	[Export] public HealthComponent HealthComponent;
	[Export] public HurtboxComponent HurtboxComponent;
	[Export] public MovementComponent MovementComponent;
	[Export] public float Speed = 300.0f;
	[Export] public AnimatedSprite2D Sprite;

	[Export] public Node2D? Target;
	[Export] public VelocityComponent VelocityComponent;

	public override void _Ready() {
		HurtboxComponent.Hit += HurtboxComponentOnHit;
		HealthComponent.Died += HealthComponentOnDied;
	}

	private void HealthComponentOnDied() {
		// TODO: Implement effects
		CombatEventHandler.HandleEvent(
			new EnemyKilledCombatEvent(Target != null ? GlobalPosition.DistanceTo(Target.GlobalPosition) : 0));
		QueueFree();
	}

	private void HurtboxComponentOnHit(HitboxComponent hitboxComponent, Vector2 direction) {
		HealthComponent.TakeDamage(hitboxComponent.ContactDamage);
	}

	public override void _PhysicsProcess(double delta) {
		if (!IsInstanceValid(Target)) {
			Target = null;
		}

		if (Target == null) {
			VelocityComponent.Velocity = Vector2.Zero;
			Sprite.Play("Idle");
			return;
		}

		Vector2 direction = GlobalPosition.DirectionTo(Target.GlobalPosition).Normalized();
		VelocityComponent.Velocity += direction * Speed;

		Sprite.FlipH = direction.X < 0;
		Sprite.Play("Move");
	}
}
