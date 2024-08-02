using Godot;
using System;

public partial class Projectile : Area2D {
	[Export] public PackedScene Shell;
	[Export] public PackedScene ImpactEffect;
	[Export] public float Speed = 200f;

	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
	}

	private void OnAreaEntered(Area2D area)
	{
		var impactEffect = ImpactEffect.Instantiate<ImpactEffect>();
		impactEffect.RotationDegrees = RotationDegrees;
		impactEffect.GlobalPosition = GlobalPosition;
		
		GetTree().CurrentScene.CallDeferred("add_child", impactEffect);

		Engine.TimeScale = 0;

		var hitStopTimer = GetTree().CreateTimer(0.02f, ignoreTimeScale: true);
		hitStopTimer.Timeout += () => Engine.TimeScale = 1f;
	}

	public override void _Process(double delta)
	{
		Position += Transform.X * Speed * (float)delta;
	}
}
