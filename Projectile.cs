using Godot;

public partial class Projectile : Node2D {
	private bool _impacted;
	[Export] public HitboxComponent HitboxComponent = null!;
	[Export] public ImpactStrategy? HurtboxImpactStrategy;
	[Export] public PackedScene ImpactEffect;
	[Export] public float LifeTime = 1f;
	[Export] public ImpactStrategy? OtherImpactStrategy;
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
			HurtboxImpactStrategy?.Apply(this, collision);
		}
		else {
			OtherImpactStrategy?.Apply(this, collision);
		}
	}
}
