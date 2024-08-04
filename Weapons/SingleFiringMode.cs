using Godot;

[GlobalClass]
public partial class SingleFiringMode : FiringMode {
	public override void Fire(Weapon weapon) {
		ExecuteShotPattern(weapon);
	}
}
