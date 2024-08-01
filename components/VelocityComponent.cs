using Godot;
using System;

public partial class VelocityComponent : Node
{
	[Export] public Vector2 Velocity;
	[Export] public float MaxVelocity = 300f;
	[Export] public float TimeToStop = 0.1f;

	public override void _Process(double delta) {
		if (Velocity.Length() > MaxVelocity) {
			Velocity = Velocity.Normalized() * MaxVelocity;
		}
		Velocity = Velocity.MoveToward(Vector2.Zero, (MaxVelocity / TimeToStop) * (float)delta);
	}
}
