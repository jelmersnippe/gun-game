using Godot;
using System;

public partial class Shell : Sprite2D {
	[Export] public float AirTime = 0.8f;
	public Vector2 Velocity = Vector2.Zero;
	private bool _onGround = false;

	private Tween _tween;

	public override void _Ready() {
		var timer = GetTree().CreateTimer(AirTime);
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
