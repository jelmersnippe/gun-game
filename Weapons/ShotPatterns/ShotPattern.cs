using Godot;
using Godot.Collections;

[GlobalClass]
public abstract partial class ShotPattern : Resource {
	public abstract Array<Projectile> CreateProjectiles(Weapon weapon);
}
