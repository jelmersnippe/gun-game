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
		HitboxComponent.AreaEntered += HandleHurtboxImpact;
		HitboxComponent.BodyEntered += HandleHurtboxImpact;

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
			if (OtherImpactStrategy.TriggerImpactEffect) {
				ShowImpactEffect(GlobalPosition + Transform.X * Speed * (float)delta);
			}

			OtherImpactStrategy.Apply(this, collision);
		}

		Position += Transform.X * Speed * (float)delta;
	}

	private void HandleHurtboxImpact(Node2D collision) {
		if (collision is not HurtboxComponent) {
			return;
		}

		if (HurtboxImpactStrategy.TriggerImpactEffect) {
			ShowImpactEffect(GlobalPosition);
		}

		HurtboxImpactStrategy.Apply(this, null);
	}

	private void ShowImpactEffect(Vector2 position) {
		var impactEffect = ImpactEffect.Instantiate<Node2D>();
		impactEffect.RotationDegrees = RotationDegrees;
		impactEffect.GlobalPosition = position;

		GetTree().CurrentScene.CallDeferred("add_child", impactEffect);
	}
}
