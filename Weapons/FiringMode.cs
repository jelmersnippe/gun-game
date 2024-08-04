using System.Collections.Generic;
using Godot;

[GlobalClass]
public abstract partial class FiringMode : Resource {
	public abstract List<Projectile> Fire(Weapon weapon);
}
