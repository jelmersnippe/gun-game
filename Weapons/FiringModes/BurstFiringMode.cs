using Godot;

[GlobalClass]
public partial class BurstFiringMode : FiringMode {
	[Export] public int ProjectileCount = 3;
	[Export] public float TimeBetweenProjectiles = 0.1f;

	public override void Fire(Weapon weapon) {
		int projectilesLeft = ProjectileCount;
		FireSingleRound(weapon, projectilesLeft);
	}

	private void FireSingleRound(Weapon weapon, int projectilesLeft) {
		if (projectilesLeft <= 0) {
			return;
		}

		ExecuteShotPattern(weapon);

		var timer = new Timer();
		timer.OneShot = true;
		timer.WaitTime = TimeBetweenProjectiles;
		weapon.AddChild(timer);
		timer.Start();

		timer.Timeout += () => { FireSingleRound(weapon, projectilesLeft - 1); };
	}
}
