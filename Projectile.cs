using Godot;

public partial class Projectile : Area2D {
	[Export] public PackedScene ImpactEffect;
	[Export] public PackedScene Shell;
	[Export] public float Speed = 200f;

	public override void _Ready() {
		AreaEntered += OnAreaEntered;
		BodyEntered += HandleCollision;
	}

	private void OnAreaEntered(Area2D area) {
		HandleCollision(area);
	}

	public override void _Process(double delta) {
		Position += Transform.X * Speed * (float)delta;
	}

	private void HandleCollision(Node2D node) {
		var impactEffect = ImpactEffect.Instantiate<ImpactEffect>();
		impactEffect.RotationDegrees = RotationDegrees;
		impactEffect.GlobalPosition = GlobalPosition;

		GetTree().CurrentScene.CallDeferred("add_child", impactEffect);

		Engine.TimeScale = 0;

		SceneTreeTimer? hitStopTimer = GetTree().CreateTimer(0.02f, ignoreTimeScale: true);
		hitStopTimer.Timeout += () => Engine.TimeScale = 1f;

		QueueFree();
	}
}