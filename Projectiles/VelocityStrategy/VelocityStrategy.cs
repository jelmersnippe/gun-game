using Godot;

[GlobalClass]
public abstract partial class VelocityStrategy : Resource {
	public abstract void Apply(Projectile projectile);
}
