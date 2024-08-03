using Godot;

public partial class Projectile : Node2D {
	private bool _impacted;
	[Export] public HitboxComponent HitboxComponent = null!;
	[Export] public PackedScene ImpactEffect;
	[Export] public PackedScene Shell;
	[Export] public float Speed = 200f;

	public override void _Ready() {
		HitboxComponent.AreaEntered += SpawnImpactEffect;
		HitboxComponent.BodyEntered += SpawnImpactEffect;
	}

	public override void _Process(double delta) {
		Position += Transform.X * Speed * (float)delta;
	}

	private void SpawnImpactEffect(Node2D collision) {
		if (_impacted) {
			return;
		}

		_impacted = true;
		var impactEffect = ImpactEffect.Instantiate<ImpactEffect>();
		impactEffect.RotationDegrees = RotationDegrees;
		impactEffect.GlobalPosition = GlobalPosition;

		GetTree().CurrentScene.CallDeferred("add_child", impactEffect);

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

		QueueFree();
	}
}
