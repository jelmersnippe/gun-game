using Godot;

public partial class Projectile : Node2D {
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
		var impactEffect = ImpactEffect.Instantiate<ImpactEffect>();
		impactEffect.RotationDegrees = RotationDegrees;
		impactEffect.GlobalPosition = GlobalPosition;

		GetTree().CurrentScene.CallDeferred("add_child", impactEffect);

		QueueFree();
	}
}
