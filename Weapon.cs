using Godot;
using System;

public partial class Weapon : Node2D
{
	[Export] public Sprite2D Sprite;
	[Export] public PackedScene ProjectileScene;
	[Export] public Node2D FirePoint;
	[Export] public float AttacksPerSecond = 6;
	[Export] public bool IsAutomatic = false;

	[Export] public float Kickback = 5f;
	[Export] public float Spread = 5f;
	
	[Export] public int ProjectilesPerShot = 1;
	[Export] public float DistanceBetweenProjectiles = 10f;

	private bool _isReloading = false;
	private bool _waitingForTriggerRelease = false;

	private float ReloadTime => 1f / AttacksPerSecond;

	public void Fire(Vector2 direction)
	{
		if (_isReloading || _waitingForTriggerRelease)
		{
			return;
		}

		_isReloading = true;
		if (!IsAutomatic)
		{
			_waitingForTriggerRelease = true;
		}

		var middleProjectileIndex = (ProjectilesPerShot - 1) / 2f;
		GD.Print("Middle projectile: " + middleProjectileIndex);
		for (var i = 0; i < ProjectilesPerShot; i++)
		{
			var offset = i - middleProjectileIndex;
			GD.Print("Offset: " + offset);
			
			var projectile = ProjectileScene.Instantiate<Projectile>();
			projectile.GlobalPosition = FirePoint.GlobalPosition;
			projectile.Rotation = direction.Angle();
			
			projectile.RotationDegrees += offset * DistanceBetweenProjectiles;
			GD.Print("Rotation Adjustment: " + offset * DistanceBetweenProjectiles);
			
			// Apply spread
			var rng = new RandomNumberGenerator();
			projectile.RotationDegrees += rng.RandfRange(-Spread, Spread);
			
			GetTree().Root.CallDeferred("add_child", projectile);
		}
		
		// Apply kickback
		Position = Vector2.Zero - direction * Kickback;

		var reloadTimer = GetTree().CreateTimer(ReloadTime);
		reloadTimer.Timeout += () => _isReloading = false;
	}

	public override void _Process(double delta)
	{
		var directionToMouse = GlobalPosition.DirectionTo(GetGlobalMousePosition());
		Sprite.FlipV = directionToMouse.X < 0;

		
		Position = Position.MoveToward(Vector2.Zero, 30 * (float)delta);
	}

	public void StopFiring()
	{
		_waitingForTriggerRelease = false;
	}
}

