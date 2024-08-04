using System.Collections.Generic;
using Godot;

[GlobalClass]
public abstract partial class ShotPattern : Resource {
	public abstract List<Projectile> CreateProjectiles(Weapon weapon);
}
