using Godot;

public partial class HurtboxComponent : Area2D {
	[Signal]
	public delegate void HitEventHandler(HitboxComponent hitboxComponent, Vector2 direction);

	[Export] public HitflashComponent? HitflashComponent;
	[Export] public float HurtTime = 0.2f;
	[Export] public AnimatedSprite2D? Sprite;

	public override void _Ready() {
		AreaEntered += HandleCollision;
		BodyEntered += HandleCollision;
	}

	private void HandleCollision(Node2D node) {
		if (node is not HitboxComponent hitboxComponent) {
			return;
		}

		EmitSignal(SignalName.Hit, hitboxComponent, GlobalPosition.DirectionTo(hitboxComponent.GlobalPosition));

		if (Sprite != null) {
			Sprite.Play("Hurt");

			SceneTreeTimer? hurtTimer = GetTree().CreateTimer(HurtTime);
			hurtTimer.Timeout += () => Sprite.Play("Idle");
		}

		HitflashComponent?.Flash();

		Engine.TimeScale = 0;

		SceneTreeTimer? hitStopTimer = GetTree().CreateTimer(hitboxComponent.HitstopTime, ignoreTimeScale: true);
		hitStopTimer.Timeout += () => Engine.TimeScale = 1f;
	}
}
