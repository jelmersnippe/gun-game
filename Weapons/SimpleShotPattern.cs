using Godot;
using Godot.Collections;

[GlobalClass]
public partial class SimpleShotPattern : ShotPattern {
	[Export] public float Spread;

	public override Array<Projectile> CreateProjectiles(Weapon weapon) {
		var projectiles = new Array<Projectile>();

		var projectile = weapon.ProjectileScene.Instantiate<Projectile>();
		projectile.Rotation = weapon.GlobalRotation;
		projectile.GlobalPosition = weapon.FirePoint.GlobalPosition;

		// Apply spread
		var rng = new RandomNumberGenerator();
		projectile.RotationDegrees += rng.RandfRange(-Spread, Spread);

		projectiles.Add(projectile);

		// TODO: perform somewhere else that has access to scene tree
		// var shell = projectile.Shell.Instantiate<Shell>();
		// shell.GlobalPosition = weapon.ShellEjectionPoint.GlobalPosition;
		// shell.Rotation = direction.Angle();
		// shell.Velocity = new Vector2(-direction.Normalized().X * 50, -1 * 200);

		return projectiles;
	}
}
