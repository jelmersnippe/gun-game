using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class MultiShotPattern : ShotPattern {
	[Export] public float DistanceBetweenProjectiles = 10f;
	[Export] public int ProjectileCount;

	[Export] public float Spread;

	public override List<Projectile> CreateProjectiles(Weapon weapon) {
		var projectiles = new List<Projectile>();

		float middleProjectileIndex = (ProjectileCount - 1) / 2f;

		for (int i = 0; i < ProjectileCount; i++) {
			float offset = i - middleProjectileIndex;

			var projectile = weapon.ProjectileScene.Instantiate<Projectile>();
			projectile.Rotation = weapon.GlobalRotation;
			projectile.GlobalPosition = weapon.FirePoint.GlobalPosition;

			projectile.RotationDegrees += offset * DistanceBetweenProjectiles;

			// Apply spread
			var rng = new RandomNumberGenerator();
			projectile.RotationDegrees += rng.RandfRange(-Spread, Spread);

			projectiles.Add(projectile);
		}

		// TODO: perform somewhere else that has access to scene tree -> Reload
		// var shell = projectile.Shell.Instantiate<Shell>();
		// shell.GlobalPosition = weapon.ShellEjectionPoint.GlobalPosition;
		// shell.Rotation = direction.Angle();
		// shell.Velocity = new Vector2(-direction.Normalized().X * 50, -1 * 200);

		return projectiles;
	}
}
