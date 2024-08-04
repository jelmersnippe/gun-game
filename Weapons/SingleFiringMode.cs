using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class SingleFiringMode : FiringMode {
	public override List<Projectile> Fire(Weapon weapon) {
		return weapon.ShotPattern.CreateProjectiles(weapon);
	}
}
