using Godot;

public partial class Shell : Sprite2D {
	private bool _onGround;

	private Tween _tween;
	[Export] public float AirTime = 0.8f;
	public Vector2 Velocity = Vector2.Zero;

	public override void _Ready() {
		SceneTreeTimer? timer = GetTree().CreateTimer(AirTime);
		timer.Timeout += () => _onGround = true;

		_tween = GetTree().CreateTween();

		float targetRotationRadians = Mathf.DegToRad(Rotation - 180f);

		_tween.TweenProperty(this, "rotation", targetRotationRadians, AirTime);

		_tween.Play();
	}

	public override void _Process(double delta) {
		if (_onGround) {
			return;
		}

		Velocity.Y += 9.8f;

		Position += Velocity * (float)delta;
	}
}
