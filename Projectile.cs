using Godot;

public partial class Projectile : StaticBody2D {
	private bool _impacted;
	[Export] public HitboxComponent HitboxComponent = null!;
	[Export] public ImpactStrategy HurtboxImpactStrategy;
	[Export] public PackedScene ImpactEffect;
	[Export] public float LifeTime = 1f;
	[Export] public ImpactStrategy OtherImpactStrategy;
	[Export] public PackedScene Shell;
	[Export] public float Speed = 200f;

	[Export] public VelocityStrategy? VelocityStrategy;

	public override void _Ready() {
		HitboxComponent.AreaEntered += HandleImpact;
		HitboxComponent.BodyEntered += HandleImpact;

		SceneTreeTimer? lifeTimeTimer = GetTree().CreateTimer(LifeTime);
		lifeTimeTimer.Timeout += () => {
			CombatEventHandler.HandleEvent(new ProjectileMissCombatEvent(this));
			QueueFree();
		};

		VelocityStrategy?.Apply(this);
	}

	public override void _Process(double delta) {
		KinematicCollision2D? collision = MoveAndCollide(Transform.X * Speed * (float)delta, true);

		if (collision != null) {
			OtherImpactStrategy.Apply(this, collision);
			GodotObject? collider = collision.GetCollider();
			if (collider is Node2D node) {
				// TODO: Performing impact strategy before movement means things happen away from the impact (slightly away from wall)
				// which is fine for bouncing but looks bad for the impact effect
				HandleImpact(node);
			}
		}

		Position += Transform.X * Speed * (float)delta;
	}

	private void HandleImpact(Node2D collision) {
		var impactEffect = ImpactEffect.Instantiate<Node2D>();
		impactEffect.RotationDegrees = RotationDegrees;
		impactEffect.GlobalPosition = GlobalPosition;

		GetTree().CurrentScene.CallDeferred("add_child", impactEffect);

		// Check layer because environment could be destroyable and have hurtbox
		bool hit = collision switch {
			Area2D area => area.GetCollisionLayerValue(2),
			PhysicsBody2D body => body.GetCollisionLayerValue(2),
			_ => false
		};

		if (hit) {
			CombatEventHandler.HandleEvent(new ProjectileHitCombatEvent(this));
		}
		else {
			CombatEventHandler.HandleEvent(new ProjectileMissCombatEvent(this));
		}

		if (collision is HurtboxComponent) {
			HurtboxImpactStrategy.Apply(this, null);
		}
	}
}
