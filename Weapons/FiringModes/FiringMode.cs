using Godot;
using Godot.Collections;

[GlobalClass]
public abstract partial class FiringMode : Resource {
	[Signal]
	public delegate void FiredEventHandler(Array<Projectile> projectiles);

	public abstract void Fire(Weapon weapon);

	private void PlaySound(Weapon weapon) {
		var soundPlayer = new AudioStreamPlayer2D();
		weapon.GetParent().AddChild(soundPlayer);
		soundPlayer.Stream = weapon.Sound;
		soundPlayer.Play();
		soundPlayer.Finished += () => soundPlayer.QueueFree();
	}

	protected void ExecuteShotPattern(Weapon weapon) {
		var projectiles = weapon.ShotPattern.CreateProjectiles(weapon);
		EmitSignal(SignalName.Fired, projectiles);
		PlaySound(weapon);
	}
}
